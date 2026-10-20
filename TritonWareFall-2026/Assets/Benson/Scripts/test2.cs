using UnityEngine;

public class GrindingTest : MonoBehaviour
{
    [SerializeField] GrindingMinigame game;

    void Start()
    {
        game.Finished += won => Debug.Log(won ? "Won!" : "Lost");
        game.Begin();
    }
}
