using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnumTypes;
using UnityEngine;

[RequireComponent(typeof(UnitAnimation))]
[RequireComponent(typeof(UnitCoverSystem))]
public class HologramController : UnitMovement, IClickable
{
    Func<Vector2, UnitType, int, Transform> getCoverPoint;

    public Vector2 Coord;

    WaitForSeconds waitForSec = new WaitForSeconds(0.5f);
    
    private void Awake()
    {
        animation = GetComponent<UnitAnimation>();
        animation.Initialize();

        cover = GetComponent<UnitCoverSystem>();
        cover.Initialize();
    }

    public void Initialize(Material material)
    {
        SkinnedMeshRenderer[] mesh = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < mesh.Length; i++)
        {
            List<Material> list = new List<Material>();
            for (int j = 0; j < mesh[i].materials.Length; j++)
            {
                Material mHologram = new Material(material);
                mHologram.SetTexture("_BaseMap", mesh[i].materials[j].GetTexture("_BaseMap"));
                list.Add(mHologram);
            }
            mesh[i].SetMaterials(list);
        }
    }

    public void Enable(Vector2 coord)
    {
        Coord = coord;
        
        gameObject.SetActive(true);
        transform.position = StageManager.Instance.cellMaps[coord].transform.position;
        
        if(EnableHandler != null)
            StopCoroutine(EnableHandler);

        EnableHandler = StartCoroutine(EnableAnimation(coord));
    }

    Coroutine EnableHandler;
    IEnumerator EnableAnimation(Vector2 coord)
    {
        animation.SetIdle();
        yield return waitForSec;
        
        Transform coverPoint = cover.CheckCoverPoint(coord, UnitType.PlayableUnit, out int obstacleType);
        if(coverPoint != null)
            cover.Cover(coverPoint, obstacleType);
        
        yield return waitForSec;

        EnableHandler = null;
    }

    public string GetInfo()
    {
        return "";
    }
}
