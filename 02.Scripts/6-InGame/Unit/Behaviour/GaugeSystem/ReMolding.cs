using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReMolding : Gauge
{
    public override void Charge(int amount)
    {
        base.Charge(amount);
        
        //TODO: 리몰딩 회복시 행동
    }

    public override bool Use(int amount)
    {
        var result = base.Use(amount);

        //TODO: 리몰딩 사용시 행동
        
        return result;
    }
}
