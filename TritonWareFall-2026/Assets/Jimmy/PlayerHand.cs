using System.Collections.Generic;
using UnityEngine;

// What the chef is carrying. Holds ONE thing at a time
public class PlayerHand : MonoBehaviour
{
    public SpriteRenderer heldSprite;   // child object above the chef's head

    public List<IngredientType> contents = new List<IngredientType>();
    public bool holdingPotion;
    public bool prepared;               // physical ingredients must be chopped/ground before the cauldron

    public bool IsEmpty => contents.Count == 0;
    public bool HoldingIngredient => !IsEmpty && !holdingPotion;

    // Holding something that still needs the chopping board / grinder
    public bool NeedsChopping => HoldingIngredient && !prepared && IsChoppable(contents[0]);
    public bool NeedsGrinding => HoldingIngredient && !prepared && IsGrindable(contents[0]);

    void Start()
    {
        Clear();
    }

    public void HoldIngredient(IngredientType type, Sprite sprite)
    {
        contents.Clear();
        contents.Add(type);
        holdingPotion = false;
        prepared = false;
        Show(sprite);
        Debug.Log("Holding " + type);
    }

    // Called by the cauldron when the player takes out a finished potion
    public void HoldPotion(List<IngredientType> ingredients, Sprite sprite)
    {
        contents = new List<IngredientType>(ingredients);
        holdingPotion = true;
        prepared = false;
        Show(sprite);
        Debug.Log("Holding finished potion");
    }

    public void Clear()
    {
        contents.Clear();
        holdingPotion = false;
        prepared = false;
        Show(null);
    }

    public static bool IsPotion(IngredientType type)
    {
        return type == IngredientType.RedPotion
            || type == IngredientType.YellowPotion
            || type == IngredientType.BluePotion;
    }

    public static bool IsChoppable(IngredientType type)
    {
        return type == IngredientType.Frog_Leg || type == IngredientType.Salamander_Tail;
    }

    public static bool IsGrindable(IngredientType type)
    {
        return type == IngredientType.Bone || type == IngredientType.EyeBall;
    }

    void Show(Sprite sprite)
    {
        if (heldSprite == null) return;
        heldSprite.sprite = sprite;
        heldSprite.enabled = sprite != null;
    }
}