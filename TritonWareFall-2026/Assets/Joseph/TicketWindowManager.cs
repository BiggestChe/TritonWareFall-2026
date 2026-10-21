using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientIcon
{
    public IngredientType ingredientType;
    public Sprite iconSprite;
}

[System.Serializable]
public class FinishedPotionIconLookup
{
    public IngredientType first;
    public IngredientType second;
    public Sprite sprite;
}

public class TicketWindowManager : MonoBehaviour
{
    public TicketUI ticketUIPrefab;
    public Transform ticketUIParent;
    public List<IngredientIcon> ingredientIcons;
    public List<FinishedPotionIconLookup> finishedPotionIcons;

    Dictionary<IngredientType, Sprite> ingredientIconLookup = new Dictionary<IngredientType, Sprite>();
    Dictionary<Recipe, TicketUI> activeTicketUIs = new Dictionary<Recipe, TicketUI>();
    List<Recipe> toRemove = new List<Recipe>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        foreach (IngredientIcon entry in ingredientIcons)
        {
            ingredientIconLookup[entry.ingredientType] = entry.iconSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (RecipeGenerator.Instance == null) return;

        List<Recipe> orders = RecipeGenerator.Instance.activeOrders;

        // New orders get ticket
        foreach (Recipe order in orders)
        {
            if (!activeTicketUIs.ContainsKey(order))
            {
                List<Sprite> sprites = new List<Sprite>();
                foreach (IngredientType ingredient in order.ingredients)
                {
                    if (ingredientIconLookup.TryGetValue(ingredient, out Sprite icon))
                    {
                        sprites.Add(icon);
                    }
                    else
                    {
                        Debug.LogWarning("No icon found for ingredient: " + ingredient);
                    }
                }

                Sprite finishedSprite = null;
                if (order.ingredients.Count >= 2)
                {
                    finishedSprite = GetFinishedSprite(order.ingredients[0], order.ingredients[1]);
                }

                TicketUI ticket = Instantiate(ticketUIPrefab, ticketUIParent);
                ticket.Setup(finishedSprite, sprites);
                activeTicketUIs[order] = ticket;
            }
        }

        // Remove tickets that were served or expired
        toRemove.Clear();
        foreach (var pair in activeTicketUIs)
        {
            if (!orders.Contains(pair.Key))
            {
                toRemove.Add(pair.Key);
            }
        }
        foreach (Recipe recipe in toRemove)
        {
            Destroy(activeTicketUIs[recipe].gameObject);
            activeTicketUIs.Remove(recipe);
        }

        // Update timers

        foreach (var pair in activeTicketUIs)
        {
            pair.Value.SetFillAmount(pair.Key.TimeLeftFraction);
        }
    }

    Sprite GetFinishedSprite(IngredientType first, IngredientType second)
    {
        foreach (FinishedPotionIconLookup entry in finishedPotionIcons)
        {
            bool sameOrder = entry.first == first && entry.second == second;
            bool reverseOrder = entry.first == second && entry.second == first;
            if (sameOrder || reverseOrder) 
            {
                return entry.sprite;
            }
        }
        Debug.LogWarning("No finished sprite found for combination: " + first + " + " + second);
        return null;
    }
}
