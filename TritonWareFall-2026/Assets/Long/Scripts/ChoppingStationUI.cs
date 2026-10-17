using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; 

public class ChoppingStationUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject ingredientMenuPanel;
    public GameObject choppingGamePanel;

    [Header("Chopping Game UI Elements")]
    public TMP_Text itemNameText;
    public Slider progressBar;
    public TMP_Text progressText;
    public Transform spacebarPromptGraphic;

    [Header("State")]
    private string selectedIngredientName;
    private int currentChops = 0;
    private bool isChoppingActive = false;

    [Header("Spacebar Visuals")]
    public UnityEngine.UI.Image spacebarPromptImage; // Drag your SpacebarPrompt UI object here
    public Sprite unpressedSprite;                   // Drag your unpressed PNG here
    public Sprite pressedSprite;                     // Drag your pressed PNG here

    [Header("Knife Effect Visuals")]
    public GameObject knifeEffect1;            // Drag your 1st knife effect object here
    public GameObject knifeEffect2;            // Drag your 2nd knife effect object here
    public float knifeDisplayDuration = 0.15f; 
    private int knifeToggleCounter = 0;        // Keeps track of odd/even presses
    private Coroutine knifeRoutine;

    private void Update()
    {
        if (!isChoppingActive) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RegisterChop();

            if (spacebarPromptImage != null && pressedSprite != null)
            {
                spacebarPromptImage.sprite = pressedSprite;
            }

            ShowAlternatingKnifeSlash();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (spacebarPromptImage != null && unpressedSprite != null)
            {
                spacebarPromptImage.sprite = unpressedSprite;
            }
        }
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
        // Decide which knife to show based on odd/even (modulo operator %)
        GameObject activeKnife = (knifeToggleCounter % 2 == 0) ? knifeEffect1 : knifeEffect2;
        knifeToggleCounter++; // Increment counter for the next press

        // Ensure both are hidden first
        if (knifeEffect1 != null) knifeEffect1.SetActive(false);
        if (knifeEffect2 != null) knifeEffect2.SetActive(false);

        // Turn on the selected knife effect
        if (activeKnife != null)
        {
            activeKnife.SetActive(true);
        }

        yield return new WaitForSeconds(knifeDisplayDuration);

        // Hide it after the duration
        if (activeKnife != null)
        {
            activeKnife.SetActive(false);
        }
    }

    public void OpenIngredientMenu()
    {
        if (ingredientMenuPanel != null) ingredientMenuPanel.SetActive(true);
        if (choppingGamePanel != null) choppingGamePanel.SetActive(false);
    }

    public void SelectIngredient(string ingredientName, int totalChopsNeeded)
    {
        selectedIngredientName = ingredientName;
        currentChops = 0;
        isChoppingActive = true;

        if (ingredientMenuPanel != null) ingredientMenuPanel.SetActive(false);
        if (choppingGamePanel != null) choppingGamePanel.SetActive(true);

        if (itemNameText != null) itemNameText.text = "Chopping: " + ingredientName;
        if (progressBar != null)
        {
            progressBar.maxValue = totalChopsNeeded;
            progressBar.value = 0;
        }
        UpdateUI();
    }

    private void RegisterChop()
    {
        currentChops++;
        UpdateUI();

        if (progressBar != null && currentChops >= progressBar.maxValue)
        {
            CompleteChopping();
        }
    }

    private void UpdateUI()
    {
        if (progressBar != null) progressBar.value = currentChops;
        if (progressText != null && progressBar != null) 
            progressText.text = $"{currentChops} / {progressBar.maxValue}";
    }

    private void CompleteChopping()
    {
        isChoppingActive = false;
        if (choppingGamePanel != null) choppingGamePanel.SetActive(false);
        Debug.Log($"<color=green>Chopping Complete!</color> Finished chopping {selectedIngredientName}.");
    }

    public void CloseAllPanels()
    {
        isChoppingActive = false;
        if (ingredientMenuPanel != null) ingredientMenuPanel.SetActive(false);
        if (choppingGamePanel != null) choppingGamePanel.SetActive(false);
    }

    private void Start()
    {
        // Ensure panels are hidden when the game starts
        CloseAllPanels();
    }
}