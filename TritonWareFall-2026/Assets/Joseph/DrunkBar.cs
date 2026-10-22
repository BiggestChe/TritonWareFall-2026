using UnityEngine;
using UnityEngine.UI;

public class DrunkBar : MonoBehaviour
{
    public Image fillImage;
    public Image soberImage;
    public Image tipsyImage;
    public Image drunkImage;
    public Image wastedImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fillImage.fillAmount = 0f;
        
        soberImage.enabled = true;
        tipsyImage.enabled = false;
        drunkImage.enabled = false;
        wastedImage.enabled = false;
    }

    // Should be called whenever DrunkManager's Drunk_Amount is updated
    public void UpdateBar(float value, DrunkState state)
    {
        fillImage.fillAmount = value;

        soberImage.enabled = state == DrunkState.Sober;
        tipsyImage.enabled = state == DrunkState.Tipsy;
        drunkImage.enabled = state == DrunkState.Drunk;
        wastedImage.enabled = state == DrunkState.Wasted;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBar(DrunkManager.Drunk_Manager.Drunk_Amount, DrunkManager.Drunk_Manager.State);
        
    }
}
