using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public enum PoolType
{
    Default,
    ParticleSystem,
}

public class ObjectPoolManager
{
    private readonly Dictionary<string, IObjectPoolWrapper<Component>> poolDictionary = new();

    public interface IObjectPoolWrapper<out T>
    {
        T Get();
        void Release(Component obj);
    }

    public void Init()
    {
        SceneManager.sceneLoaded += (_, _) => poolDictionary.Clear();  
    }
    
    private class ObjectPoolWrapper<T> : IObjectPoolWrapper<T> where T : Component
    {
        private readonly ObjectPool<T> objectPool;
        
        public ObjectPoolWrapper(Func<T> createFunc, Action<T> actionOnGet, Action<T> actionOnRelease, Action<T> actionOnDestroy, int defaultCapacity, int maxSize)
        {
            objectPool = new ObjectPool<T>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, false, defaultCapacity, maxSize);
        }

        public T Get()
        {
            return objectPool.Get();
        }

        public void Release(Component obj)
        {
            if (false == (obj is T tObj))
                return;
            
            objectPool.Release(tObj);
        }
    }

    public class ObjectPoolWrapperParticle<T> : IObjectPoolWrapper<T> where T : Component 
    {
        private readonly Queue<T> poolQueue = new();
        private readonly Func<T> createFunc;
        private readonly Action<T> actionOnGet;
        private readonly Action<T> actionOnRelease;
        private readonly int maxSize;

        public ObjectPoolWrapperParticle(Func<T> createFunc, Action<T> actionOnGet, Action<T> actionOnRelease, int defaultCapacity, int maxSize)
        {
            this.createFunc = createFunc;
            this.actionOnGet = actionOnGet;
            this.actionOnRelease = actionOnRelease;
            this.maxSize = maxSize;
            
            for (var i = 0; i < defaultCapacity; i++)
            {
                T obj = createFunc();
                actionOnRelease?.Invoke(obj);
                poolQueue.Enqueue(obj);
            }
        }

        public T Get()
        {
            var component = poolQueue.Dequeue();
            poolQueue.Enqueue(component);
            actionOnGet?.Invoke(component);
            
            return component;
        }

        public void Release(Component obj)
        {
            if (obj is not T tObj)
                return;
            
            actionOnRelease?.Invoke(tObj);
        }
    }


    public void CreateNewPool<T>(string key, T pooledObject, int capacity, int maxSize) where T : Component
    {
        if (poolDictionary.ContainsKey(key))
            return;

        poolDictionary[key] = new ObjectPoolWrapper<T>(() => CreateInstance(pooledObject), OnGetFromPool,
            OnReleaseToPool, OnDestroyPooledObject, capacity, maxSize);

        for(var i = 0; i < capacity; i++)
            poolDictionary[key].Release(Object.Instantiate(pooledObject));
    }
    public void CreateNewPoolParticle<T>(string key, T pooledObject, int capacity, int maxSize) where T : Component 
    {
        if (poolDictionary.ContainsKey(key))
            return;

        poolDictionary[key] = new ObjectPoolWrapperParticle<T>(() => CreateInstance(pooledObject), OnGetFromPool,
            OnReleaseToPool, capacity, maxSize);
    }

    public T GetPooledObject<T>(string key) where T : Component
    {
        if (!poolDictionary.TryGetValue(key, out var poolWrapper))
            return null;

        T component = poolWrapper.Get() as T;

        return component;
    }

    public void ReleaseObject<T>(string key, T poolObject) where T : Component
    {
        if (!poolDictionary.TryGetValue(key, out var poolWrapper))
            return;

        poolWrapper.Release(poolObject);
    }

    private static T CreateInstance<T>(T pooledObject) where T : Component
    {
        T component = Object.Instantiate(pooledObject);
        component.gameObject.SetActive(false);
        return component;
    }

    private static void OnGetFromPool(Component component)
    {
        component.gameObject.SetActive(true);
    }

    private static void OnReleaseToPool(Component component)
    {
        component.gameObject.SetActive(false);
    }

    private static void OnDestroyPooledObject(Component component)
    {
        Object.Destroy(component.gameObject);
    }

    public void ReleaseObjectPool(string key)
    {
        poolDictionary.Remove(key);
    }

    public void ReleaseObjectPool()
    {
        poolDictionary.Clear();
    }
}
