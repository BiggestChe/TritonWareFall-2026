using System.Collections.Generic;
using UnityEngine;

// What the chef is carrying. Holds ONE thing at a time
public class PlayerHand : MonoBehaviour
{
    public SpriteRenderer heldSprite;   // child object above the chef's head

    public List<IngredientType> contents = new List<IngredientType>();
    public bool holdingPotion;

    public bool IsEmpty => contents.Count == 0;
    public bool HoldingIngredient => !IsEmpty && !holdingPotion;

    void Start()
    {
        Clear();
    }

    
    public void HoldIngredient(IngredientType type, Sprite sprite)
    {
        contents.Clear();
        contents.Add(type);
        holdingPotion = false;
        Show(sprite);
        Debug.Log("Holding " + type);
    }

    //potion is then used with match() to determine correct ingredients or not
    public void HoldPotion(List<IngredientType> ingredients, Sprite sprite)
    {
        contents = new List<IngredientType>(ingredients);
        holdingPotion = true;
        Show(sprite);
        Debug.Log("Holding finished potion");
    }

    public void Clear()
    {
        contents.Clear();
        holdingPotion = false;
        Show(null);
    }

    void Show(Sprite sprite)
    {
        if (heldSprite == null) return;
        heldSprite.sprite = sprite;
        heldSprite.enabled = sprite != null;
    }
}