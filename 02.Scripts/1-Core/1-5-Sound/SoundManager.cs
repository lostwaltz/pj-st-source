using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class SoundManager
{
    private IObjectPool<SoundEmitter> soundEmitterPool;
    private readonly List<SoundEmitter> activeSoundEmitters = new();
    public readonly LinkedList<SoundEmitter> FrequentSoundEmitters = new();
    private Dictionary<SoundData, SoundEmitter> activeDataEmitters = new Dictionary<SoundData, SoundEmitter>();

    private SoundEmitter soundEmitterPrefab;

    private bool collectionCheck = true;
    private int defaultCapacity = 10;
    private int maxPoolSize = 100;
    private int maxSoundInstances = 30;

    /// <summary>
    /// 마스터 볼륨 추가함
    /// </summary>
    private float masterVolume = 1.0f;
    public float MasterVolume
    {
        get => masterVolume;
        set
        {
            masterVolume = Mathf.Clamp01(value);
            UpdateAllEmittersVolume();
        }
    }
    private void UpdateAllEmittersVolume()
    {
        foreach (var emitter in activeSoundEmitters)
        {
            emitter.UpdateVolume(masterVolume);
        }
    }
    ///

    public void Init(bool collectionCheck, int defaultCapacity, int maxPoolSize, int maxSoundInstances)
    {
        this.collectionCheck = collectionCheck;
        this.defaultCapacity = defaultCapacity;
        this.maxPoolSize = maxPoolSize;
        this.maxSoundInstances = maxSoundInstances;

        soundEmitterPrefab = Resources.Load<SoundEmitter>("Prefabs/Sound/SoundEmitter");

        InitializePool();
    }

    public SoundBuilder CreateSoundBuilder() => new SoundBuilder(this);

    public bool CanPlaySound(SoundData data)
    {
        if (!data.FrequentSound) return true;

        if (FrequentSoundEmitters.Count < maxSoundInstances) return true;

        try
        {
            FrequentSoundEmitters.First.Value.Stop();
            return true;
        }
        catch
        {
            Debug.Log("SoundEmitter is already released");
        }

        return false;
    }

    public SoundEmitter Get()
    {
        return soundEmitterPool.Get();
    }

    public void ReturnToPool(SoundEmitter soundEmitter)
    {
        soundEmitterPool.Release(soundEmitter);
    }

    public void StopAll()
    {
        for (int i = 0; i < activeSoundEmitters.Count; i++)
        {
            activeSoundEmitters[i].Stop();
        }
        activeDataEmitters.Clear();
    }

    private void InitializePool()
    {
        soundEmitterPool = new ObjectPool<SoundEmitter>(
            CreateSoundEmitter,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            collectionCheck,
            defaultCapacity,
            maxPoolSize);
    }

    private SoundEmitter CreateSoundEmitter()
    {
        var soundEmitter = Object.Instantiate(soundEmitterPrefab);
        soundEmitter.gameObject.SetActive(false);
        return soundEmitter;
    }

    private void OnTakeFromPool(SoundEmitter soundEmitter)
    {
        soundEmitter.gameObject.SetActive(true);
        activeSoundEmitters.Add(soundEmitter);
    }

    private void OnReturnedToPool(SoundEmitter soundEmitter)
    {
        if (soundEmitter.Node != null)
        {
            FrequentSoundEmitters.Remove(soundEmitter.Node);
            soundEmitter.Node = null;
        }

        soundEmitter.gameObject.SetActive(false);
        activeSoundEmitters.Remove(soundEmitter);
    }

    private void OnDestroyPoolObject(SoundEmitter soundEmitter)
    {
        Object.Destroy(soundEmitter.gameObject);
    }

    public void RegisterActiveSound(SoundData data, SoundEmitter emitter)
    {
        activeDataEmitters[data] = emitter;
    }

    public void StopSound(SoundData data)
    {
        if (activeDataEmitters.TryGetValue(data, out SoundEmitter emitter))
        {
            emitter.Stop();
            activeDataEmitters.Remove(data);
        }
    }
}