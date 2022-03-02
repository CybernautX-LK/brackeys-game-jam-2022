using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Localization;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New AudioClip", menuName = "Nietzsche/Audio/AudioObject")]
public class AudioObject : ScriptableObject
{
    // This script is excluded from odin inspector serialization due to Localization classes

    [Header("Settings")]

    //[BoxGroup("Settings")]
    //[ShowIf("trackType", AudioManager2.TrackType.VoiceOver)]
    //public AudioplayerStyle audioPlayerStyle = null;

    [BoxGroup("Settings")]
    public AudioType type;

    [BoxGroup("Settings")]
    public AudioManager2.TrackType trackType;

    [BoxGroup("Settings")]
    [Range(0.0f, 1.0f)] 
    public float volume = 1.0f;

    [BoxGroup("Settings")]
    public bool randomizePitch = false;

    [Header("Content")]

    [BoxGroup("Content")]
    public bool enableLocalization = false;

    [BoxGroup("Content")]
    [HideIf("enableLocalization")]
    public AudioClip clip;

    [BoxGroup("Content")]
    [HideIf("enableLocalization")]
    public AudioClip clipEN;

    //[BoxGroup("Content")]
    //[ShowIf("enableLocalization")]
    //public LocalizedAudioClip clipKey = new LocalizedAudioClip();

    [Header("Debug Info")]

    [BoxGroup("Debug Info")]
    [ReadOnly]
    [ShowIf("trackType", AudioManager2.TrackType.VoiceOver)]
    public bool hasBeenPlayedCompletely = false;

    [BoxGroup("Debug Info")]
    [ShowIf("trackType", AudioManager2.TrackType.VoiceOver)]
    [ReadOnly]
    [Range(0.0f, 1.0f)]
    public float playbackPosition = 0.0f;

    private void OnEnable()
    {
        playbackPosition = 0.0f;
        hasBeenPlayedCompletely = false;
    }
}
