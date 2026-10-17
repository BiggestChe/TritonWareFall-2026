using UnityEngine;

public class KitchenInventory : MonoBehaviour
{
    public static KitchenInventory Instance;

    [Header("Inventory Counts")]
    public int frogLegCount = 2;          // Starting unchopped frog legs
    public int choppedFrogLegCount = 0;   // Finished chopped frog legs
    
    public int salamanderTailCount = 2;   // Starting unchopped tails
    public int choppedSalamanderTailCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Call this when chopping finishes successfully
    public void CompleteChop(string itemName)
    {
        if (itemName == "Frog Leg")
        {
            if (frogLegCount > 0) frogLegCount--;
            choppedFrogLegCount++;
            Debug.Log("Chopped a Frog Leg! Remaining: " + frogLegCount + " | Finished: " + choppedFrogLegCount);
        }
        else if (itemName == "Salamander Tail")
        {
            if (salamanderTailCount > 0) salamanderTailCount--;
            choppedSalamanderTailCount++;
            Debug.Log("Chopped a Salamander Tail! Remaining: " + salamanderTailCount + " | Finished: " + choppedSalamanderTailCount);
        }
    }
}