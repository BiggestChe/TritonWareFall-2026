using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUIController : MonoBehaviour
{
    public GameObject ticketPrefab;   // Drag your Ticket Prefab here in the Inspector
    public Transform ticketContainer; // Drag the Layout Group container here

    private void Start()
    {
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.onOrdersChanged += UpdateTicketUI;
        }
    }

    private void OnDestroy()
    {
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.onOrdersChanged -= UpdateTicketUI;
        }
    }

    void UpdateTicketUI()
    {
        // Clear old UI tickets
        foreach (Transform child in ticketContainer)
        {
            Destroy(child.gameObject);
        }

        // Rebuild current UI tickets from OrderManager list
        List<ActiveOrder> currentOrders = OrderManager.Instance.GetActiveOrders();
        foreach (var order in currentOrders)
        {
            GameObject ticketObj = Instantiate(ticketPrefab, ticketContainer);
            
            // Find the icon component inside your prefab and assign the recipe sprite
            Transform iconTransform = ticketObj.transform.Find("RecipeIcon");
            if (iconTransform != null)
            {
                Image iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null && order.recipe.recipeIcon != null)
                {
                    iconImage.sprite = order.recipe.recipeIcon;
                }
            }
        }
    }
}