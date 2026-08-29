using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectManager : MonoBehaviour
{

    public static SoundEffectManager Instance;

    AudioSource aud;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;

        }
        else
        {
            Destroy(this.gameObject);
        }
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.loop = false;
    }

    public static void PlaySoundEffect(AudioClip clip)
    {
        Instance.aud.clip = clip;
        Instance.aud.Play();
    }
    public static void PlaySoundEffectWithRandomPitch(AudioClip clip)
    {
        Instance.aud.pitch = Random.Range(0.8f, 1.2f);

        Instance.aud.clip = clip;
        Instance.aud.Play();
    }
}
