using UnityEngine;

// Walk up holding an unchopped physical ingredient to start chopping.
// Setup: give the board a Collider with "Is Trigger" ticked (the interaction zone).
public class ChoppingBoard : MonoBehaviour
{
    public ChoppingStationUI stationUI;
    [SerializeField] KeyCode interactKey = KeyCode.E;

    PlayerHand playerHand;           // set while the player is in range
    MovementManager playerMovement;

    void OnTriggerEnter(Collider other)
    {
        PlayerHand hand = other.GetComponentInParent<PlayerHand>();
        if (hand != null)
        {
            playerHand = hand;
            playerMovement = hand.GetComponent<MovementManager>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerHand>() != null)
            playerHand = null;
    }

    void Update()
    {
        if (playerHand == null || stationUI.IsChopping) return;
        if (playerMovement != null && !playerMovement.enabled) return;   // another menu is open
        if (!Input.GetKeyDown(interactKey)) return;

        if (!playerHand.HoldingUnchoppedIngredient)
        {
            Debug.Log("Bring an unchopped Mandrake or Lizard Tail to chop");
            return;
        }

        stationUI.StartChopping(playerHand, playerMovement);
    }
}