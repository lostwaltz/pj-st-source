using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//TODO: 구조 개선
public class SkillFactory
{
    public PassiveSkill CreatePassiveSkill(int key, Unit owner, SkillInstance instance)
    {
        return key switch
        {
            305000 => new Recovery(owner, instance),
            305001 => new RemoldingSupply(owner, instance),
            305002 => new SupportFire(owner, instance),
            _ => new SupportFire(owner, instance)
        };
    }
}
