using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ChoppingStationUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject ingredientMenuPanel;     // Drag your IngredientSelectionPanel here
    public GameObject choppingGamePanel;       // Drag your ChoppingGamePanel here

    [Header("UI Elements")]
    public TMP_Text itemNameText;              // Drag your Item Name Text component here
    public TMP_Text progressText;              // Drag your ProgressText component here
    public Slider progressBar;                 // Drag your ChoppingProgressBar slider here

    [Header("Ingredient Visuals")]
    public Sprite frogLegSprite;               // Drag your Frog Leg sprite here
    public Sprite salamanderTailSprite;        // Drag your Salamander Tail sprite here
    public UnityEngine.UI.Image ingredientDisplayImage; // Image in chopping panel to show selected food

    [Header("Spacebar Visuals")]
    public UnityEngine.UI.Image spacebarPromptImage; // Drag your SpacebarPrompt UI object here
    public Sprite unpressedSprite;                   // Drag your unpressed PNG here
    public Sprite pressedSprite;                     // Drag your pressed PNG here

    [Header("Knife Effect Visuals")]
    public GameObject knifeEffect1;            // Drag your 1st knife effect object here
    public GameObject knifeEffect2;            // Drag your 2nd knife effect object here
    public float knifeDisplayDuration = 0.15f; 
    private int knifeToggleCounter = 0;        
    private Coroutine knifeRoutine;

    [HideInInspector] public bool isChoppingActive = false;
    private string currentIngredientName;
    private int currentRequiredChops;
    private int currentChopCount;

    private void Start()
    {
        CloseAllPanels();
    }

    private void Update()
    {
        if (!isChoppingActive) return;

        // Animate the spacebar prompt graphic scaling
        if (spacebarPromptImage != null)
        {
            float scale = 1f + Mathf.PingPong(Time.time * 3f, 0.2f);
            spacebarPromptImage.rectTransform.localScale = new Vector3(scale, scale, 1f);
        }

        // Detect Spacebar press (down)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RegisterChop();

            if (spacebarPromptImage != null && pressedSprite != null)
            {
                spacebarPromptImage.sprite = pressedSprite;
            }

            ShowAlternatingKnifeSlash();
        }

        // Detect Spacebar release (up)
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (spacebarPromptImage != null && unpressedSprite != null)
            {
                spacebarPromptImage.sprite = unpressedSprite;
            }
        }
    }

    // Called by the ChoppingBoard when clicked
    public void OpenIngredientMenu()
    {
        CloseAllPanels();
        if (ingredientMenuPanel != null)
        {
            ingredientMenuPanel.SetActive(true);
        }
    }

    // Called when player clicks an ingredient button
    public void SelectIngredient(string ingredientName, int requiredChops)
    {
        currentIngredientName = ingredientName;
        currentRequiredChops = requiredChops;
        currentChopCount = 0;

        // Update title text
        if (itemNameText != null)
        {
            itemNameText.text = currentIngredientName;
        }

        // Update ingredient picture graphic
        if (ingredientDisplayImage != null)
        {
            if (ingredientName == "Frog Leg" && frogLegSprite != null)
            {
                ingredientDisplayImage.sprite = frogLegSprite;
                ingredientDisplayImage.gameObject.SetActive(true);
            }
            else if (ingredientName == "Salamander Tail" && salamanderTailSprite != null)
            {
                ingredientDisplayImage.sprite = salamanderTailSprite;
                ingredientDisplayImage.gameObject.SetActive(true);
            }
        }

        // Setup slider
        if (progressBar != null)
        {
            progressBar.maxValue = currentRequiredChops;
            progressBar.value = 0;
        }
        UpdateProgressUI();

        // Switch panels
        if (ingredientMenuPanel != null) ingredientMenuPanel.SetActive(false);
        if (choppingGamePanel != null) choppingGamePanel.SetActive(true);

        isChoppingActive = true;
    }

    private void RegisterChop()
    {
        currentChopCount++;
        UpdateProgressUI();

        if (currentChopCount >= currentRequiredChops)
        {
            FinishChopping();
        }
    }

    private void UpdateProgressUI()
    {
        if (progressBar != null)
        {
            progressBar.value = currentChopCount;
        }

        if (progressText != null)
        {
            progressText.text = currentChopCount + " / " + currentRequiredChops;
        }
    }

    private void FinishChopping()
    {
        isChoppingActive = false;

        // Complete the inventory reduction and add chopped item count
        if (KitchenInventory.Instance != null)
        {
            KitchenInventory.Instance.CompleteChop(currentIngredientName);
        }

        CloseAllPanels();
    }

    public void CloseAllPanels()
    {
        isChoppingActive = false;
        if (ingredientMenuPanel != null) ingredientMenuPanel.SetActive(false);
        if (choppingGamePanel != null) choppingGamePanel.SetActive(false);
    }

    private void ShowAlternatingKnifeSlash()
    {
        if (knifeRoutine != null)
        {
            StopCoroutine(knifeRoutine);
        }
        knifeRoutine = StartCoroutine(FlashKnifeRoutine());
    }

    private IEnumerator FlashKnifeRoutine()
    {
        GameObject activeKnife = (knifeToggleCounter % 2 == 0) ? knifeEffect1 : knifeEffect2;
        knifeToggleCounter++;

        if (knifeEffect1 != null) knifeEffect1.SetActive(false);
        if (knifeEffect2 != null) knifeEffect2.SetActive(false);

        if (activeKnife != null)
        {
            activeKnife.SetActive(true);
        }

        yield return new WaitForSeconds(knifeDisplayDuration);

        if (activeKnife != null)
        {
            activeKnife.SetActive(false);
        }
    }
}