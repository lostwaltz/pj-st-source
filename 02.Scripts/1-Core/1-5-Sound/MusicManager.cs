using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

[RequireComponent(typeof(MusicManager))]
public class MusicManager : SingletonDontDestroy<MusicManager>
{
    private float maxVolume = 0.1f;
    public float MaxVolume
    {
        get => maxVolume;
        set
        {
            maxVolume = value;
            if (current != null) current.volume = maxVolume;
            if (previous != null) previous.volume = maxVolume;
        }
    }
    private const float CrossFadeTime = 1.0f;
    private float fading;
    private AudioSource current;
    private AudioSource previous;
    private readonly Queue<AudioClip> playlist = new();

    [FormerlySerializedAs("initialPlaylist")][SerializeField] private List<AudioClip> InitialPlaylist;
    [FormerlySerializedAs("musicMixerGroup")][SerializeField] private AudioMixerGroup MusicMixerGroup;

    private void Start()
    {
        if (InitialPlaylist != null)
        {
            foreach (var clip in InitialPlaylist)
            {
                AddToPlaylist(clip);
            }
        }
    }

    public void AddToPlaylist(AudioClip clip)
    {
        playlist.Enqueue(clip);
        if (current == null && previous == null)
        {
            PlayNextTrack();
        }
    }

    public void Clear() => playlist.Clear();

    public void PlayNextTrack()
    {
        if (playlist.TryDequeue(out AudioClip nextTrack))
        {
            Play(nextTrack);
        }
    }

    public void Play(AudioClip clip)
    {
        if (current && current.clip == clip) return;

        if (previous)
        {
            Destroy(previous);
            previous = null;
        }

        previous = current;

        current = gameObject.GetOrAdd<AudioSource>();
        current.clip = clip;
        current.outputAudioMixerGroup = MusicMixerGroup;
        current.loop = true; 
        current.volume = 0f;
        current.bypassListenerEffects = true;
        current.Play();

        fading = 0.001f;
    }

    private void Update()
    {
        HandleCrossFade();

        if (current && !current.isPlaying && playlist.Count > 0)
        {
            PlayNextTrack();
        }
    }

    private void HandleCrossFade()
    {
        if (fading <= 0f) return;

        fading += Time.deltaTime;

        var fraction = Mathf.Clamp01(fading / CrossFadeTime);

        // Logarithmic fade
        var logFraction = fraction.ToLogarithmicFraction();

        if (previous) previous.volume = (1.0f - logFraction) * maxVolume;
        if (current) current.volume = logFraction * maxVolume;

        if (!(fraction >= 1)) return;

        fading = 0.0f;

        if (!previous) return;

        Destroy(previous);
        previous = null;
    }

    public void FadeOutStop()
    {
        if (!current) return;

        previous = current;
        current = null;
        fading = 0.1f;
    }
    public void Stop()
    {
        if (current)
        {
            current.Stop();
            current = null;
        }
        if (previous)
        {
            previous.Stop();
            previous = null;
        }
    }
}