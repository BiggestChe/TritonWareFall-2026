using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class OnPress : MonoBehaviour
{
    public Sprite unpressed;
    public Sprite pressed;
    public UnityEvent OnClick;

    SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = unpressed;
    }

    void OnMouseDown()
    {
        sr.sprite = pressed;
    }

    void OnMouseUp()
    {
        sr.sprite = unpressed;
    }

    void OnMouseUpAsButton()
    {
        OnClick.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
