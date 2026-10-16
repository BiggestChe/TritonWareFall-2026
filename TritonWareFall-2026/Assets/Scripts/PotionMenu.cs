using System.Collections.Generic;
using UnityEngine;

public class PotionMenu : MonoBehaviour
{
    [SerializeField] Transform[] slots;      // 3 empty slot positions
    [SerializeField] PotionItem[] potions;   // the 3 potions
    [SerializeField] Transform selector;     // highlight that follows the cursor

    PotionItem[] bySlot;                     // bySlot[i] = potion currently in slot i
    int index;

    public bool IsOpen { get; private set; }
    public System.Action<PotionItem> OnPotionChosen;

    public void Open()
    {
        gameObject.SetActive(true);
        IsOpen = true;
        Shuffle();
        index = 1;
        MoveSelector();
    }

    void Update()
    {
        if (!IsOpen) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)  || Input.GetKeyDown(KeyCode.A)) Move(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) Move(1);

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            OnPotionChosen?.Invoke(bySlot[index]);
            Close();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) Close();
    }

    void Move(int dir)
    {
        index = (index + dir + slots.Length) % slots.Length;  // wraps around
        MoveSelector();
    }

    void MoveSelector() => selector.position = slots[index].position;

    void Shuffle()
    {
        var list = new List<PotionItem>(potions);
        for (int i = list.Count - 1; i > 0; i--)             // Fisher-Yates
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        bySlot = list.ToArray();
        for (int i = 0; i < bySlot.Length; i++) 
        {
            bySlot[i].transform.position = slots[i].position;
        }   
    }

    void Close()
    {
        IsOpen = false;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
