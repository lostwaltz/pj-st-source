using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIUnitManageClassInfo : UIBase
{
    [SerializeField] private Button btnClose;

    [Header("Fade 설정")]
    [SerializeField] private CanvasGroup CanvasGroup;
    private float fadeinduration = 0.3f;
    private float fadeoutduration = 0.3f;

    private void Awake()
    {
        CanvasGroup.alpha = 0;
        btnClose.onClick.AddListener(OnClose);
    }


    private void Start()
    {

    }

    public override void Open()
    {
        base.Open();
        UISound.PlayInterfaceOpen();

        CanvasGroup.DOFade(1, fadeinduration)
        .SetEase(Ease.OutQuad)
        .OnComplete(() =>
        {

        });
    }

    private void OnClose()
    {
        UISound.PlayWindowClose();
        CanvasGroup.DOFade(0, fadeoutduration)
        .SetEase(Ease.InQuad)
        .OnComplete(() =>
        {
            base.Close();
            CanvasGroup.alpha = 0;
        });

    }
}
