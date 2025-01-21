using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : Currency
{
    public Credits() { Key = 0; }
    public override void Add(int amount)
    {
        Amount += amount;
    }
}
