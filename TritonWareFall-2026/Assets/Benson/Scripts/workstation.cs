using UnityEngine;

public class Workstation : MonoBehaviour
{
    [SerializeField] ItemSelector menu;
    [SerializeField] KeyCode interactKey = KeyCode.E;
    bool playerNear;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<MovementManager>() != null) playerNear = true;
        Debug.Log("Player near workstation: " + playerNear);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<MovementManager>() != null) playerNear = false;
    }

    void Update()
    {
        if (playerNear && !menu.IsOpen && Input.GetKeyDown(interactKey))
            menu.Open();
    }
}