using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class DialogInfo : DataModel
{
    /// <summary>
    /// 대사 출력자 이름
    /// </summary>
    public string CharacterName;

    /// <summary>
    /// 캐릭터 스프라이트 경로
    /// </summary>
    public string CharacterSpritePath;

    /// <summary>
    /// 캐릭터 전신 스프라이트 경로
    /// </summary>
    public string CharacterLDSpritePath;

    /// <summary>
    /// 해당 다이얼로그 배경화면
    /// </summary>
    public string BackgroundSpritePath;

    /// <summary>
    /// 해당 다이얼로그 텍스트
    /// </summary>
    public string Dialog;

}
