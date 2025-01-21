using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailHandler : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    public float randValue;
    public Transform parent;

    private void Update()
    {
        trailRenderer.transform.position =  parent.position.AddRandom(randValue);
    }

    private void OnDisable()
    {
        trailRenderer.Clear();
    }
}
