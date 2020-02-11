using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    #region Audio Sources
    [SerializeField]
    private AudioSource music;
    [SerializeField]
    private AudioSource breakableBrickHit;
    [SerializeField]
    private AudioSource unbreakableBrick_paddleHit;
    [SerializeField]
    private AudioSource clearLevel;
    [SerializeField]
    private AudioSource fail;
    [SerializeField]
    private AudioSource UIhover;
    [SerializeField]
    private AudioSource UIclick;
    // Add more Audio Sources below

    #endregion

    [SerializeField] private AudioMixer audioMixer;
    private static AudioManager instance = null; // Only one instance of AudioManager can exist at the same time
    private void Awake()
    {
        // If there is no instance of AudioManger currently
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBreakableHitAudio()
    {
        breakableBrickHit.Play();
    }
    public void PlayUnbreakableHitAudio()
    {
        unbreakableBrick_paddleHit.Play();
    }
    public void PlayFailingAudio()
    {
        fail.Play();
    }
    public void PlayClearingLevelAudio()
    {
        clearLevel.Play();
    }
    public void PlayMouseHoverUIAudio()
    {
        UIhover.Play();
    }
    public void PlayMouseClickUIAudio()
    {
        UIclick.Play();
    }
    // Add more function to play audio below
    // ...

    // Load Mixer settings from previous game
    void LoadMixerSettings()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
        }
    }
    void Start()
    {
        LoadMixerSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
