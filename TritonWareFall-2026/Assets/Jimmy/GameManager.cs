using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum IngredientType { RedPotion, GreenPotion, BluePotion, Mandrake, LizardTail }
public enum GameState { Menu, Playing, Paused, GameOver }
[DefaultExecutionOrder(-1)] //Lets GameManager run before other scripts
public class GameManager : MonoBehaviour
{ 

    public GameOverStats GameOverStats;

    //Public singleton accessible anywhere
    public static GameManager Instance { get; private set; }

    //Game Flow
    [SerializeField] private float shiftLength = 240f;        // seconds (GDD: 3-5 min)
    [SerializeField] private int maxFailedOrders = 3;
    [SerializeField] private bool startOnPlay = true;         // turn off once the main menu calls StartShift
    [SerializeField] public int timer; 
    // Added by Joseph Tse, 2026-10-19
    [SerializeField] private GameObject PauseMenu;

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
    
    // Added by Joseph Tse, 2026-10-19
    public float Drunkeness { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       Instance = this;
       StartShift(); 
    }

    
    public void StartShift()
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
        // Added by Joseph Tse, 2026-10-19
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (State == GameState.Playing) PauseGame();
            else if (State == GameState.Paused) ResumeGame();
        }

        if (State != GameState.Playing) return;

        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            EndGame(SurvivedShift);
        }
    }

    // Called by RecipeGenerator when the player successfully serves an order
    public void ServeOrder(int basePoints, float timeLeftFraction)
{
    if (State != GameState.Playing) return;

    Streak++;
    if (Streak > LongestStreak) LongestStreak = Streak;
    OrdersServed++;

    float drunk = DrunkManager.Drunk_Manager != null ? DrunkManager.Drunk_Manager.Drunk_Amount : 0f;
    int points = Mathf.RoundToInt(basePoints * (1f + timeLeftFraction * 0.5f));

    //if the player is drunk, they get a bonus to their tips
    int tip = Mathf.RoundToInt(points * drunk);   // drunk genius bonus

    Score += points;
    Tips += tip;
    
    DrunkManager.Drunk_Manager?.AddIntoxication(DrunkManager.Drunk_Manager.swigAmount); // Add intoxication when an order is served
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
        // Joseph Tse, 2026-10-18
        if (!survived)
        {
            SceneManager.LoadScene("GameOver");
        }
        return;
    }
    
    // Added by Joseph Tse, 2026-10-19
    public void PauseGame()
    {
        if (State != GameState.Playing) return;
        State = GameState.Paused;
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
    }

    // Added by Joseph Tse, 2026-10-19
    public void ResumeGame()
    {
        if (State != GameState.Paused) return;
        State = GameState.Playing;
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
    }

    // Added by Joseph Tse, 2026-10-19
    float drunkMultiplier(float x)
        {
            return 2 * x + 1;
        }


    // Added by Joseph Tse, 2026-10-19
    public void CompleteOrder(int score, int tips)
    {
        if (State != GameState.Playing) return;
         
        OrdersServed++;
        Streak++;
        if (Streak > LongestStreak) LongestStreak = Streak;

        Tips += tips;

        Drunkeness = DrunkManager.Drunk_Manager.Drunk_Amount;

        score = Mathf.RoundToInt(score * drunkMultiplier(Drunkeness) + tips);

        Score += score;
    }
}
