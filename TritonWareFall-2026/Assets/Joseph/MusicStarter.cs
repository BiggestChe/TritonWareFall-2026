using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    public AudioClip music;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayMusic(music);
    }
}
