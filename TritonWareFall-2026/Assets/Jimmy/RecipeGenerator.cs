using UnityEngine;
using System.Collections.Generic;

public class RecipeGenerator : MonoBehaviour
{
    public static RecipeGenerator Instance { get; private set; }
 
    [Header("Ingredients")]
    public IngredientType[] potions = { IngredientType.RedPotion, IngredientType.GreenPotion, IngredientType.BluePotion };
    public IngredientType[] physicalIngredients = { IngredientType.Mandrake, IngredientType.LizardTail };
    public bool allowSamePotionTwice = true;
 
    [Header("Orders")]
    public int maxActiveOrders = 4;
    public float orderTimeLimit = 60f;         // seconds a customer will wait
    public int pointsPerOrder = 100;
 
    [Header("Spawning (gets faster over the shift)")]
    public float firstOrderDelay = 2f;
    public float startSpawnInterval = 20f;
    public float minSpawnInterval = 8f;
    public float intervalDecreasePerOrder = 1f;
 
    // The current tickets, oldest first. The ticket UI reads this list.
    public List<Recipe> activeOrders = new List<Recipe>();
 
    float spawnTimer;
    float currentInterval;
 
    void Awake()
    {
        Instance = this;
    }
 
    void Start()
    {
        ResetOrders();
    }
 
    public void ResetOrders()
    {
        activeOrders.Clear();
        spawnTimer = firstOrderDelay;
        currentInterval = startSpawnInterval;
    }
 
    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing) return;
 
        if (activeOrders.Count == 0 && spawnTimer > 1f)
            spawnTimer = 1f;
 
        // Spawn new orders
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f && activeOrders.Count < maxActiveOrders)
        {
            activeOrders.Add(GenerateRecipe());
            spawnTimer = currentInterval;
            currentInterval = Mathf.Max(minSpawnInterval, currentInterval - intervalDecreasePerOrder);
        }
 
        // Tick down every order; expired orders count as failures
        for (int i = activeOrders.Count - 1; i >= 0; i--)
        {
            activeOrders[i].timeLeft -= Time.deltaTime;
            if (activeOrders[i].timeLeft <= 0f)
            {
                Debug.Log("Order failed: " + activeOrders[i].name);
                activeOrders.RemoveAt(i);
                GameManager.Instance.FailOrder();
            }
        }
    }

    // Generates a new random order with 2 potions and 1 physical ingredient.
    public Recipe GenerateRecipe()
    {
        IngredientType potionA = potions[Random.Range(0, potions.Length)];
        IngredientType potionB = potions[Random.Range(0, potions.Length)];
 
        if (!allowSamePotionTwice && potions.Length > 1)
        {
            while (potionB == potionA)
                potionB = potions[Random.Range(0, potions.Length)];
        }
 

        //pick one of the two physical ingredeients at random to add to the order
        IngredientType physical = physicalIngredients[Random.Range(0, physicalIngredients.Length)];
 

        Recipe recipe = new Recipe();
        recipe.ingredients.Add(potionA);
        recipe.ingredients.Add(potionB);
        recipe.ingredients.Add(physical);
        recipe.name = GetName(potionA) + " & " + GetName(potionB) + " with " + GetName(physical);
        recipe.points = pointsPerOrder;
        recipe.timeLimit = orderTimeLimit;
        recipe.timeLeft = orderTimeLimit;
 
        Debug.Log("New order: " + recipe.name);
        return recipe;
    }
 
    // Call this when the player hands a finished potion to the customer/serving window.
    public bool TryServe(List<IngredientType> dish)
    {
        // Check if the dish matches any active order
        for (int i = 0; i < activeOrders.Count; i++)
        {
            if (activeOrders[i].Matches(dish))
            {
                Recipe served = activeOrders[i];
                activeOrders.RemoveAt(i);
                GameManager.Instance.ServeOrder(served.points, served.TimeLeftFraction);
                Debug.Log("Order served: " + served.name);
                return true;
            }
        }
 
        Debug.Log("Wrong dish, no matching order");
        return false;
    }
 
    string GetName(IngredientType type)
    {
        switch (type)
        {
            case IngredientType.RedPotion:   return "Red";
            case IngredientType.GreenPotion: return "Green";
            case IngredientType.BluePotion:  return "Blue";
            case IngredientType.Mandrake:    return "Mandrake";
            case IngredientType.LizardTail:  return "Lizard Tail";
            default:                         return type.ToString();
        }
    }
}
 
