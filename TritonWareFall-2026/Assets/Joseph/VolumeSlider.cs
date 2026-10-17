using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    public Image fillImage;
    Slider slider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider = GetComponent<Slider>();

        slider.value = AudioManager.Instance.Volume;
        UpdateVisual(slider.value);
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        AudioManager.Instance.SetVolume(value);
        UpdateVisual(value);
    }

    void UpdateVisual(float value)
    {
        fillImage.fillAmount = value;
    }
}
