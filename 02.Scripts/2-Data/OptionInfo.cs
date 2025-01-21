using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class OptionInfo : DataModel
{
    /// <summary>
    /// 이름
    /// </summary>
    public string Name;

    /// <summary>
    /// 설명
    /// </summary>
    public string Description;

    /// <summary>
    /// 최소값
    /// </summary>
    public int MinValue;

    /// <summary>
    /// 최대값
    /// </summary>
    public int MaxValue;

}
