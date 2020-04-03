using UnityEngine;

public class Gimbal : Manager<Gimbal>
{
    private Vector3 smoothScale;

    [SerializeField]
    private PlayerController target = null;

    [SerializeField]
    [Range(1, 15)]
    private float m_speed = 5f;

    [SerializeField]
    private Transform cameraHolder = null;

    private void Awake()
    {
        smoothScale = cameraHolder.localPosition;
    }

    public void SetTarget(PlayerController target)
    {
        this.target = target;
    }

    private void LateUpdate()
    {
        float delta = Time.deltaTime;

        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, m_speed * delta);
            float change = InputManager.Instance.GetWheel();
            if (change != 0 && smoothScale.z + change <= -4 && smoothScale.z + change >= -14)
            {
                smoothScale += new Vector3(0, 0, change);
            }
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, smoothScale, m_speed * delta);
        }
    }
}