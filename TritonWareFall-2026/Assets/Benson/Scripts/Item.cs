using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
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
