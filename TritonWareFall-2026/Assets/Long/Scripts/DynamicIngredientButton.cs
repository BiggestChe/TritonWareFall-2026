using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DynamicIngredientButton : MonoBehaviour
{
    [Header("References")]
    public ChoppingStationUI stationUI;        // Drag your OrderUIController here
    public string ingredientName = "Frog Leg"; // Type "Frog Leg" or "Salamander Tail"
    public int requiredChops = 5;              // How many chops needed

    private void OnEnable()
    {
        UpdateDisplayAndAvailability();
    }

    public void UpdateDisplayAndAvailability()
    {
        Button btn = GetComponent<Button>();
        TMP_Text label = GetComponentInChildren<TMP_Text>();

        int availableCount = 0;
        if (KitchenInventory.Instance != null)
        {
            if (ingredientName == "Frog Leg") availableCount = KitchenInventory.Instance.frogLegCount;
            else if (ingredientName == "Salamander Tail") availableCount = KitchenInventory.Instance.salamanderTailCount;
        }

        // Update button text to display name and stock count
        if (label != null)
        {
            label.text = ingredientName + " (x" + availableCount + ")";
        }

        // Disable button if out of stock
        if (btn != null)
        {
            bool hasStock = availableCount > 0;
            btn.interactable = hasStock;

            btn.onClick.RemoveAllListeners();
            if (hasStock)
            {
                btn.onClick.AddListener(() => {
                    stationUI.SelectIngredient(ingredientName, requiredChops);
                    EventSystem.current.SetSelectedGameObject(null);
                });
            }
        }
    }
}