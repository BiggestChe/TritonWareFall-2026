using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// The chopping minigame. ChoppingBoard calls StartChopping() with the player's hand.
// Mash Space to chop; when done, the held ingredient becomes chopped.
// Escape cancels (ingredient stays unchopped).
public class ChoppingStationUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject choppingGamePanel;       // screen panel with the spacebar prompt + knife effects
    public Slider progressBar;                 // world-space bar above the chopping board

    [Header("Chopping")]
    public int requiredChops = 10;

    [Header("Spacebar Visuals")]
    public Image spacebarPromptImage;
    public Sprite unpressedSprite;
    public Sprite pressedSprite;

    public bool IsChopping { get; private set; }

    PlayerHand hand;
    MovementManager player;
    int chopCount;

    void Start()
    {
        if (choppingGamePanel != null) choppingGamePanel.SetActive(false);
        if (progressBar != null) progressBar.gameObject.SetActive(false);
    }

    public void StartChopping(PlayerHand playerHand, MovementManager movement)
    {
        hand = playerHand;
        player = movement;
        chopCount = 0;

        // Freeze the chef (same as ItemSelector)
        player.input = Vector3.zero;
        player.rb.linearVelocity = Vector3.zero;
        player.enabled = false;

        if (spacebarPromptImage != null && unpressedSprite != null)
            spacebarPromptImage.sprite = unpressedSprite;

        if (progressBar != null)
        {
            progressBar.maxValue = requiredChops;
            progressBar.value = 0;
            progressBar.gameObject.SetActive(true);
        }

        if (choppingGamePanel != null) choppingGamePanel.SetActive(true);
        IsChopping = true;
    }

    void Update()
    {
        if (!IsChopping) return;

        // Pulse the spacebar prompt
        if (spacebarPromptImage != null)
        {
            float scale = 1f + Mathf.PingPong(Time.time * 3f, 0.2f);
            spacebarPromptImage.rectTransform.localScale = new Vector3(scale, scale, 1f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (spacebarPromptImage != null && pressedSprite != null)
                spacebarPromptImage.sprite = pressedSprite;
            Chop();
        }

        if (Input.GetKeyUp(KeyCode.Space) && spacebarPromptImage != null && unpressedSprite != null)
            spacebarPromptImage.sprite = unpressedSprite;

        if (Input.GetKeyDown(KeyCode.Escape))
            StartCoroutine(StopChopping(false));
    }

    void Chop()
    {
        chopCount++;
        if (progressBar != null) progressBar.value = chopCount;

        if (AudioManager.Instance != null) AudioManager.Instance.Play("Chop");

        if (chopCount >= requiredChops)
            StartCoroutine(StopChopping(true));
    }

    IEnumerator StopChopping(bool finished)
    {
        IsChopping = false;

        if (finished)
        {
            hand.chopped = true;
        }

        if (choppingGamePanel != null) choppingGamePanel.SetActive(false);
        if (progressBar != null) progressBar.gameObject.SetActive(false);

        // Wait one frame so the last Space press doesn't also trigger a dash
        yield return null;
        player.enabled = true;
    }


}