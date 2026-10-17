using System.Collections.Generic;
using UnityEngine;

public enum IngredientType { RedPotion, GreenPotion, BluePotion, Mandrake, LizardTail }
public enum GameState { Menu, Playing, Paused, GameOver }
public class GameManager : MonoBehaviour
{ 

    //Public singleton accessible anywhere
    public static GameManager Instance { get; private set; }

    //Game Flow
    [SerializeField] private float shiftLength = 240f;        // seconds (GDD: 3-5 min)
    [SerializeField] private int maxFailedOrders = 3;
    [SerializeField] private bool startOnPlay = true;         // turn off once the main menu calls StartShift
    [SerializeField] public int timer; 

    // => denotes a lambda function
    public int FailuresRemaining => maxFailedOrders - OrdersFailed;

    //Game Stats with their own setters/getters, automatically done by Unity ig
    public int Score { get; private set; }
    public int Tips { get; private set; }
    public int OrdersServed { get; private set; }
    public int OrdersFailed { get; private set; }
    public int Streak { get; private set; }
    public int LongestStreak { get; private set; }

    public GameState State { get; private set; } = GameState.Menu;
    public float TimeRemaining { get; private set; }    

    //if a player somehow lasts the whole 5 minutes, display a cool message saying yay you did or something
    public bool SurvivedShift { get; private set; }
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       StartShift(); 
    }

    
    void StartShift()
    {
        Time.timeScale = 1f;
        TimeRemaining = shiftLength;
        Score = Tips = OrdersServed = OrdersFailed = Streak = LongestStreak = 0;
        SurvivedShift = false;
        State = GameState.Playing;
 
        if (DrunkManager.Drunk_Manager != null) DrunkManager.Drunk_Manager.DrunkReset();
    }

    // Update is called once per frame
     void Update()
    {
        if (State != GameState.Playing) return;

        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            EndGame(SurvivedShift);
        }
    }

    public void FailOrder()
    {
        if (State != GameState.Playing) return;
 
        OrdersFailed++;
        Streak = 0;
 
        if (OrdersFailed >= maxFailedOrders) EndGame(false);
    }
    public void EndGame(bool shift_complete)
    {
        State = GameState.GameOver;
        Display_Results(shift_complete);
    }

    void Display_Results(bool survived)
    {   
        //something like resultsscreen.setactive(true)
        return;
    }
}
