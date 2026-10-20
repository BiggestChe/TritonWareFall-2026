using System.Collections.Generic;
using UnityEngine;
 
// One customer order: 2 potions + 1 physical ingredient.
[System.Serializable]
public class Recipe
{
    public string name;
    public List<IngredientType> ingredients = new List<IngredientType>();
    public int points;
    public float timeLimit;
    public float timeLeft;
 
    // 1 = just ordered, 0 = out of time (use for the ticket's patience bar)
    public float TimeLeftFraction => timeLeft / timeLimit;
 
    // True if the given ingredients match this recipe (order doesn't matter)
    public bool Matches(List<IngredientType> given)
    {
        if (given.Count != ingredients.Count) return false;
 
        List<IngredientType> remaining = new List<IngredientType>(ingredients);
        foreach (IngredientType ingredient in given)
        {
            if (!remaining.Remove(ingredient)) return false;
        }
        return true;
    }
}