using UnityEngine;

public class IDieNow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void EndGame()
    {
        GameManager.Instance.EndGame(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
