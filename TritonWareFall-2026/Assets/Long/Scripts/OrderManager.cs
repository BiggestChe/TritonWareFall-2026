using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveOrder
{
    public RecipeData recipe;
    public float timeRemaining;
    public float maxTime;

    public ActiveOrder(RecipeData recipeData)
    {
        recipe = recipeData;
        maxTime = recipeData.basePatienceTime;
        timeRemaining = maxTime;
    }
}

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [Header("Available Recipes")]
    public List<RecipeData> allRecipes; // Drag your RecipeData assets here in the Inspector

    [Header("Order Settings")]
    public float spawnInterval = 10f; 
    public int maxActiveOrders = 3;

    private List<ActiveOrder> activeOrders = new List<ActiveOrder>();
    private float spawnTimer;

    public delegate void OnOrdersChanged();
    public event OnOrdersChanged onOrdersChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // Handle order generation timing
        if (activeOrders.Count < maxActiveOrders)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnRandomOrder();
                spawnTimer = 0f;
            }
        }

        // Countdown active order timers
        for (int i = activeOrders.Count - 1; i >= 0; i--)
        {
            activeOrders[i].timeRemaining -= Time.deltaTime;

            if (activeOrders[i].timeRemaining <= 0)
            {
                FailOrder(i);
            }
        }
    }

    public void SpawnRandomOrder()
    {
        if (allRecipes.Count == 0) return;

        RecipeData randomRecipe = allRecipes[Random.Range(0, allRecipes.Count)];
        ActiveOrder newOrder = new ActiveOrder(randomRecipe);
        activeOrders.Add(newOrder);

        onOrdersChanged?.Invoke();
        Debug.Log("New order spawned: " + randomRecipe.recipeName);
    }

    private void FailOrder(int index)
    {
        Debug.Log("Order failed: " + activeOrders[index].recipe.recipeName);
        activeOrders.RemoveAt(index);
        onOrdersChanged?.Invoke();
    }

    public List<ActiveOrder> GetActiveOrders()
    {
        return activeOrders;
    }
}