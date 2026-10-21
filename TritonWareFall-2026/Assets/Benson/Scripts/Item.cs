using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public IngredientType type;
    [SerializeField] ItemSelector menu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!menu.IsOpen)
                menu.Open();
        }
    }
}
