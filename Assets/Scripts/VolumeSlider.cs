using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioEventChannelSO audioEventChannel = null;
    public AudioManager2.TrackType trackType;

    [ReadOnly]
    public Slider slider;
    public float currentVolume { get => slider.value; }

    public UnityAction<VolumeSlider> OnVolumeSetEvent;

    private void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        slider.onValueChanged.AddListener(SetVolume);

        if (audioEventChannel != null)
        {
            audioEventChannel.RegisterVolumeSlider(this);
        }
    }

    private void OnDestroy()
    {
        if (audioEventChannel != null)
        {
            audioEventChannel.UnregisterVolumeSlider(this);
        }
    }

    private void OnEnable()
    {
        // Kann das auch mit dem AudioEventChannel gelöst werden?
        if (AudioManager2.Instance != null)
        {
            AudioManager2.AudioTrack track = AudioManager2.Instance.GetAudioTrack(this.trackType);

            if (track != null)
                slider.SetValueWithoutNotify(track.originalVolume);
        }
    }

    public void SetVolume(float value)
    {
        if (audioEventChannel != null)
        {
            audioEventChannel.SetVolume(trackType, value);
        }
        else
        {
            AudioManager2.Instance.SetVolume(trackType, value);
        }

        OnVolumeSetEvent?.Invoke(this);

    }
}
