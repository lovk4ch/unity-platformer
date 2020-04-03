using UnityEngine;

public class PlatformController : PhysicsSpriteController
{
    [SerializeField]
    private SpriteRenderer blockPrefab = null;

    private ParticleSystem sparks;

    public float Length { get; private set; }

    public override Bounds GetBounds()
    {
        return renderer.bounds;
    }

    public void Expand(int blocks)
    {
        renderer = Instantiate(blockPrefab, transform);
        Length = 0.5f * blocks;
        renderer.size = new Vector2(Length, renderer.size.y);

        sparks = renderer.GetComponentInChildren<ParticleSystem>();
        var shape = sparks.shape;
        shape.radius *= blocks;
        var emission = sparks.emission;
        emission.rateOverTimeMultiplier *= blocks;

        transform.Translate(Vector3.right * 0.5f * Length);
    }
}