using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class AudioClipContainer
{
    [Header("필요하다면 입력")]
    public string containerName;//
    public AudioClip[] SoundArray;
}

public class SoundHandler : MonoBehaviour
{
    private int lastSoundIndex = -1;


    public List<AudioClipContainer> soundContainer;

    private readonly Dictionary<string, SoundData> soundDataDict = new();

    private Dictionary<string, AudioClipContainer> containerDict = new(); //

    private void Awake()
    {
        InitializeSounds();
        InitializeContainerDictionary();
    }

    private void InitializeSounds()
    {
        foreach (var container in soundContainer)
            CacheAudioClips(container.SoundArray);
    }


    private void CacheAudioClips(AudioClip[] clips)
    {
        foreach (var clip in clips)
        {
            soundDataDict[clip.name] = new SoundData
            {
                Clip = clip,
                Volume = 0.2f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
    }

    public void PlaySound(Vector3 position)
    {
        foreach (var container in soundContainer)
            PlayGunSound(container.SoundArray, position);
    }

    private void PlayGunSound(AudioClip[] soundArray, Vector3 position)
    {
        if (soundArray == null || soundArray.Length == 0) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, soundArray.Length);
        } while (newIndex == lastSoundIndex && soundArray.Length > 1);

        lastSoundIndex = newIndex;
        AudioClip selectedSound = soundArray[newIndex];

        Core.SoundManager.CreateSoundBuilder()
            .WithPosition(position)
            .Play(soundDataDict[selectedSound.name]);
    }








    private void InitializeContainerDictionary()
    {
        containerDict.Clear();
        foreach (var container in soundContainer)
        {
            if (!string.IsNullOrEmpty(container.containerName))
            {
                containerDict[container.containerName] = container;
            }
        }
    }
    public void PlaySoundFromContainer(string containerName, Vector3 position)
    {
        if (containerDict.TryGetValue(containerName, out AudioClipContainer container))
        {
            PlayGunSound(container.SoundArray, position);
        }
        else
        {
            Debug.LogWarning($"컨테이너를 찾을 수 없습니다: {containerName}");
        }
    }

}
