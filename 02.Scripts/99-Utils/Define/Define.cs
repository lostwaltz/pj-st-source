using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public static int[,] FourDirection = new int[2, 4]
    {
        { 1, -1, 0, 0 },    // x : 0  
        { 0, 0, 1, -1 }     // y : 1 
    };
    
    public static int[,] EightDirection = new int[2, 8]
    {
        { 1, -1, 0, 0, 1, 1, -1, -1 }, // x
        { 0, 0, 1, -1, 1, -1, 1, -1 }  // y
    };
    
}
