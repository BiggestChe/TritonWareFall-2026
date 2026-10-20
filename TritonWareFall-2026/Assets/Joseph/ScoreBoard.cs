using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = "000000";
    }
    
    // Update is called once per frame
    void Update()
    {
        scoreText.text = GameManager.Instance.Score.ToString("D6");
    }
}
