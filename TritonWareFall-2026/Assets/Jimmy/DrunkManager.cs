using System.Collections; //For IEnumerator
using UnityEngine;
using System.Collections.Generic;
//Drunk State Machine
public enum DrunkState { Sober, Tipsy, Drunk, Wasted }

public class DrunkManager : MonoBehaviour
    {

        //drunk singleton, basically just call DrunkManager.Drunk_Manager anywhere
    public static DrunkManager Drunk_Manager { get; private set; }
    
    //how drunk player is, is incremented in a flat manner
    public float Drunk_Amount { get; private set; }

    public float swigAmount = 0.12f;        // added per completed order
    
    [Header("Drunk Thresholds (0-1)")]
    [SerializeField] private float tipsyThreshold = 0.25f;
    [SerializeField] private float drunkThreshold = 0.5f;
    [SerializeField] private float wastedThreshold = 0.75f;
 

    //starting state = Sober
    public DrunkState State { get; private set; } = DrunkState.Sober;
    public DrunkState HighestState { get; private set; } = DrunkState.Sober;

    // Added by Joseph Tse, 2024-06-05
    public DrunkBar DrunkBar;

    private void Awake()
    {
        Drunk_Manager = this;
    }

    public void DrunkReset()
    {
        Drunk_Amount = 0f;
        State = DrunkState.Sober;
    }
    private DrunkState ReturnState(float value)
    {
        if (value >= wastedThreshold) return DrunkState.Wasted;
        if (value >= drunkThreshold) return DrunkState.Drunk;
        if (value >= tipsyThreshold) return DrunkState.Tipsy;
        return DrunkState.Sober;
    }

    //should called whenever order is completed, flat amount?
    public void AddIntoxication(float amount)
    {
        Drunk_Amount = Mathf.Clamp01(Drunk_Amount + amount);
 
        DrunkState newState = ReturnState(Drunk_Amount);
        if (newState != State)
        {
            State = newState;
            if (newState > HighestState) HighestState = newState;
            // Drunk transition?
        }

        // Added by Joseph Tse, 2026-10-17
        if (DrunkBar != null)
        {
            DrunkBar.UpdateBar(Drunk_Amount, State);
        }

    }

    // Added by Joseph Tse, 2026-10-17, for testing purposes
    bool resetPending = false;

    // Update is called once per frame
    void Update()
    {
        // For testing Drunkbar, Joseph Tse, 2026-10-17
        if (!resetPending)
        {
            AddIntoxication(swigAmount);
            Debug.Log($"Drunk Amount: {Drunk_Amount}, State: {State}");
            StartCoroutine(WaitAndReset());
        }
    }

    // Added by Joseph Tse, 2026-10-17, for testing purposes
    IEnumerator WaitAndReset()
        {
            resetPending = true;
            yield return new WaitForSeconds(1f);
            if (Drunk_Amount == 1f)
            {
                Debug.Log("Reached max drunk");
                DrunkReset();
            }
            resetPending = false;
        }
}
