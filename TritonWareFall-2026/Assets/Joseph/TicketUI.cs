using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicketUI : MonoBehaviour
{
    public Image finishedImage;
    public Image[] ingredientImages;
    public Image timerFill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Places images into image slots
    public void Setup(Sprite finished, List<Sprite> ingredients)
    {
        finishedImage.sprite = finished;
        finishedImage.enabled = finished != null;

        for (int i = 0; i < ingredientImages.Length; i++)
        {
            bool spriteExists = i < ingredients.Count && ingredients[i] != null;
            ingredientImages[i].gameObject.SetActive(spriteExists);
            if (spriteExists)
            {
                ingredientImages[i].sprite = ingredients[i];
            }
        }
    }

    // Updates timer bar
    public void SetFillAmount (float fillAmount)
    {
        timerFill.fillAmount = Mathf.Clamp01(fillAmount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
