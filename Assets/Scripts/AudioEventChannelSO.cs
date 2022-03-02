using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Audio Event Channel")]
public class AudioEventChannelSO : ScriptableObject
{
    public UnityAction<AudioObject, float> OnAudioObjectPlayEvent;
    public UnityAction<AudioObject, AudioSource> OnAudioObjectPlayWithSourceEvent;
    public UnityAction<AudioManager2.TrackType, float> OnTrackTypePlayEvent;
    public UnityAction<AudioObject> OnPlayOneShotEvent;
    
    public UnityAction<AudioObject, float> OnAudioObjectPauseEvent;
    public UnityAction<AudioManager2.TrackType, float> OnTrackTypePauseEvent;
    
    public UnityAction<AudioObject, float> OnAudioObjectStopEvent;
    public UnityAction<AudioManager2.TrackType, float> OnTrackTypeStopEvent;

    public UnityAction<AudioManager2.TrackType, float> SetVolumeEvent;
    public UnityAction<float> SetMainMusicVolumeEvent;
    public UnityAction<float> SetVoiceOverVolumeEvent;
    public UnityAction<float> SetSFXVolumeEvent;

    public UnityAction<VolumeSlider> RegisterVolumeSliderEvent;
    public UnityAction<VolumeSlider> UnregisterVolumeSliderEvent;


    public void Play(AudioObject audioObject)
    {
        OnAudioObjectPlayEvent?.Invoke(audioObject, 0.0f);
    }

    public void Play(AudioObject audioObject, AudioSource audioSource)
    {
        OnAudioObjectPlayWithSourceEvent?.Invoke(audioObject, audioSource);
    }

    public void Play(AudioObject audioObject, float fade)
    {
        OnAudioObjectPlayEvent?.Invoke(audioObject, fade);
    }

    public void Play(AudioManager2.TrackType type)
    {
        OnTrackTypePlayEvent?.Invoke(type, 0.0f);
    }

    public void Play(AudioManager2.TrackType type, float fade)
    {
        OnTrackTypePlayEvent?.Invoke(type, fade);
    }

    public void PlayOneShot(AudioObject audioObject)
    {
        OnPlayOneShotEvent?.Invoke(audioObject);
    }

    public void Pause(AudioObject audioObject)
    {
        OnAudioObjectPauseEvent?.Invoke(audioObject, 0.0f);
    }

    public void Pause(AudioObject audioObject, float fade)
    {
        OnAudioObjectPauseEvent?.Invoke(audioObject, fade);
    }

    public void Pause(AudioManager2.TrackType type)
    {
        OnTrackTypePauseEvent?.Invoke(type, 0.0f);
    }

    public void Pause(AudioManager2.TrackType type, float fade)
    {
        OnTrackTypePauseEvent?.Invoke(type, fade);
    }

    public void Stop(AudioObject audioObject)
    {
        OnAudioObjectStopEvent?.Invoke(audioObject, 0.0f);
    }

    public void Stop(AudioObject audioObject, float fade)
    {
        OnAudioObjectStopEvent?.Invoke(audioObject, fade);
    }

    // Quick Fix - delete later
    public void Stop(int type)
    {
        OnTrackTypeStopEvent?.Invoke((AudioManager2.TrackType)type, 2.0f);
    }

    public void Stop(AudioManager2.TrackType type)
    {
        OnTrackTypeStopEvent?.Invoke(type, 0.0f);
    }

    public void Stop(AudioManager2.TrackType type, float fade)
    {
        OnTrackTypeStopEvent?.Invoke(type, fade);
    }

    public void SetVolume(AudioManager2.TrackType type, float volume)
    {
        SetVolumeEvent?.Invoke(type, volume);
    }

    public void SetMainMusicVolume(float volume)
    {
        SetMainMusicVolumeEvent?.Invoke(volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetSFXVolumeEvent?.Invoke(volume);
    }

    public void SetVoiceOverVolume(float volume)
    {
        SetVoiceOverVolumeEvent?.Invoke(volume);
    }

    public void RegisterVolumeSlider(VolumeSlider slider)
    {
        RegisterVolumeSliderEvent?.Invoke(slider);
    }

    public void UnregisterVolumeSlider(VolumeSlider slider)
    {
        UnregisterVolumeSliderEvent?.Invoke(slider);
    }
}
