using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UICombatResult : UIBase
{
    [SerializeField] private List<UICombatResultSlot> slots = new();
    [SerializeField] TextMeshProUGUI txtStage;
    
    private void Start()
    {
        BindEvent(gameObject, _ =>
        {
            MusicManager.Instance.Stop();
            Core.SoundManager.StopAll();
            Close();
            Core.UIManager.OpenUI<UICombatResultReward>();
        });

        txtStage.text = StageManager.Instance.stageData.stageName;
    }

    protected override void OpenProcedure()
    {
        Core.UIManager.CloseUI<UIBattleCanvas>();

        foreach (var unit in GameUnitManager.Instance.Units[UnitType.PlayableUnit])
        {
            unit.data.AddExp(Random.Range(10, 1001));
        }

        var units = GameUnitManager.Instance.Units[UnitType.PlayableUnit];
        for (var i = 0; i < slots.Count; i++)
        {
            if (i >= units.Count)
            {
                slots[i].gameObject.SetActive(false);
                continue;
            }

            slots[i].UpdateSlot(units[i].data);
        }
        MusicManager.Instance.Stop();
        BGM.PlayBattleWinBGM();
    }
}
