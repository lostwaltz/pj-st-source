using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class BehaviorFactoryData
{
    public float animationDuration = 1f;
    public BehaviorFactory BehaviorFactory;
    
    public GameObject target;
}

public class BehaviorData
{
    public readonly float AnimationDuration;
    public readonly BehaviorBase Behavior;
    
    public GameObject Target;

    public BehaviorData(float animationDuration, BehaviorBase behavior, GameObject target)
    {
        this.AnimationDuration = animationDuration;
        this.Behavior = behavior;
        Target = target;
    }
}

public class UIAnimation : UIBase
{
    [SerializeField] private GameObject spawnVFX;
    
    [Header("OnStart")]
    [SerializeField] private List<BehaviorFactoryData> OnStartBehaviorFactoryList;
    private List<BehaviorData> onStartEffectList;
    
    [Header("OnClick")]
    [SerializeField] private List<BehaviorFactoryData> OnClickBehaviorFactoryList;
    private List<BehaviorData> onClickEffectList;
    
    [Header("OnDown")]
    [SerializeField] private List<BehaviorFactoryData> OnDownBehaviorFactoryList;
    private List<BehaviorData> onDownEffectList;
    
    [Header("OnUp")]
    [SerializeField] private List<BehaviorFactoryData> OnUpBehaviorFactoryList;
    private List<BehaviorData> onUpEffectList;
    
    [Header("OnEnter")]
    [SerializeField] private List<BehaviorFactoryData> OnEnterBehaviorFactoryList;
    private List<BehaviorData> onEnterEffectList;
    
    [Header("OnExit")]
    [SerializeField] private List<BehaviorFactoryData> OnExitBehaviorFactoryList;
    private List<BehaviorData> onExitEffectList;
    
    [Header("OnOpen")]
    [SerializeField] private List<BehaviorFactoryData> OnOpenBehaviorFactoryList;
    private List<BehaviorData> onOpenEffectList;
    
    [Header("OnClose")]
    [SerializeField] private List<BehaviorFactoryData> OnCloseBehaviorFactoryList;
    private List<BehaviorData> onCloseEffectList;
    
    [Header("OnTrigger")]
    [SerializeField] private List<BehaviorFactoryData> OnTriggerBehaviorFactoryList;
    private List<BehaviorData> onTriggerEffectList;

    protected bool IgnoreExitEvent;
    protected bool IgnoreEnterEvent;
    
    protected virtual void Awake()
    {
        BindEvent(gameObject, _ =>
        {
            onClickEffectList ??= InitBehaviorFactoryList(OnClickBehaviorFactoryList);
            PlayEffects(onClickEffectList);
        });
        
        BindEvent(gameObject, _ =>
        {
            onDownEffectList ??= InitBehaviorFactoryList(OnDownBehaviorFactoryList);
            PlayEffects(onDownEffectList);
        }, UIEvent.Down);
        
        BindEvent(gameObject, _ =>
        {
            onUpEffectList ??= InitBehaviorFactoryList(OnUpBehaviorFactoryList);
            PlayEffects(onUpEffectList);
        }, UIEvent.Up);
        
        BindEvent(gameObject, _ =>
        {
            if(IgnoreEnterEvent) return;
            
            onEnterEffectList ??= InitBehaviorFactoryList(OnEnterBehaviorFactoryList);
            PlayEffects(onEnterEffectList);
        }, UIEvent.Enter);
        
        BindEvent(gameObject, _ =>
        {
            if(IgnoreExitEvent) return;
            
            onExitEffectList ??= InitBehaviorFactoryList(OnExitBehaviorFactoryList);
            PlayEffects(onExitEffectList);
        }, UIEvent.Exit);
    }

    protected virtual void Start()
    {
        onStartEffectList ??= InitBehaviorFactoryList(OnStartBehaviorFactoryList);
        PlayEffects(onStartEffectList);
    }

    private List<BehaviorData> InitBehaviorFactoryList(List<BehaviorFactoryData> behaviorFactoryList)
    {
        List<BehaviorData> list = new();
        
        foreach (var factory in behaviorFactoryList)
            list.Add(new BehaviorData(factory.animationDuration, factory.BehaviorFactory.CreateBehaviorEffects(), factory.target));

        return list;
    }
    
    public void OnTrigger()
    {
        onTriggerEffectList ??= InitBehaviorFactoryList(OnTriggerBehaviorFactoryList);
        PlayEffects(onTriggerEffectList);
    }

    public void PlayEffects(List<BehaviorData> effects, Action callback = null)
    {
        PlayAnimation(0, effects, callback);
        
        if (spawnVFX != null)
            Instantiate(spawnVFX, transform.position, Quaternion.identity);

        if(true == TryGetComponent<AudioSource>(out AudioSource audioSource))
            audioSource.Play();
    }

    private void PlayAnimation(int index, List<BehaviorData> effects, Action callback = null)
    {
        if (index < 0 || index >= effects.Count) return;

        if (null == effects[index].Target)
            effects[index].Target = gameObject;
        
        effects[index].Behavior.BehaviorExecute(effects[index].Target.transform, effects[index].AnimationDuration).OnComplete(() =>
        {
            PlayAnimation(index + 1, effects);

            if (effects.Count == index + 1)
            {
                callback?.Invoke();
            }
        });
    }

    public override void Open()
    {
        onOpenEffectList ??= InitBehaviorFactoryList(OnOpenBehaviorFactoryList);
        PlayEffects(onOpenEffectList, base.Open);
        
        if(onOpenEffectList.Count <= 0)
            base.Open();
    }

    public override void Close()
    {
        onCloseEffectList ??= InitBehaviorFactoryList(OnCloseBehaviorFactoryList);
        PlayEffects(onCloseEffectList, base.Close);
        
        if(onCloseEffectList.Count <= 0)
            base.Close();
    }
}
