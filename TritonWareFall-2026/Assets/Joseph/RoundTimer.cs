using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoundTimer : MonoBehaviour
{
    public Text timerText;
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
        if (!resetPending)
        {
            Debug.Log($"Time Remaining: {timeLeft}");
            StartCoroutine(WaitAndReset());
        }
    }
}
