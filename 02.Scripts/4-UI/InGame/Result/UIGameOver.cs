using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : UIBase
{
    [SerializeField] private TMP_Text textStageTitle;
    [SerializeField] private TMP_Text textLoseTitle;
    
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button confirmButton;

    private void Awake()
    {
        confirmButton.onClick.AddListener(Confirm);
        tryAgainButton.onClick.AddListener(TryAgain);
    }

    public void UpdateUI(string stageTitle, string loseTitle)
    {
        textStageTitle.text = stageTitle;
        textLoseTitle.text = loseTitle;
    }

    private void Confirm()
    {
        GameManager.Instance.BackToLobby();
    }

    private void TryAgain()
    {
        Time.timeScale = 1f;
        Core.SceneLoadManager.LoadScene("GameScene", Core.DataManager.SelectedStage.sceneName);
    }
}
