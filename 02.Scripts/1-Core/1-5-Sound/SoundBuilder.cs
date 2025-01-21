using UnityEngine;

public class SoundBuilder
{
    private readonly SoundManager soundManager;
    private Vector3 position = Vector3.zero;
    private bool randomPitch;

    public SoundBuilder(SoundManager soundManager)
    {
        this.soundManager = soundManager;
    }

    public SoundBuilder WithPosition(Vector3 playPosition)
    {
        this.position = playPosition;
        return this;
    }

    public SoundBuilder WithRandomPitch()
    {
        this.randomPitch = true;
        return this;
    }

    public void Play(SoundData soundData)
    {
        if (soundData == null)
        {
            Debug.LogError("SoundData is null");
            return;
        }

        if (!soundManager.CanPlaySound(soundData)) return;

        SoundEmitter soundEmitter = soundManager.Get();
        soundEmitter.Initialize(soundData);
        soundEmitter.transform.position = position;
        soundEmitter.transform.parent = Core.Instance.transform;

        if (randomPitch)
        {
            soundEmitter.WithRandomPitch();
        }

        if (soundData.FrequentSound)
        {
            soundEmitter.Node = soundManager.FrequentSoundEmitters.AddLast(soundEmitter);
        }

        soundEmitter.Play();
        soundManager.RegisterActiveSound(soundData, soundEmitter);
    }
}