using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageDialog : UIBasicDialog
{
    [SerializeField] private TMP_Text MessageText;
    [SerializeField] private TMP_Text Title;

    [SerializeField] private Image Avatar;
    
    private Coroutine typingCoroutine;
    
    private BasicDialog curDialog;

    private Action<string> OnUpdateDialog;
    
    public override void StartDialog(List<BasicDialog> dialogs)
    {
        base.StartDialog(dialogs);
        
        MessageText.text = "";
    }

    protected override void OpenProcedure()
    {
        base.OpenProcedure();

        OnUpdateDialog += UpdateText;
        
        UpdateText(DialogsContainer.First().Data.Dialog);
    }

    protected override void UpdateDialog(BasicDialog dialog)
    {
        base.UpdateDialog(dialog);

        OnTrigger();
        
        curDialog = dialog;
        
        Title.text = dialog.Data.CharacterName;
        
        Sprite sprite = Resources.Load<Sprite>(dialog.Data.CharacterSpritePath);
        if(sprite == null)
            sprite = Resources.Load<Sprite>("Sprite/Dialog/plain_black_16x16");

        Avatar.sprite = sprite;
        
        OnUpdateDialog?.Invoke(dialog.Data.Dialog);
    }

    private void UpdateText(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        
        typingCoroutine = StartCoroutine(TypeText(text));
    }
    
    private IEnumerator TypeText(string dialog)
    {
        MessageText.text = "";
        foreach (char letter in dialog)
        {
            MessageText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
    }
}
