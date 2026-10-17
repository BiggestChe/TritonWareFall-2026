using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // Required for clearing button focus

public class SelectIngredientButton : MonoBehaviour
{
    [Header("References")]
    public ChoppingStationUI stationUI;        // Drag your UI manager here
    public ChoppableItem choppableItem;        // Drag your "Stuff" object here

    private void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null && choppableItem != null && stationUI != null)
        {
            // Automatically update the button text to match the ingredient name
            TMP_Text label = GetComponentInChildren<TMP_Text>();
            if (label != null)
            {
                label.text = choppableItem.unchoppedName;
            }

            // When clicked, start chopping and unfocus the button so Spacebar works
            btn.onClick.AddListener(() => {
                stationUI.SelectIngredient(choppableItem.unchoppedName, choppableItem.requiredChops);
                
                // Clears button focus so Spacebar goes to the mini-game instead of the button
                EventSystem.current.SetSelectedGameObject(null);
            });
        }
    }
}