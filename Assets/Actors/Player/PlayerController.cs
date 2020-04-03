using System.Collections.Generic;
using UnityEngine;
using static LevelManager;

public class PlayerController : PhysicsSpriteController
{
    private const float Epsilon = 0.001f;

    [SerializeField]
    private float ySpeedMax;
    private float ySpeed;

    [SerializeField]
    private float xSpeed;

    private Vector3 boundsSize;
    private float rotationSpeed;

    [SerializeField]
    private float jetPower;

    private bool isFalling;
    private bool isMaxJump;
    private bool isFinished;

    private List<KeyAction> actions;

    public int lastPlatform { get; private set; }

    protected override void Awake()
    {
        actions = new List<KeyAction>()
        {
            new KeyAction(KeyInputMode.KeyUp, InputManager.Jump, () => isMaxJump = isFalling),
            new KeyAction(KeyInputMode.KeyPressed, InputManager.Jump, AddForce),
            new KeyAction(KeyInputMode.KeyDown, InputManager.Jump, Jump)
        };

        foreach (KeyAction action in actions)
            InputManager.Instance.AddKeyAction(action);

        xSpeed = 2 + LevelManager.Instance.Level;
        ySpeedMax = 11;
        jetPower = 4;

        base.Awake();

        Gimbal.Instance.SetTarget(this);
        boundsSize = renderer.bounds.size;
        rotationSpeed = Mathf.PI * boundsSize.x * xSpeed;
    }

    private void OnDestroy()
    {
        if (!InputManager.Instance || !LevelManager.Instance)
            return;

        foreach (KeyAction action in actions)
            InputManager.Instance.RemoveKeyActions(action);

        if (isFinished)
            LevelManager.Instance.Win();
        else
            LevelManager.Instance.Lose();
    }

    public override Bounds GetBounds()
    {
        return new Bounds(renderer.bounds.center, boundsSize);
    }

    private void AddForce()
    {
        if (isFalling && !isMaxJump)
        {
            float dv = ySpeedMax * Time.deltaTime * jetPower;
            if (ySpeed + dv < ySpeedMax)
                ySpeed += dv;
            else
            {
                ySpeed = ySpeedMax;
                isMaxJump = true;
            }
        }
    }

    private void Jump()
    {
        if (!isFalling && ySpeed == 0)
        {
            isFalling = true;
            AddForce();
        }
    }

    private void Stop()
    {
        isMaxJump = false;
        isFalling = false;
        ySpeed = 0;
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        float dv = dt * Gravity;

        float dx = dt * xSpeed;

        ySpeed -= dv;
        float dy = dt * ySpeed;

        if (CheckCollisions(ref dy))
            Stop();

        transform.Rotate(0, 0, -rotationSpeed, Space.Self);
        transform.Translate(Vector3.right * dx + Vector3.up * dy, Space.World);

        if (transform.position.y < -30)
            Destroy(gameObject);
    }

    private bool CheckCollisions(ref float dy)
    {
        List<PlatformController> platforms = LevelManager.Instance.Platforms;
        for (int i = lastPlatform; i < platforms.Count; i++)
        {
            // check nearest platform
            if (GetBounds().max.x > platforms[i].GetBounds().min.x
                && GetBounds().min.x < platforms[i].GetBounds().max.x)
            {
                // if the ball is above the platform
                if (GetBounds().min.y - platforms[i].GetBounds().max.y >= -Epsilon)
                {
                    lastPlatform = i;
                    float border = platforms[i].GetBounds().max.y;

                    // if the ball is on the platform
                    if (GetBounds().min.y + dy < border)
                    {
                        if (Mathf.Abs(GetBounds().min.y - border) > Epsilon)
                        {
                            AudioManager.Instance.Play("Hit");
                            if (lastPlatform == platforms.Count - 1)
                            {
                                isFinished = true;
                                Destroy(gameObject);
                            }
                        }

                        dy = border - GetBounds().min.y;
                        return true;
                    }
                }
                else if (GetBounds().max.y > platforms[i].GetBounds().min.y && !LevelManager.Instance.isWallKicks)
                {
                    AudioManager.Instance.Play("Bump");
                    Destroy(gameObject);
                }

                break;
            }
        }
        return false;
    }
}