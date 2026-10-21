using System.Collections;
using UnityEngine;

// Walk up holding an unground Bone / Eyeball and press E to start the grinding minigame.

public class GrindingStation : MonoBehaviour
{
    public GrindingMinigame minigame;
    public bool ruinOnFail = true;
    [SerializeField] KeyCode interactKey = KeyCode.E;

    PlayerHand playerHand;           // set while the player is in range
    MovementManager playerMovement;
    PlayerHand grindingHand;         // the hand being ground for (null when idle)

    void Start()
    {
        minigame.Finished += OnFinished;
    }

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
        if (playerHand == null || grindingHand != null) return;
        if (playerMovement != null && !playerMovement.enabled) return;   // another menu is open
        if (!Input.GetKeyDown(interactKey)) return;

        if (!playerHand.NeedsGrinding)
        {
            Debug.Log("Bring an unground Bone or Eyeball to grind");
            return;
        }

        grindingHand = playerHand;
        minigame.Begin();
    }

    void OnFinished(bool won)
    {
        if (grindingHand == null) return;   // another grinding station started it

        if (won)
        {
            grindingHand.prepared = true;
            Debug.Log("Ground it!");
        }
        else if (ruinOnFail)
        {
            grindingHand.Clear();
            Debug.Log("Ruined it! Get another one.");
        }

        grindingHand = null;
    }
}