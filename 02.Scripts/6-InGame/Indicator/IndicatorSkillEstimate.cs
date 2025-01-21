using System;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorSkillEstimate : IndicatorComponent
{
    Camera mainCam;
    
    [SerializeField] [Range(0f, 1f)] float lerpPoint = 0.8f;
    [SerializeField] Sprite[] icons;
    [SerializeField] private SkillEstimateInfo info;
    [SerializeField] private Transform infoParent;

    List<SkillEstimateInfo> instances = new List<SkillEstimateInfo>();
    private Vector3 position;
    
    private void Awake()
    {
        mainCam = Camera.main;

        for (int i = 0; i < icons.Length; i++)
        {
            var inst = Instantiate(info, infoParent);
            inst.Set(0, icons[i]);
            instances.Add(inst);
            
            inst.gameObject.SetActive(false);
        }
    }
    
    public override void Show(Vector3 newPosition)
    {
        position = newPosition;
        gameObject.SetActive(true);
    }
    
    public void ShowWithoutInfo(Vector3 newPosition)
    {
        Show(newPosition);

        for (int i = 0; i < instances.Count; i++)
            instances[i].gameObject.SetActive(false);
    }

    public void SetInfo(int stability, int cover, int critical) // int[] infos)
    {
        // for (int i = 0; i < infos.Length; i++)
        // {
        //     instances[i].gameObject.SetActive(infos[i] > 0);
        //     instances[i].Set(infos[i]);    
        // }
        
        instances[0].gameObject.SetActive(stability > 0);
        instances[0].Set(stability);
        
        instances[1].gameObject.SetActive(cover > 0);
        instances[1].Set(cover);
        
        instances[2].gameObject.SetActive(true);
        instances[2].Set(critical);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(mainCam.transform.position, position, lerpPoint);
        transform.forward = mainCam.transform.forward;
    }
}
