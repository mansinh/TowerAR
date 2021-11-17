using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    AudioListener audioListener;
    public AudioSource[] SoundSources;
    public AudioSource[] AllSources;
    public AudioClip[] SoundClips = new AudioClip[12];
    int sourceIndex = 0;
   

    public enum SoundType
    {
        Arrow,
        Build,
        Damage,
        DealCard,
        Lumberyard,
        Magic,
        MonsterDamage,
        PickTree,
        PlaceBuilding,
        PlantTree,
        Restored,
        Select      
    }

    private void Awake()
    {
        audioListener = FindObjectOfType<AudioListener>();

        print(Options.Instance + "OPTIONS");
        if (Options.Instance)
        {
            AudioListener.volume = Options.Instance.volume;
        }

        if (Instance == null)
        {
            Instance = this;
            LoadSounds();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }


    }

   

    public void Play(SoundType type)
    {
        AudioSource soundSource = SoundSources[sourceIndex];
        if (!soundSource.isPlaying)
        {
            print("play sound " + type);
            soundSource.clip = SoundClips[(int)type];
            soundSource.Play();
            sourceIndex++;
            if (sourceIndex >= SoundSources.Length)
            {
                sourceIndex = 0;
            }
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    void LoadSounds()
    {
        print("LOAD SOUNDS");
        SoundClips[(int)SoundType.Arrow] = Resources.Load("Sounds/arrow") as AudioClip;
        SoundClips[(int)SoundType.Build] = Resources.Load("Sounds/build") as AudioClip;
        SoundClips[(int)SoundType.Damage] = Resources.Load("Sounds/damage") as AudioClip;
        SoundClips[(int)SoundType.DealCard] = Resources.Load("Sounds/dealCard") as AudioClip;
        SoundClips[(int)SoundType.Lumberyard] = Resources.Load("Sounds/lumberyard") as AudioClip;
        SoundClips[(int)SoundType.Magic] = Resources.Load("Sounds/magic") as AudioClip;
        SoundClips[(int)SoundType.MonsterDamage] = Resources.Load("Sounds/monsterDamage") as AudioClip;
        SoundClips[(int)SoundType.PickTree] = Resources.Load("Sounds/pickTree") as AudioClip;
        SoundClips[(int)SoundType.PlaceBuilding] = Resources.Load("Sounds/placeBuilding") as AudioClip;
        SoundClips[(int)SoundType.PlantTree] = Resources.Load("Sounds/plantTree") as AudioClip;
        SoundClips[(int)SoundType.Restored] = Resources.Load("Sounds/restored") as AudioClip;
        SoundClips[(int)SoundType.Select] = Resources.Load("Sounds/select") as AudioClip;
    }
}
