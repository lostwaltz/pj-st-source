using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shard : Currency
{
    public Shard() { Key = 1; }
    public override void Add(int amount)
    {
        Amount += amount;
    }
}
