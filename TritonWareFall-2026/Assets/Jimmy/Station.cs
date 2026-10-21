using System.Collections.Generic;
using UnityEngine;

public enum StationType { Cauldron, Serving, Trash }

// One script for the cauldron, serving window and trash. Pick the Type in the Inspector.
// (Ingredient shelves use Workstation.)
public class Station : MonoBehaviour
{
    public StationType type;
    [SerializeField] KeyCode interactKey = KeyCode.E;

    [Header("Cauldron")]
    public float brewTime = 3f;
    [SerializeField] Sprite finishedPotionSprite;
    public List<IngredientType> contents = new List<IngredientType>();

    PlayerHand playerHand;       // set while the player is in range
    MovementManager playerMovement;
    bool brewing;
    bool potion_ready;
    float brewTimer;

    // 0 to 1, for a progress bar above the cauldron
    public float BrewProgress => potion_ready ? 1f : brewing ? 1f - brewTimer / brewTime : 0f;

    void OnTriggerEnter(Collider other)
    {
        PlayerHand hand = other.GetComponentInParent<PlayerHand>();
        if (hand != null)
        {
            Debug.Log("Player entered " + type + " station");
            playerHand = hand;
            playerMovement = hand.GetComponent<MovementManager>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerHand>() != null)
            playerHand = null;
    }

    void Update()
    {
        // Brewing keeps going even if the player walks away
        if (brewing)
        {
            brewTimer -= Time.deltaTime;
            if (brewTimer <= 0f)
            {
                brewing = false;
                potion_ready = true;
                AudioManager.Instance.Play("Cooked");
                Debug.Log("Potion ready!");
            }
        }

        if (playerHand == null) return;
        if (playerMovement != null && !playerMovement.enabled) return;
        if (!Input.GetKeyDown(interactKey)) return;

        //check for which station the player is interacting with and call the appropriate function
        switch (type)
        {
            case StationType.Cauldron: UseCauldron();
             AudioManager.Instance.Play("Put");

             break;
            case StationType.Serving:  UseServing();  break;
            case StationType.Trash:    UseTrash();    break;
        }
    }

    // ---------- Cauldron ----------

    void UseCauldron()
    {
        // Take out a finished potion
        if (potion_ready)
        {

            if (!playerHand.IsEmpty) { Debug.Log("Empty your hands to take the potion"); return; }
            playerHand.HoldPotion(contents, finishedPotionSprite);
            contents.Clear();
            potion_ready = false;
            return;
        }

        if (brewing) { Debug.Log("Still brewing..."); return; }
        if (!playerHand.HoldingIngredient) return;

        IngredientType ingredient = playerHand.contents[0];
        if (!CanAdd(ingredient)) { Debug.Log("The cauldron doesn't need that"); return; }

        contents.Add(ingredient);
        playerHand.Clear();
        Debug.Log("Added " + ingredient + " (" + contents.Count + "/3)");

        if (contents.Count == 3)
        {
            brewing = true;
            brewTimer = brewTime;
            AudioManager.Instance.Play("Cooking");
        }
    }

    // Formula: max 2 potions and max 1 physical ingredient
    bool CanAdd(IngredientType ingredient)
    {
        int potions = 0, physical = 0;
        foreach (IngredientType c in contents)
        {
            if (IsPotion(c)) potions++;
            else physical++;
        }
        return IsPotion(ingredient) ? potions < 2 : physical < 1;
    }

    static bool IsPotion(IngredientType t)
    {
        return t == IngredientType.RedPotion || t == IngredientType.YellowPotion || t == IngredientType.BluePotion;
    }

    // ---------- Serving ----------

    void UseServing()
    {
        if (!playerHand.holdingPotion) { Debug.Log("Bring a finished potion"); return; }

        if (RecipeGenerator.Instance.TryServe(playerHand.contents))
        {
            playerHand.Clear();
            Debug.Log("S");
        }
        else
        {
            Debug.Log("No customer ordered that potion");
        }  
    }

    // ---------- Trash ----------

    void UseTrash()
    {
        if (playerHand.IsEmpty) return;
        playerHand.Clear();
        Debug.Log("Trashed it");
    }
}