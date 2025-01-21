using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlottable  
{
    public void Initialize();
    public void SetHighlight(bool highlight);
}

public class UISlotGroupSystem : MonoBehaviour
{
    private readonly List<ISlottable> slots = new ();

    public void RegisterSlot(ISlottable slot)
    {
        if (!slots.Contains(slot))
            slots.Add(slot);
    }

    public void DeselectAll()
    {
        foreach (var slot in slots)
                slot.SetHighlight(false);
    }

    public void InitAllSlots()
    {
        foreach (var slot in slots)
            slot.Initialize();
    }
}
