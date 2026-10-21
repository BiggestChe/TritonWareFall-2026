using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// The chopping minigame. ChoppingBoard calls StartChopping() with the player's hand.
// Mash Space to chop; when done, the held ingredient becomes chopped.
// Escape cancels (ingredient stays unchopped).
public class ChoppingStationUI : MonoBehaviour
{
    [Header("UI Elements")]
    //
    public GameObject choppingGamePanel;       // the chopping panel
    public TMP_Text itemNameText;
    public TMP_Text progressText;
    public Slider progressBar;
    public Image ingredientDisplayImage;       // shows the held ingredient's sprite

    [Header("Chopping")]
    public int requiredChops = 10;

    [Header("Spacebar Visuals")]
    public Image spacebarPromptImage;
    public Sprite unpressedSprite;
    public Sprite pressedSprite;

    [Header("Knife Effect Visuals")]
    public GameObject knifeEffect1;
    public GameObject knifeEffect2;
    public float knifeDisplayDuration = 0.15f;

    public bool IsChopping { get; private set; }

    PlayerHand hand;
    MovementManager player;
    int chopCount;
    int knifeToggleCounter;
    Coroutine knifeRoutine;

    void Start()
    {
        if (choppingGamePanel != null) choppingGamePanel.SetActive(false);
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

        // Show what's being chopped
        if (ingredientDisplayImage != null && hand.heldSprite != null)
            ingredientDisplayImage.sprite = hand.heldSprite.sprite;
        if (spacebarPromptImage != null && unpressedSprite != null)
            spacebarPromptImage.sprite = unpressedSprite;

        if (progressBar != null)
        {
            progressBar.maxValue = requiredChops;
            progressBar.value = 0;
        }
        UpdateProgressUI();

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
        UpdateProgressUI();
        ShowAlternatingKnifeSlash();

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
        if (knifeEffect1 != null) knifeEffect1.SetActive(false);
        if (knifeEffect2 != null) knifeEffect2.SetActive(false);

        // Wait one frame so the last Space press doesn't also trigger a dash
        yield return null;
        player.enabled = true;
    }

    void UpdateProgressUI()
    {
        if (progressBar != null) progressBar.value = chopCount;
        if (progressText != null) progressText.text = chopCount + " / " + requiredChops;
    }

    // ---------- Knife effects ----------

    void ShowAlternatingKnifeSlash()
    {
        if (knifeRoutine != null) StopCoroutine(knifeRoutine);
        knifeRoutine = StartCoroutine(FlashKnifeRoutine());
    }

    IEnumerator FlashKnifeRoutine()
    {
        GameObject activeKnife = (knifeToggleCounter % 2 == 0) ? knifeEffect1 : knifeEffect2;
        knifeToggleCounter++;

        if (knifeEffect1 != null) knifeEffect1.SetActive(false);
        if (knifeEffect2 != null) knifeEffect2.SetActive(false);
        if (activeKnife != null) activeKnife.SetActive(true);

        yield return new WaitForSeconds(knifeDisplayDuration);

        if (activeKnife != null) activeKnife.SetActive(false);
    }
}