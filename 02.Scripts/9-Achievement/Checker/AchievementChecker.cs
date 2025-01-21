using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementChecker : MonoBehaviour
{
    public virtual float CheckIncrement(float curValue, float increment)
    {
        return curValue + increment; 
    }
}