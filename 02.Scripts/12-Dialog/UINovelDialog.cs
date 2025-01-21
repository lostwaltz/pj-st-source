using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINovelDialog : UIBasicDialog
{
    [SerializeField] private TMP_Text MessageText;
    [SerializeField] private TMP_Text Title;
    
    [SerializeField] private Image BG;
    [SerializeField] private Image Char;
    
    [SerializeField] private CanvasGroup canvas;

    private Coroutine typingCoroutine;
    private Coroutine blockCoroutine;
    private WaitForSeconds waitForSecond = new WaitForSeconds(1.0f);

    public override void StartDialog(List<BasicDialog> dialogs)
    {
        base.StartDialog(dialogs);
        Fader fade = new Fader();
        StartCoroutine(fade.FadeCoroutine(canvas, 1, 0, 0.3f));
    }

    protected override void UpdateDialog(BasicDialog dialog)
    {
        base.UpdateDialog(dialog);
        
        Title.text = dialog.Data.CharacterName;
        
        BG.sprite = Resources.Load<Sprite>(dialog.Data.BackgroundSpritePath);
        
        Sprite sprite = Resources.Load<Sprite>(dialog.Data.CharacterLDSpritePath);
        if(sprite == null)
            Char.gameObject.SetActive(false);
        else
            Char.gameObject.SetActive(true);

        Char.sprite = sprite;
        
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        if (blockCoroutine != null)
            StopCoroutine(blockCoroutine);

        typingCoroutine = StartCoroutine(TypeText(dialog.Data.Dialog));
        blockCoroutine = StartCoroutine(BlockInput(0.2f));
    }

    private IEnumerator TypeText(string dialog)
    {
        MessageText.text = "";
        foreach (char letter in dialog)
        {
            MessageText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator BlockInput(float duration)
    {
        SetActiveInteractable(false);
        yield return new WaitForSeconds(duration);
        SetActiveInteractable(true);
    }
}