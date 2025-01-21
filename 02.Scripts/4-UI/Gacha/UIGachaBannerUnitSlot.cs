using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGachaBannerUnitSlot : UIBase, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image unitImage;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private UIGachaInfoCanvas uiGachaInfoCanvas;
    [SerializeField] private Button uiGachaInfoCanvasOpenBtn;
    [SerializeField] private Image outLine;
    private Image gachaBannerSlotBG;
    private UnitInstance currentUnit;
    public void OnEnable()
    {
        uiGachaInfoCanvasOpenBtn.onClick.AddListener(OnClickSlot);
    }
    public void Init(UnitInstance unit)
    {
        currentUnit = unit;
        gachaBannerSlotBG = GetComponent<Image>();
        ParticleSystem.MainModule particle = particleSystem.main;

        if (unit.UnitBase.Grade == ExternalEnums.Grade.Legendary)
        {
            gachaBannerSlotBG.color = Color.yellow;
            particle.startColor = Color.yellow;
        }
        else if (unit.UnitBase.Grade == ExternalEnums.Grade.Epic)
        {
            gachaBannerSlotBG.color = new Color(0x71f / 255f, 0x00f / 255f, 0xFFf / 255f);
            particle.startColor = new Color(0x71f / 255f, 0x00f / 255f, 0xFFf / 255f); //'#7100FF'
        }
        else if (unit.UnitBase.Grade == ExternalEnums.Grade.Rare)
        {
            gachaBannerSlotBG.color = Color.cyan;
            particle.startColor = Color.cyan;
        }
        else if (unit.UnitBase.Grade == ExternalEnums.Grade.Common)
        {
            gachaBannerSlotBG.color = Color.white;
            particle.startColor = Color.white;
        }

        unitImage.sprite = Resources.Load<Sprite>(unit.UnitBase.GachaPath);
        uiGachaInfoCanvas = Core.UIManager.GetUI<UIGachaInfoCanvas>();
        uiGachaInfoCanvas.Close();
    }
    public void OnClickSlot()
    {
        uiGachaInfoCanvas.SetDataOpenUI(currentUnit);
        
        UIBattle.PlayUIBattleClickNormalSound();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        outLine.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        outLine.gameObject.SetActive(false);
    }
}
