using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectManager : MonoBehaviour
{

    public static SoundEffectManager Instance;
    public float highVolume = 0.7f;
    public float lowVolume = 0.2f;
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
        Instance.aud.volume = Instance.highVolume;
        Instance.aud.clip = clip;
        Instance.aud.Play();
    }
    public static void PlaySoundEffectLowVolume(AudioClip clip)
    {
        Instance.aud.volume = Instance.lowVolume;
        Instance.aud.clip = clip;
        Instance.aud.Play();
    }
    public static void PlaySoundEffectWithRandomPitch(AudioClip clip)
    {

        Instance.aud.volume = Instance.highVolume;
        Instance.aud.pitch = Random.Range(0.8f, 1.2f);
        Instance.aud.clip = clip;
        Instance.aud.Play();
    }
    public static void PlaySoundEffectWithRandomPitchAndVolume(AudioClip clip, float volume)
    {
        Instance.aud.volume = Instance.highVolume;
        Instance.aud.pitch = Random.Range(0.8f, 1.2f);
        Instance.aud.clip = clip;
        Instance.aud.Play();
    }
    public static void PlaySoundEffectWithRandomPitchLowVolume(AudioClip clip)
    {
        Instance.aud.volume = Instance.lowVolume;
        Instance.aud.pitch = Random.Range(0.8f, 1.2f);
        Instance.aud.clip = clip;
        Instance.aud.Play();
    }
}
