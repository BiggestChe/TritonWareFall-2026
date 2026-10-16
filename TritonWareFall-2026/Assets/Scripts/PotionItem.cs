using UnityEngine;

public class PotionItem : MonoBehaviour
{
    public string potionName;
    [SerializeField] PotionMenu menu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!menu.IsOpen)
                menu.Open();
        }
    }
}
