using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;
using UnityEngine.Events;

public class AudioManager2 : MonoBehaviour
{
    public static AudioManager2 Instance;

    public enum TrackType { None, MainMusic, SFX, VoiceOver };

    [System.Serializable]
    public class AudioTrack
    {
        public TrackType type;
        public AudioSource source;
        public List<VolumeSlider> volumeSlider = new List<VolumeSlider>();
        public List<AudioObject> audio = new List<AudioObject>();

        [Range(0, 1)] public float originalVolume = 1.0f;

        public Tween fadeInTween = null;
        public Tween fadeOutTween = null;
    }

    public bool debug;
    [SerializeField]
    private bool dontDestroyOnLoad = false;
    //[SerializeField] private AudioEventChannelSO audioEventChannel = null;
    //[SerializeField] private SceneLoadingEventChannelSO sceneEventChannel = null;
    public Ease fadeInEase;
    public Ease fadeOutEase;
    public AudioTrack[] tracks;

    public UnityAction<TrackType, float> OnVolumeChangedEvent;
    public UnityAction<VolumeSlider> OnVolumeSliderRegisteredEvent;
    public UnityAction<VolumeSlider> OnVolumeSliderUnregisteredEvent;

    private Hashtable audioObjectTable = new Hashtable(); // relationship between audio objects (key) and audio tracks (value)
    private Tween fadeInTween;
    private Tween fadeOutTween;

    private void Awake()
    {
        Instance = this;

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(this);

        //TestWindowController.PlayAudio.AddListener(Play);
        //
        ////if(NativeBridge.Instance != null)
        ////   NativeBridge.OnAudioRestrictionUpdate.AddListener(SetAudioPlayback);
        //
        //if (audioEventChannel != null)
        //{
        //    audioEventChannel.OnAudioObjectPlayEvent += Play;
        //    audioEventChannel.OnAudioObjectPlayWithSourceEvent += Play;
        //    audioEventChannel.OnTrackTypePlayEvent += Play;
        //    audioEventChannel.OnPlayOneShotEvent += PlayOneShot;
        //    audioEventChannel.OnAudioObjectPauseEvent += Pause;
        //    audioEventChannel.OnTrackTypePauseEvent += Pause;
        //    audioEventChannel.OnAudioObjectStopEvent += Stop;
        //    audioEventChannel.OnTrackTypeStopEvent += Stop;
        //    audioEventChannel.SetVolumeEvent += SetVolume;
        //    audioEventChannel.SetMainMusicVolumeEvent += SetMainMusicVolume;
        //    audioEventChannel.SetSFXVolumeEvent += SetSFXVolume;
        //    audioEventChannel.SetVoiceOverVolumeEvent += SetVoiceOverVolume;
        //    audioEventChannel.RegisterVolumeSliderEvent += RegisterVolumeSlider;
        //    audioEventChannel.UnregisterVolumeSliderEvent += UnregisterVolumeSlider;
        //}

       //if (sceneEventChannel != null)
       //{
       //    sceneEventChannel.OnSceneUnloadEvent += OnSceneUnload;
       //}

        GenerateAudioObjectTable();

        //NativeBridge.OnAudioRestrictionUpdate += SetAudioPlayback;
    }

   //private void OnSceneUnload(string sceneName)
   //{
   //    Stop(TrackType.MainMusic, 2.0f);
   //}

    private void OnDestroy()
    {
        //TestWindowController.PlayAudio.RemoveListener(Play);
        //
        //if (audioEventChannel != null)
        //{
        //    audioEventChannel.OnAudioObjectPlayEvent -= Play;
        //    audioEventChannel.OnAudioObjectPlayWithSourceEvent -= Play;
        //    audioEventChannel.OnTrackTypePlayEvent -= Play;
        //    audioEventChannel.OnPlayOneShotEvent -= PlayOneShot;
        //    audioEventChannel.OnAudioObjectPauseEvent -= Pause;
        //    audioEventChannel.OnTrackTypePauseEvent -= Pause;
        //    audioEventChannel.OnAudioObjectStopEvent -= Stop;
        //    audioEventChannel.OnTrackTypeStopEvent -= Stop;
        //    audioEventChannel.SetVolumeEvent -= SetVolume;
        //    audioEventChannel.SetMainMusicVolumeEvent -= SetMainMusicVolume;
        //    audioEventChannel.SetSFXVolumeEvent -= SetSFXVolume;
        //    audioEventChannel.SetVoiceOverVolumeEvent -= SetVoiceOverVolume;
        //    audioEventChannel.RegisterVolumeSliderEvent -= RegisterVolumeSlider;
        //    audioEventChannel.UnregisterVolumeSliderEvent -= UnregisterVolumeSlider;
        //}

        //if (sceneEventChannel != null)
        //{
        //    sceneEventChannel.OnSceneUnloadEvent -= OnSceneUnload;
        //}

        //NativeBridge.OnAudioRestrictionUpdate -= SetAudioPlayback;
    }

    public void Play(AudioObject audioObject)
    {
        Play(audioObject, 0.0f);
    }

    public void Play(AudioObject audioObject, AudioSource audioSource)
    {
        if (audioObject == null || audioSource == null) return;

        audioSource.PlayOneShot(audioObject.clip, audioObject.volume);
    }

    public void Play(AudioObject audioObject, float fade = 0.0f)
    {
        if (audioObject == null) return;

        AudioTrack track = (AudioTrack)audioObjectTable[audioObject];

        if (track == null) return;

        UnityAction OnSFXPlay = () =>
        {
            if (!track.source.mute)
                track.source.PlayOneShot(audioObject.clip, audioObject.volume * track.originalVolume);
        };

        UnityAction OnNoSFXPlay = () =>
        {
            track.source.clip = audioObject.clip;
            track.source.volume = audioObject.volume * track.originalVolume;
            track.source.Play();
        };

        UnityAction ActionToCall = (audioObject.trackType == TrackType.SFX) ? OnSFXPlay : OnNoSFXPlay;

        if (fade > 0.0f)
        {
            if (DOTween.IsTweening(track.source))
                DOTween.Kill(track.source);

            track.fadeInTween = track.source.DOFade(audioObject.volume * track.originalVolume, fade).From(0.0f).SetEase(fadeInEase).OnPlay(() => ActionToCall.Invoke());
        }
        else
        {
            ActionToCall.Invoke();
        }

        Log("[AudioManager]: Playing " + audioObject);
    }

    public void Play(TrackType type, float fade = 0.0f)
    {
        if (type == TrackType.None) return;

        AudioTrack track = GetAudioTrack(type);

        if (track != null)
        {           
            if (fade > 0.0f)
            {
                if (DOTween.IsTweening(track.source))
                    DOTween.Kill(track.source);

                track.source.volume = 0.0f;

                track.fadeInTween = track.source.DOFade(track.originalVolume, fade).From(0.0f).SetEase(fadeInEase).OnPlay(() => track.source.Play());
            }
            else
            {
                track.source.Play();
            }           
        }

        Log("[AudioManager]: Playing " + type);
    }

    public void PlayOneShot(AudioObject audioObject)
    {
        if (audioObject == null) return;

        AudioTrack track = (AudioTrack)audioObjectTable[audioObject];

        if (track == null) return;

        track.source.pitch = (audioObject.randomizePitch) ? Random.Range(0.75f, 1.25f) : 1.0f;

        if (!track.source.mute)
            track.source.PlayOneShot(audioObject.clip, audioObject.volume * track.originalVolume);

        Log("[AudioManager]: Playing " + audioObject);
    }

    public void Pause(AudioObject audioObject, float fade = 0.0f)
    {
        if (audioObject == null) return;

        AudioTrack track = (AudioTrack)audioObjectTable[audioObject];

        if (fade > 0.0f)
        {
            if (DOTween.IsTweening(track.source))
                DOTween.Kill(track.source);

            track.fadeOutTween = track.source.DOFade(0.0f, fade).From(track.source.volume).OnComplete(track.source.Pause).SetEase(fadeOutEase);
            return;
        }

        track.source.Pause();

        Log("[AudioManager]: Pausing " + audioObject);
    }

    public void Pause(TrackType type, float fade = 0.0f)
    {
        if (type == TrackType.None) return;

        AudioTrack track = GetAudioTrack(type);

        if (track != null)
        {
            if (fade > 0.0f)
            {
                if (DOTween.IsTweening(track.source))
                    DOTween.Kill(track.source);

                track.fadeOutTween = track.source.DOFade(0.0f, fade).From(track.source.volume).OnComplete(track.source.Pause).SetEase(fadeOutEase);
                return;
            }

            track.source.Pause();

            Log("[AudioManager]: Pausing " + type);
        }
    }

    public void Stop(AudioObject audioObject, float fade = 0.0f)
    {
        if (audioObject == null) return;

        AudioTrack track = (AudioTrack)audioObjectTable[audioObject];

        if (fade > 0.0f)
        {
            if (DOTween.IsTweening(track.source))
                DOTween.Kill(track.source);

            track.fadeOutTween = track.source.DOFade(0.0f, fade).From(track.source.volume).OnComplete(track.source.Stop).SetEase(fadeOutEase);
            return;
        }

        track.source.Stop();

        Log("[AudioManager]: Stopping " + audioObject);
    }

    public void Stop(TrackType type, float fade = 0.0f)
    {
        if (type == TrackType.None) return;

        AudioTrack track = GetAudioTrack(type);

        if (track != null)
        {
            if (fade > 0.0f)
            {
                if (DOTween.IsTweening(track.source))
                    DOTween.Kill(track.source);

                track.fadeOutTween = track.source.DOFade(0.0f, fade).From(track.source.volume).OnComplete(track.source.Pause).SetEase(fadeOutEase);
                return;
            }

            track.source.Stop();

            Log("[AudioManager]: Stopping " + type);
        }
    }

    //public void SetAudioPlayback(AudioRestriction restriction)
    //{
    //    switch (restriction)
    //    {
    //        case AudioRestriction.None:
    //
    //            foreach (AudioTrack track in tracks)
    //            {
    //                track.source.mute = false;
    //            }
    //
    //            AudioTrack mainMusic = GetAudioTrack(TrackType.MainMusic);
    //
    //            if (!mainMusic.source.isPlaying)
    //                Play(TrackType.MainMusic);
    //
    //            break;
    //        case AudioRestriction.Restricted:
    //
    //            foreach (AudioTrack track in tracks)
    //            {
    //                track.source.mute = true;
    //                track.source.Pause();
    //            }
    //
    //            break;
    //    }
    //}

    public void SetMainMusicVolume(float volume)
    {
        SetVolume(TrackType.MainMusic, volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume(TrackType.SFX, volume);
    }

    public void SetVoiceOverVolume(float volume)
    {
        SetVolume(TrackType.VoiceOver, volume);
    }

    public void SetVolume(TrackType type, float volume)
    {
        AudioTrack track = GetAudioTrack(type);

        if (track != null)
        {
            track.originalVolume = volume;
            track.source.volume = volume;

            OnVolumeChangedEvent?.Invoke(type, volume);
        }
    }

    private void GenerateAudioObjectTable()
    {
        foreach (AudioTrack _track in tracks)
        {
            foreach (AudioObject _object in _track.audio)
            {
                if (audioObjectTable.ContainsKey(_object))
                {
                    LogWarning("You are trying to register audio [" + _object + "] that has already been registered.");
                }
                else
                {
                    audioObjectTable.Add(_object, _track);
                    Log("Registering audio [" + _object + "].");
                }
            }
        }
    }

    public AudioTrack GetAudioTrack(TrackType type)
    {
        foreach (AudioTrack track in tracks)
        {
            if (track.type == type)
            {
                return track;
            }
        }

        return null;
    }

    private void RegisterVolumeSlider(VolumeSlider slider)
    {
        foreach (AudioTrack track in tracks)
        {
            if (slider.trackType == track.type)
            {
                if (!track.volumeSlider.Contains(slider))
                {
                    track.volumeSlider.Add(slider);
                }
            }
        }

        OnVolumeSliderRegisteredEvent?.Invoke(slider);
    }

    private void UnregisterVolumeSlider(VolumeSlider slider)
    {
        foreach (AudioTrack track in tracks)
        {
            if (slider.trackType == track.type)
            {
                if (track.volumeSlider.Contains(slider))
                {
                    track.volumeSlider.Remove(slider);
                }
            }
        }

        OnVolumeSliderUnregisteredEvent?.Invoke(slider);
    }

    #region Debug Functions
    private void Log(string _msg)
    {
        if (!debug) return;
        Debug.Log("[AudioManager]: " + _msg);
    }

    private void LogWarning(string _msg)
    {
        if (!debug) return;
        Debug.LogWarning("[AudioManager]: " + _msg);
    }
    #endregion
}
