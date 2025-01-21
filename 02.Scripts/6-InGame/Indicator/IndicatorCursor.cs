using System.Collections.Generic;
using UnityEngine;

public class IndicatorCursor : IndicatorComponent
{
    [SerializeField] Material hologramMaterial;
    // id, key - obj
    Dictionary<int, HologramController> holograms = new Dictionary<int, HologramController>();
    HologramController currentHologram;
    
    public void Show(Unit unit, Vector2 coord)
    {
        if (!holograms.ContainsKey(unit.data.UnitBase.Key))
            AddHologram(unit);
        
        currentHologram = holograms[unit.data.UnitBase.Key];
        currentHologram.Enable(coord);
        
        base.Show(coord);
    }

    public override void Hide()
    {
        currentHologram.gameObject.SetActive(false);
        base.Hide();
    }

    void AddHologram(Unit unit)
    {
        int key = unit.data.UnitBase.Key;
        
        HologramController prefab = Resources.Load<HologramController>($"{Constants.Path.Units}Hologram/{key}_Hologram");
        HologramController instance = Instantiate(prefab);
        instance.Initialize(hologramMaterial);
        
        holograms.Add(key, instance);
    }
}