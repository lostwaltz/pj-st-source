using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIObjective : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ObjectiveText;


    [Header("Animation Settings")]
    [SerializeField] private RectTransform maskTransform; // 마스크용 RectTransform
    [SerializeField] private CanvasGroup canvasGroup;     // 전체 페이드 효과용
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Ease easeType = Ease.InOutQuad;

    private ChallengeGoalSO challengeGoal;
    private Vector2 originalSize;

    private void Awake()
    {
        // 초기 설정
        originalSize = maskTransform.sizeDelta;
        maskTransform.sizeDelta = new Vector2(0, originalSize.y);
        if (canvasGroup != null) canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        PlayShowAnimation();
    }

    public void SetChallengeGoal(ChallengeGoalSO goal)
    {
        challengeGoal = goal;
        if (goal != null)
        {
            ObjectiveText.text = goal.challengeGoalDescription;
        }
    }

    private void PlayShowAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(maskTransform.DOSizeDelta(originalSize, animationDuration)
            .SetEase(easeType));

        if (canvasGroup != null)
        {
            sequence.Join(canvasGroup.DOFade(1f, animationDuration * 0.8f));
        }

        sequence.OnComplete(() =>
        {
            DOTween.Sequence().AppendInterval(3f).OnComplete(() =>
            {
                PlayHideAnimation();
            });
        });
    }

    public void PlayHideAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        if (canvasGroup != null)
        {
            sequence.Join(canvasGroup.DOFade(0f, animationDuration * 0.8f));
        }

        sequence.OnComplete(() =>
        {
        });
    }
}
