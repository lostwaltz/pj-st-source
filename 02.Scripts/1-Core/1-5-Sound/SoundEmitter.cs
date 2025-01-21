using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    public SoundData Data { get; private set; }
    public LinkedListNode<SoundEmitter> Node { get; set; }

    private AudioSource audioSource;
    private Coroutine playingCoroutine;

    private float baseVolume;

    private void Awake()
    {
        audioSource = gameObject.GetOrAdd<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize(SoundData data)
    {
        Data = data;

        baseVolume = data.Volume;

        audioSource.clip = data.Clip;
        audioSource.outputAudioMixerGroup = data.MixerGroup;
        audioSource.loop = data.Loop;
        audioSource.playOnAwake = data.PlayOnAwake;

        audioSource.mute = data.Mute;
        audioSource.bypassEffects = data.BypassEffects;
        audioSource.bypassListenerEffects = data.BypassListenerEffects;
        audioSource.bypassReverbZones = data.BypassReverbZones;

        audioSource.priority = data.Priority;
        audioSource.volume = data.Volume;
        audioSource.pitch = data.Pitch;
        audioSource.panStereo = data.PanStereo;
        audioSource.spatialBlend = data.SpatialBlend;
        audioSource.reverbZoneMix = data.ReverbZoneMix;
        audioSource.dopplerLevel = data.DopplerLevel;
        audioSource.spread = data.Spread;

        audioSource.minDistance = data.MinDistance;
        audioSource.maxDistance = data.MaxDistance;

        audioSource.ignoreListenerVolume = data.IgnoreListenerVolume;
        audioSource.ignoreListenerPause = data.IgnoreListenerPause;

        audioSource.rolloffMode = data.RolloffMode;

        UpdateVolume(Core.SoundManager.MasterVolume);
    }

    public void UpdateVolume(float masterVolume)
    {
        audioSource.volume = baseVolume * masterVolume;
    }

    public void Play()
    {
        if (playingCoroutine != null)
        {
            StopCoroutine(playingCoroutine);
        }

        audioSource.Play();
        playingCoroutine = StartCoroutine(WaitForSoundToEnd());
    }

    IEnumerator WaitForSoundToEnd()
    {
        yield return new WaitWhile(() => audioSource.isPlaying || audioSource.loop == true);
        Stop();
    }

    public void Stop()
    {
        if (playingCoroutine != null)
        {
            StopCoroutine(playingCoroutine);
            playingCoroutine = null;
        }

        audioSource.Stop();
        Core.SoundManager.ReturnToPool(this);
    }

    public void WithRandomPitch(float min = -0.05f, float max = 0.05f)
    {
        audioSource.pitch += Random.Range(min, max);
    }
}