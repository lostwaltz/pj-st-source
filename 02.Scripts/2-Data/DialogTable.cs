using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class DialogTable : DataModel
{
    /// <summary>
    /// 다이얼로그 주제
    /// </summary>
    public string Title;

    /// <summary>
    /// 다이얼로그 메시지 모음
    /// </summary>
    public List<int> DialogList;

}
