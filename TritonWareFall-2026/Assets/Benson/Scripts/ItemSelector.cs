using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    [SerializeField] Transform[] slots;      // 3 empty slot positions
    [SerializeField] Item[] items;           // the items to choose from
    [SerializeField] Transform selector;     // highlight that follows the cursor

    [SerializeField] int columns = 3;        // number of columns in the grid
    [SerializeField] int starter = 1;        // index of the item to start on
    [SerializeField] MovementManager player;
    Item[] bySlot;                           // bySlot[i] = item currently in slot i
    int index;

    //added by Jimmy, 10/20/26
    [SerializeField] PlayerHand playerHand;
    public bool IsOpen { get; private set; }
    public System.Action<Item> OnItemChosen;

    public void Open()
    {
        gameObject.SetActive(true);
        IsOpen = true;
        Shuffle();
        index = starter;
        MoveSelector();
        player.input = Vector3.zero;
        player.rb.linearVelocity = Vector3.zero;
        player.enabled = false;
    }

    void Update()
    {
        if (!IsOpen) return;

        if (Pressed(KeyCode.LeftArrow,  KeyCode.A)) Step(-1, 0);
        if (Pressed(KeyCode.RightArrow, KeyCode.D)) Step(1, 0);
        if (Pressed(KeyCode.UpArrow,    KeyCode.W)) Step(0, -1);
        if (Pressed(KeyCode.DownArrow,  KeyCode.S)) Step(0, 1);

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            //changed by Jimmy, 10/20/26, 
               if (playerHand.IsEmpty)
   {
                SpriteRenderer sr = bySlot[index].GetComponent<SpriteRenderer>();
                playerHand.HoldIngredient(bySlot[index].type, sr.sprite);
                AudioManager.Instance.Play("PickUp");
   }
            Close();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) Close();
    }

    bool Pressed(KeyCode a, KeyCode b) => Input.GetKeyDown(a) || Input.GetKeyDown(b);

    void Step(int dx, int dy)
    {
        int rows = Mathf.CeilToInt(slots.Length / (float)columns);
        int x = index % columns;
        int y = index / columns;

        x = (x + dx + columns) % columns;   // wrap horizontally
        y = (y + dy + rows) % rows;         // wrap vertically

        int next = y * columns + x;
        if (next < slots.Length) index = next;   // ignore empty cells in uneven grids
        MoveSelector();
    }

    void MoveSelector() => selector.position = slots[index].position;

    void Shuffle()
    {
        var list = new List<Item>(items);
        if (DrunkManager.Drunk_Manager.State == DrunkState.Drunk || DrunkManager.Drunk_Manager.State == DrunkState.Wasted)
        {
            for (int i = list.Count - 1; i > 0; i--)             // Fisher-Yates
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
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
        player.enabled = true;
    }
}
