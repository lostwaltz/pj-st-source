using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleEmitter : MonoBehaviour
{
    [SerializeField] private ParticleSystem ParticleSystem;

    private Coroutine playingCoroutine;
    private Action onStopAction;
    private Effect effect;

    private void Awake()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
        effect = GetComponent<Effect>();
    }

    public ParticleEmitter OnComplete(Action action)
    {
        onStopAction += action;
        return this;
    }

    public void Stop()
    {
        if (playingCoroutine != null)
        {
            StopCoroutine(playingCoroutine);
            playingCoroutine = null;
        }

        onStopAction?.Invoke();
        onStopAction = null;

        ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        Core.ObjectPoolManager.ReleaseObject<Effect>(effect.Key, effect);
    }
    public void Play()
    {
        if (playingCoroutine != null)
        {
            StopCoroutine(playingCoroutine);
        }

        ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ParticleSystem.Clear();
        ParticleSystem.Play();

        playingCoroutine = StartCoroutine(WaitForParticleToEnd());
    }

    private IEnumerator WaitForParticleToEnd()
    {
        yield return new WaitWhile(() => ParticleSystem.isPlaying);
        Stop();
    }
}
