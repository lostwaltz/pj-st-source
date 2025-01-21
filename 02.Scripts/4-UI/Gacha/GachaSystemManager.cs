using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GachaSystemManager : MonoBehaviour
{
    private DataManager dataManager;

    //생성될 프리팹
    [SerializeField] private UIGachaCanvas uiSetGachaCanvas;
    [SerializeField] private UIGachaBannerUnitSlot uiSetGachaBannerUnitSlot;
    [SerializeField] private UIPieceSlot uiSetPieceSlot;
    [SerializeField] private TMP_Text textToolTip;
    [SerializeField] private Button skipBtn;
    private UIGachaBannerSlotsCanvas uiGachaBannerSlotsCanvasPrefab;

    //실체
    private UIGachaCanvas curGachaCanvas; // 현재 생성된 프리팹 인스턴스
    [SerializeField] private int count = 0; // 현재 생성 중인 프리팹의 인덱스

    private List<UIGachaCanvas> uiGachaCanvasList = new();
    private List<UnitInstance> gachaResultList = new();

    private Animator animator;

    private void OnEnable()
    {
        Core.InputManager.OnClickReceived += OnClickNextPrefab; //마우스 왼쪽클릭 콜백 구독
        skipBtn.onClick.AddListener(OnClickSkipButton);
        Init();
    }

    public void Init()
    {
        dataManager = Core.DataManager;

        uiGachaBannerSlotsCanvasPrefab = Core.UIManager.GetUI<UIGachaBannerSlotsCanvas>();
        uiGachaBannerSlotsCanvasPrefab.Close();
        //uiPiecePanel = Core.UIManager.GetUI<UIPiecePanel>();
        //uiPiecePanel.Close();

        //데이터 매니저에 키값 가져오기
        SetUnitPrefabs(dataManager.GachaCount, dataManager.GachaKey);

        textToolTip.gameObject.SetActive(true);
    }

    //가치유닛프리팹 갯수 세팅 및 생성
    public void SetUnitPrefabs(int prefabsCount, int gachaUnitKey)
    {
        uiGachaCanvasList.Clear();
        gachaResultList.Clear();

        for (int i = 0; i < prefabsCount; i++)
        {
            //TODO : 프리팹 미리생성, 비활성화 + I번째에 I번째 정보 세팅 해주기

            UIGachaCanvas uiGachaCanvas = Instantiate(uiSetGachaCanvas);
            uiGachaCanvas.gameObject.SetActive(false);
            uiGachaCanvasList.Add(uiGachaCanvas);
            UIGachaBannerUnitSlot gachaUnitBannerSlot = Instantiate(uiSetGachaBannerUnitSlot);
            gachaUnitBannerSlot.gameObject.SetActive(false);
            gachaUnitBannerSlot.transform.SetParent(uiGachaBannerSlotsCanvasPrefab.GachaBannerPanel.transform);
            uiGachaBannerSlotsCanvasPrefab.GachaUnitBannerSlots.Add(gachaUnitBannerSlot);

            SetGachaInfoInit(gachaUnitKey); // 뽑기 결과 전체데이터 세팅

            uiGachaCanvasList[i].Init(gachaResultList[i]);
            uiGachaCanvas.Init(gachaResultList[i]);
            uiGachaBannerSlotsCanvasPrefab.GachaUnitBannerSlots[i].Init(gachaResultList[i]);

            //보유유닛 추가해주기
            if (Core.UnitManager.GetUnitInfoByKey(gachaResultList[i].UnitBase.Key) != null)
            {
                UIPieceSlot uiPieceSlot = null;

                if (uiGachaBannerSlotsCanvasPrefab.uiPieceSlots != null)
                {
                    foreach (var slot in uiGachaBannerSlotsCanvasPrefab.uiPieceSlots)
                    {
                        if (slot.unitKey == gachaResultList[i].UnitBase.Key) //UnitKey는 슬롯에 저장된 키값확인
                        {
                            uiPieceSlot = slot;
                            break;
                        }
                    }
                    
                }
                if (uiPieceSlot != null)
                {
                    // 이미 생성된 슬롯이 있다면 Count만 증가 및 텍스트 업데이트
                    uiPieceSlot.unitCount += 1;
                    uiPieceSlot.Init(gachaResultList[i], uiPieceSlot.unitCount, gachaResultList[i].UnitBase.Key);
                }
                else
                {
                    uiPieceSlot = Instantiate(uiSetPieceSlot);
                    uiPieceSlot.transform.SetParent(uiGachaBannerSlotsCanvasPrefab.uiPiecePanel.transform);
                    uiPieceSlot.gameObject.SetActive(false);
                    uiPieceSlot.unitCount = 1;
                    uiPieceSlot.Init(gachaResultList[i], uiPieceSlot.unitCount, gachaResultList[i].UnitBase.Key);
                    uiGachaBannerSlotsCanvasPrefab.uiPieceSlots.Add(uiPieceSlot);
                }
            }
            Core.UnitManager.AddUnitInfo(gachaResultList[i]);
        }
    }

    //뽑기데이터세팅
    public void SetGachaInfoInit(int gachaUnitKey)
    {
        for (int i = 0; i < uiGachaCanvasList.Count; i++)
        {
            UnitInstance unitInstance = GachaUnit(gachaUnitKey);
            gachaResultList.Add(unitInstance);
        }
    }

    //버튼클릭시 오픈 및 닫기
    public void OnClickNextPrefab()
    {
        textToolTip.gameObject.SetActive(false);
        skipBtn.gameObject.SetActive(true);

        //현재 애니메이션이 진행 중이라면, 애니메이션 완전종료상태로 전환
        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            animator.CrossFade(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0f, 0, 1f);
        }
        else
        {
            count++;
            // 현재 프리팹 리스트를 초과하고, 클릭하면 배너 캔버스로 전환
            if (count > dataManager.GachaCount)
            {
                count = dataManager.GachaCount;
                Destroy(curGachaCanvas.gameObject);
                skipBtn.gameObject.SetActive(false);
                uiGachaBannerSlotsCanvasPrefab.Open();
                Core.InputManager.OnClickReceived -= OnClickNextPrefab;
            }
            else
            {
                if (curGachaCanvas != null)
                {
                    Destroy(curGachaCanvas.gameObject);
                    uiGachaCanvasList.Remove(curGachaCanvas);
                    curGachaCanvas = null;
                }

                if (uiGachaCanvasList != null && uiGachaCanvasList.Count > 0)
                {
                    curGachaCanvas = uiGachaCanvasList.First();
                    curGachaCanvas.gameObject.SetActive(true);

                    GachaSound.PlayGachaShowCharacterSR();

                    //애니메이션 재생시간체크
                    animator = curGachaCanvas.GetComponent<Animator>();
                    if (animator != null)
                    {
                        StartCoroutine(WaitForAnimation(animator));
                    }
                }
            }
        }
    }

    private IEnumerator WaitForAnimation(Animator animator)
    {
        // 애니메이션이 끝날 때까지 대기(0~1)
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }
    }

    //랜덤뽑기
    public UnitInstance GachaUnit(int key)
    {
        GachaTable data = dataManager.GachaTable.GetByKey(key);

        List<UnitInfo> gachaUnits = new();

        foreach (int unitsKey in data.Units)
        {
            UnitInfo unit = dataManager.UnitTable.GetByKey(unitsKey);
            gachaUnits.Add(unit);
        }

        //뽑기확률계산
        int randomValue = Random.Range(0, 100);
        ExternalEnums.Grade grade;

        if (randomValue < data.LegendaryRate)
        {
            grade = ExternalEnums.Grade.Legendary;
        }
        else if (randomValue < data.LegendaryRate + data.EpicRate)
        {
            grade = ExternalEnums.Grade.Epic;
        }
        else if (randomValue < data.LegendaryRate + data.EpicRate + data.RareRate)
        {
            grade = ExternalEnums.Grade.Rare;
        }
        else
        {
            grade = ExternalEnums.Grade.Common;
        }

        //해당 등급의 유닛 필터링
        List<UnitInfo> gradeUnits = gachaUnits.FindAll(unit => unit.Grade == grade);
        UnitInfo seletedUnit = gradeUnits[Random.Range(0, gradeUnits.Count)];

        UnitInstance unitInstance = new UnitInstance(seletedUnit, this);
        return unitInstance;
    }

    void OnClickSkipButton()
    {
        if (curGachaCanvas != null)
        {
            curGachaCanvas.gameObject.SetActive(false);
        }

        skipBtn.gameObject.SetActive(false);
        uiGachaCanvasList.Clear();

        uiGachaBannerSlotsCanvasPrefab.Open();
        //Core.SceneLoadManager.LoadScene("LobbyScene");
    }

    private void OnDestroy()
    {
        Core.InputManager.OnClickReceived -= OnClickNextPrefab;
    }
}