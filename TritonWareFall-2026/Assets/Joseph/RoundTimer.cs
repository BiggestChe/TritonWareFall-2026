using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Image fillImage;
    private float timeLeft;
    private float shiftLength;

    bool resetPending = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fillImage.fillAmount = 1f;
        shiftLength = GameManager.Instance.TimeRemaining;
        timeLeft = GameManager.Instance.TimeRemaining;
        timerText.text = timeLeft.ToString("F0");
    }
 
    IEnumerator WaitAndReset()
        {
            resetPending = true;
            yield return new WaitForSeconds(1f);
            resetPending = false;
        }

    // Update is called once per frame
    void Update()
    {
        timeLeft = GameManager.Instance.TimeRemaining;
        fillImage.fillAmount = timeLeft / shiftLength;
        timerText.text = timeLeft.ToString("F0");
        if (!resetPending)
        {
            Debug.Log($"Time Remaining: {timeLeft}");
            StartCoroutine(WaitAndReset());
        }
    }
}
