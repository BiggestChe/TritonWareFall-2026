using UnityEngine;

public class MenuTester : MonoBehaviour
{
    [SerializeField] PotionMenu menu;

    void Start()
    {
        menu.OnPotionChosen += p => Debug.Log("Picked: " + p.potionName);
        menu.Open();   // open immediately on play
    }

    void Update()
    {
        // Reopen with T after picking or cancelling
        if (!menu.IsOpen && Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Reopening menu");
            menu.Open();
        }
    }
}
