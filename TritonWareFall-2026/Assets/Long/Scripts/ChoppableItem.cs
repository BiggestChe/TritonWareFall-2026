using UnityEngine;

public class ChoppableItem : MonoBehaviour
{
    [Header("Item Info")]
    public string unchoppedName = "Stuff";
    public string choppedName = "Chopped Stuff";

    [Header("Chopping Settings")]
    public int requiredChops = 10;
    private int currentChops = 0;

    [Header("Visual Feedback")]
    public Sprite choppedSprite;
    private SpriteRenderer spriteRenderer;

    public bool IsChopped { get; private set; } = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Chop()
    {
        if (IsChopped) return;

        currentChops++;
        Debug.Log($"[Chopping] Progress: {currentChops}/{requiredChops}");

        if (currentChops >= requiredChops)
        {
            CompleteChopping();
        }
    }

    private void CompleteChopping()
    {
        IsChopped = true;
        gameObject.name = choppedName;

        if (choppedSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = choppedSprite;
        }

        Debug.Log($"<color=green>SUCCESS:</color> {choppedName} is fully chopped!");
    }
}