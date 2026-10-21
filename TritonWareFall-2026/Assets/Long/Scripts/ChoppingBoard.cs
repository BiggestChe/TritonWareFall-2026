using UnityEngine;

public class ChoppingBoard : MonoBehaviour
{
    [Header("UI Reference")]
    public ChoppingStationUI stationUI; // Drag your ChoppingStationUI GameObject here

    private void OnMouseDown()
    {
        // Opens the ingredient menu directly on click for easy testing (no player needed yet!)
        if (stationUI != null)
        {
            stationUI.OpenIngredientMenu();
            Debug.Log("Chopping board clicked! Opening ingredient menu.");
        }
        else
        {
            Debug.LogWarning("Station UI reference is missing on the ChoppingBoard!");
        }
    }
}