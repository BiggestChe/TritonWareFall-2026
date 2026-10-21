using UnityEngine;
using TMPro;

public class WinScreen : MonoBehaviour
{

    public static WinScreen Instance { get; private set; }

    public TextMeshProUGUI TipsNumber;
    public TextMeshProUGUI OrdersServedNumber;
    public TextMeshProUGUI OrdersFailedNumber;
    public TextMeshProUGUI FinalScoreNumber;

    public TextMeshProUGUI LongestStreakNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;

        TipsNumber.text = GameManager.Instance.Tips.ToString("F0");
        OrdersServedNumber.text = GameManager.Instance.OrdersServed.ToString("F0");
        OrdersFailedNumber.text = GameManager.Instance.OrdersFailed.ToString("F0");
        FinalScoreNumber.text = GameManager.Instance.Score.ToString("F0");
        LongestStreakNumber.text = GameManager.Instance.LongestStreak.ToString("F0");

        
    }

    // Update is called once per frame
    void Update()
    {
        TipsNumber.text = GameManager.Instance.Tips.ToString("F0");
        OrdersServedNumber.text = GameManager.Instance.OrdersServed.ToString("F0");
        OrdersFailedNumber.text = GameManager.Instance.OrdersFailed.ToString("F0");
        FinalScoreNumber.text = GameManager.Instance.Score.ToString("F0");
        LongestStreakNumber.text = GameManager.Instance.LongestStreak.ToString("F0");
    }
}
