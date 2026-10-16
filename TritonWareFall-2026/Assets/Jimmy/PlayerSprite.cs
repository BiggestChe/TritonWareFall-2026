using UnityEngine;

// Shows the up or down sprite depending on which way the chef is facing.
// Put this on a child object that has the SpriteRenderer.
public class PlayerSprite : MonoBehaviour
{
    public MovementManager movement;
    public Sprite upSprite;
    public Sprite downSprite;

    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 facing = movement.facing;

        if (facing.y > 0.1f) sr.sprite = downSprite;
        else if (facing.y < -0.1f) sr.sprite = upSprite;

        // Mirror when moving left/right
        if (facing.x > 0.1f) sr.flipX = false;
        else if (facing.x < -0.1f) sr.flipX = true;
    }
}