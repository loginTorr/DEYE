using System.Collections.Generic;
using UnityEngine;

public enum SoundType {
    SHOTGUN_PUNCH_1,
    SHOTGUN_PUNCH_2,
    SHOTGUN_PUNCH_3,
    SHOTGUN_PUNCH_4,
    SHOTGUN_LOAD_1,
    SHOTGUN_LOAD_2,
    SHOTGUN_LOAD_3,
    SHOTGUN_LOAD_4,
}

public class SoundCollection {
    private AudioClip[] clips;
    private int lastClipIndex;

    public SoundCollection(params string[] clipNames) {
        this.clips = new AudioClip[clipNames.Length];
        for (int i = 0; i < clips.Length; i++) {
            clips[i] = Resources.Load<AudioClip>(clipNames[i]);
            if (clips[i] == null) {
                Debug.Log($"can't find audio clip {clipNames[i]}");
            }
        }
        lastClipIndex = -1;
    }

    public AudioClip GetRandClip() {
        if (clips.Length == 0) {
            Debug.Log("No clips to give");
            return null;
        }
        else if (clips.Length == 1) {
            return clips[0];
        }
        else {
            int index = lastClipIndex;
            while (index == lastClipIndex) {
                index = Random.Range(0, clips.Length);
            }
            lastClipIndex = index;
            return clips[index];
        }
    }

}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {
    public float mainVolume = 1.0f;
    private Dictionary<SoundType, SoundCollection> sounds;
    private AudioSource audioSrc;

    public static SoundManager Instance { get; private set; }

    // unity life cycle
    private void Awake() {
        Instance = this;
        audioSrc = GetComponent<AudioSource>();
        sounds = new() {
            { SoundType.SHOTGUN_PUNCH_1, new("ShotGun Shot Punchy_1") },
            { SoundType.SHOTGUN_PUNCH_2, new("ShotGun Shot Punchy_2") },
            { SoundType.SHOTGUN_PUNCH_3, new("ShotGun Shot Punchy_3") },
            { SoundType.SHOTGUN_PUNCH_4, new("ShotGun Shot Punchy_4") },
            { SoundType.SHOTGUN_LOAD_1, new("ShotGun Load_1") },
            { SoundType.SHOTGUN_LOAD_2, new("ShotGun Load_2") },
            { SoundType.SHOTGUN_LOAD_3, new("ShotGun Load_3") },
            { SoundType.SHOTGUN_LOAD_4, new("ShotGun Load_4") },
        };
    }

    public static void Play(SoundType type, AudioSource audioSrc = null, float pitch = -1) {
        print("playing sound");
        if (Instance.sounds.ContainsKey(type)) {
            audioSrc ??= Instance.audioSrc;
            audioSrc.volume = Random.Range(0.70f, 1.0f) * Instance.mainVolume;
            audioSrc.pitch = pitch >= 0 ? pitch : Random.Range(0.75f, 1.25f);
            audioSrc.clip = Instance.sounds[type].GetRandClip();
            audioSrc.Play();
        }
    }
}
