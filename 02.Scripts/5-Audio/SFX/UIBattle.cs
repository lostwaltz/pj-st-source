using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattle
{
    private static SoundData uiBattleClickNormalSound;
    private static SoundData uiBattleClickOtherSound;
    private static SoundData uiBattleCharacterAssign;  //
    private static SoundData uiBattleCharacterDeploy; //
    private static SoundData uiBattleCharacterFilteringWindowOpen;
    private static SoundData uiBattleCharacterFilteringWindowClose;
    private static SoundData uiBattleSFXIntro;
    private static SoundData uiBattleClickVirtualMove;
    private static SoundData uiBattleClickVirtualAttack;
    private static SoundData uiBattlePlayerTurn;
    private static SoundData uiBattleEnemyTurn;

    public static void PlayUIBattleSFXIntro()
    {
        if (uiBattleSFXIntro == null)
        {
            uiBattleSFXIntro = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UIBATTLE_PATH + "UI_Battle#25 (UI_Battle_SFX_Intro)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(uiBattleSFXIntro);
    }
    public static void PlayUIBattleClickNormalSound()
    {
        if (uiBattleClickNormalSound == null)
        {
            uiBattleClickNormalSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UIBATTLE_PATH + "UI_Battle#42 (UI_Battle_Click_Normal)"),
                Volume = 1.0f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(uiBattleClickNormalSound);
    }
    public static void PlayUIBattleClickOtherSound()
    {
        if (uiBattleClickOtherSound == null)
        {
            uiBattleClickOtherSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UIBATTLE_PATH + "UI_Battle#37 (UI_Battle_Click_Other)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(uiBattleClickOtherSound);
    }
    public static void PlayUIBattleCharacterDeploy()
    {
        if (uiBattleCharacterDeploy == null)
        {
            uiBattleCharacterDeploy = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UIBATTLE_PATH + "UI_Battle#23 (UI_Battle_Click_CharacterDeploy)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(uiBattleCharacterDeploy);
    }
    public static void PlayUIBattleCharacterAssign()
    {
        if (uiBattleCharacterAssign == null)
        {
            uiBattleCharacterAssign = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UIBATTLE_PATH + "UI_Battle#22 (UI_Battle_Click_CharacterAssign)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(uiBattleCharacterAssign);
    }
    public static void PlayUIBattleCharacterFilteringWindowOpen()
    {
        if (uiBattleCharacterFilteringWindowOpen == null)
        {
            uiBattleCharacterFilteringWindowOpen = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UIBATTLE_PATH + "UI_Battle#32 (UI_Battle_SFX_FilteringWindow)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(uiBattleCharacterFilteringWindowOpen);
    }
    public static void PlayUIBattleCharacterFilteringWindowClose()
    {
        if (uiBattleCharacterFilteringWindowClose == null)
        {
            uiBattleCharacterFilteringWindowClose = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UIBATTLE_PATH + "UI_Battle#33 (UI_Battle_SFX_FilteringWindowClose)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };

        }

        Core.SoundManager.CreateSoundBuilder().Play(uiBattleCharacterFilteringWindowClose);
    }
    public static void PlayUIBattleClickVirtualMove()
    {
        if (uiBattleClickVirtualMove == null)
        {
            uiBattleClickVirtualMove = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UIBATTLE_PATH + "UI_Battle#31 (UI_Battle_Click_VirtualMove)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(uiBattleClickVirtualMove);
    }
    public static void PlayUIBattleClickVirtualAttack()
    {
        if (uiBattleClickVirtualAttack == null)
        {
            uiBattleClickVirtualAttack = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UIBATTLE_PATH + "UI_Battle#30 (UI_Battle_Click_VirtualAttack)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(uiBattleClickVirtualAttack);
    }
    public static void PlayUIBattlePlayerTurn()
    {
        if (uiBattlePlayerTurn == null)
        {
            uiBattlePlayerTurn = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UIBATTLE_PATH + "UI_Battle#39 (UI_Battle_SFX_RoundStart)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(uiBattlePlayerTurn);
    }
    public static void PlayUIBattleEnemyTurn()
    {
        if (uiBattleEnemyTurn == null)
        {
            uiBattleEnemyTurn = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UIBATTLE_PATH + "UI_Battle#40 (UI_Battle_SFX_RoundStart_Enemy)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(uiBattleEnemyTurn);
    }




}
