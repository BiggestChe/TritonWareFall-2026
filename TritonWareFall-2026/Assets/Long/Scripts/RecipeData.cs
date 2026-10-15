using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "CookingGame/RecipeData")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    public Sprite recipeIcon; // Icon displayed on the ticket
    public float basePatienceTime = 30f; // Time limit before order fails
    public int basePoints = 100; // Points awarded upon success
}