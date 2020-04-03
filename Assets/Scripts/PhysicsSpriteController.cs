using UnityEngine;

public abstract class PhysicsSpriteController : MonoBehaviour
{
    protected new SpriteRenderer renderer = null;

    public abstract Bounds GetBounds();

    protected virtual void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
}