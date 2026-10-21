using UnityEngine;

[DefaultExecutionOrder(-1)]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    const string VolumeKey = "MasterVolume";
    public float Volume { get; private set; } = 1f;

    public int bruh;

    AudioSource musicSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public float volume = 1f;
        public bool loop = false;
        [HideInInspector] public AudioSource source;
    }

    public Sound[] sounds;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persists across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

        Play("MainMusic");
        
    }

    public void SetVolume(float value)
    {
        Volume = Mathf.Clamp01(value);
        AudioListener.volume = Volume;
        PlayerPrefs.SetFloat(VolumeKey, Volume);
    }

    public void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s != null) s.source.Stop();
    }
}
