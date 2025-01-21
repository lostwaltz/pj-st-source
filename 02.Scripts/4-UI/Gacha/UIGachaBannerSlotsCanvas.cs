using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class UIGachaBannerSlotsCanvas : UIBase
{
    public List<UIGachaBannerUnitSlot> GachaUnitBannerSlots = new ();
    public List<UIPieceSlot> uiPieceSlots = new ();
    private Camera mainCam;
    private Canvas canvas;
    public GameObject GachaBannerPanel;
    public GameObject uiPiecePanel;
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button skipBtn;
    private Sequence sequence;
    private bool isSkipping = false;

    private void Awake()
    {
        exitBtn.onClick.AddListener(OnClickExitButton);
        skipBtn.onClick.AddListener(OnClickSkipButton); 
    }

    private void Start()
    {
        mainCam = Camera.main;
        canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = mainCam;
        horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;

        PlaySlotAnimation(0); // 첫 번째 슬롯부터 시작
    }

    //슬롯 애니메이션 
    private void PlaySlotAnimation(int index)
    {
        if (isSkipping) return;
        if (index >= GachaUnitBannerSlots.Count)
        {
            if (uiPieceSlots!= null)
            {
                foreach (var uiPieceSlot in uiPieceSlots)
                {
                    uiPieceSlot.gameObject.SetActive(true);
                }
            }

            return; // 모든 슬롯이 끝나면 종료
        }

        if (GachaUnitBannerSlots.Count == 1)
        {
            horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        }

        GachaSound.PlayGachaShowCharacterSSR();

        GachaUnitBannerSlots[index].gameObject.SetActive(true);

        Transform childImage = GachaUnitBannerSlots[index].transform.GetChild(0);

        sequence = DOTween.Sequence();

        // 슬롯만 스케일 변경
        sequence.Append(GachaUnitBannerSlots[index].transform.DOScale(new Vector3(1, 1.5f, 1), 0.5f)
            .OnUpdate(() =>
            {
                // 애니메이션 중에도 하위 이미지 스케일을 원본 유지
                if (childImage != null)
                {
                    childImage.localScale = new Vector3(
                        1 / GachaUnitBannerSlots[index].transform.localScale.x,
                        1 / GachaUnitBannerSlots[index].transform.localScale.y,
                        1 / GachaUnitBannerSlots[index].transform.localScale.z
                    );
                }
            }));

        sequence.Append(GachaUnitBannerSlots[index].transform.DOScale(new Vector3(1, 1, 1), 0.3f)
            .OnUpdate(() =>
            {
                // 스케일 애니메이션 중에도 보정 유지
                if (childImage != null)
                {
                    childImage.localScale = new Vector3(
                        1 / GachaUnitBannerSlots[index].transform.localScale.x,
                        1 / GachaUnitBannerSlots[index].transform.localScale.y,
                        1 / GachaUnitBannerSlots[index].transform.localScale.z
                    );
                }
            }));

        // 현재 시퀀스가 완료되면 다음 슬롯의 애니메이션 실행
        sequence.OnComplete(() => PlaySlotAnimation(index + 1));
    }
    void OnClickSkipButton()
    {
        isSkipping = true;

        // 현재 시퀀스 완료 / Kill은 즉시
        sequence?.Complete();

        // 모든 배너 & 유닛조각 활성화
        foreach (var slot in GachaUnitBannerSlots)
        {
            slot.gameObject.SetActive(true);
        }

        if (uiPieceSlots != null)
        {
            foreach (var uiPieceSlot in uiPieceSlots)
            {
                uiPieceSlot.gameObject.SetActive(true);
            }
        }
    }

    void OnClickExitButton()
    {
        Close();
        UISound.PlayBackButtonClick();
        Core.SceneLoadManager.LoadScene("LobbyScene");
        Core.SceneLoadManager.sceneDic["LobbyScene"].OnEnterEvent += OpenAndClose;
    }

    void OpenAndClose()
    {
        Core.UIManager.CloseUI<UILobbyMain>();
        Core.UIManager.OpenUI<UIGachaManager>();
    }
}


