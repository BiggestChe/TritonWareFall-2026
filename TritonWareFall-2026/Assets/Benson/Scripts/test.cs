using UnityEngine;

public class MenuTester : MonoBehaviour
{
    [SerializeField] ItemSelector menu;

    // void Start()
    // {
    //     menu.OnItemChosen += i => Debug.Log("Picked: " + i.itemName);
    //     menu.Open();   // open immediately on play
    // }

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
