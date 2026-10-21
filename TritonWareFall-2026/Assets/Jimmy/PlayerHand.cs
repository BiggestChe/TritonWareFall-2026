using System.Collections.Generic;
using UnityEngine;

// What the chef is carrying. Holds ONE thing at a time
public class PlayerHand : MonoBehaviour
{
    public SpriteRenderer heldSprite;   // child object above the chef's head

    public List<IngredientType> contents = new List<IngredientType>();
    public bool holdingPotion;
    public bool chopped;                // physical ingredients must be chopped before the cauldron

    public bool IsEmpty => contents.Count == 0;
    public bool HoldingIngredient => !IsEmpty && !holdingPotion;

    // Holding a Mandrake / Lizard Tail that still needs chopping
    public bool HoldingUnchoppedIngredient => HoldingIngredient && !chopped && !IsPotion(contents[0]);

    void Start()
    {
        Clear();
    }

    public void HoldIngredient(IngredientType type, Sprite sprite)
    {
        contents.Clear();
        contents.Add(type);
        holdingPotion = false;
        chopped = false;
        Show(sprite);
        Debug.Log("Holding " + type);
    }

    // Called by the cauldron when the player takes out a finished potion
    public void HoldPotion(List<IngredientType> ingredients, Sprite sprite)
    {
        contents = new List<IngredientType>(ingredients);
        holdingPotion = true;
        chopped = false;
        Show(sprite);
        Debug.Log("Holding finished potion");
    }

    public void Clear()
    {
        contents.Clear();
        holdingPotion = false;
        chopped = false;
        Show(null);
    }

    public static bool IsPotion(IngredientType type)
    {
        return type == IngredientType.RedPotion
            || type == IngredientType.YellowPotion
            || type == IngredientType.BluePotion;
    }

    void Show(Sprite sprite)
    {
        if (heldSprite == null) return;
        heldSprite.sprite = sprite;
        heldSprite.enabled = sprite != null;
    }
}