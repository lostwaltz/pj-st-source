using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

public class UIUnitManageSkillInfo : UIBase
{
    [SerializeField] private Button btnClose;

    [Header("Fade 설정")]
    [SerializeField] private CanvasGroup CanvasGroup;
    private float fadeinduration = 0.3f;
    private float fadeoutduration = 0.3f;

    [Header("스킬 상세 정보")]
    [SerializeField] private Image SkillIcon;
    [SerializeField] private TMP_Text SkillName;
    [SerializeField] private TMP_Text SkillType;

    [SerializeField] private TMP_Text StabilityDamage;
    [SerializeField] private TMP_Text Description;
    [SerializeField] private TMP_Text Script;


    [SerializeField] private TMP_Text SkillRange;
    [SerializeField] private TMP_Text SkillScope;



    [Header("그리드 설정")]
    [SerializeField] private GridLayoutGroup GridLayoutGroup;
    [SerializeField] private RectTransform GridLayoutGroupRectTransform;
    private float GridSize = 230f;


    private int MinCellSize = 11;
    private int MaxCellSize = 22;

    [Header("그리드 이미지")]
    [SerializeField] private Image GridCellPrefab;
    [SerializeField] private int MaxPoolSize = 400;
    private Queue<Image> cellPool;
    private List<Image> activeCells;
    [SerializeField] private Sprite CasterSprite; // 시전자 그리드
    [SerializeField] private Sprite RangeSprite;  // 사거리 그리드 
    [SerializeField] private Sprite ScopeSprite; // 적용범위 그리드
    [SerializeField] private Sprite emptySprite;  // 빈 그리드
    [SerializeField] private Sprite TargetSprite; // 타겟 그리드

    private int currentGridSize;
    private float cellSize;
    private Image[,] gridCells;
    private Vector2Int centerPos;
    private bool isInitialized = false;
    private bool isPoolInitialized = false;

    private void Awake()
    {
        CanvasGroup.alpha = 0;
        btnClose.onClick.AddListener(Close);
    }


    private void OnEnable()
    {
        if (!isInitialized)
        {
            InitializeGrid();
        }
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

    public override void Close()
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


    /// <summary>
    /// 스킬 설명 표시
    /// </summary>
    /// <param name="skillIndex"></param>
    /// <param name="skillInfo"></param>
    /// 
    public void ShowSkillInfo(SkillInfo skillInfo)
    {
        InitializePool();

        CalculateGridSize(skillInfo.Range);

        InitializeGrid();

        //ui정보 업데이트
        SkillIcon.sprite = Resources.Load<Sprite>(skillInfo.Path);
        SkillName.text = skillInfo.Name;
        SkillType.text = skillInfo.SkillType.ToString();

        StabilityDamage.text = $"안정도 피해: {skillInfo.StabilityDamage}";
        Description.text = skillInfo.Description;
        // Script.text = skillInfo.TargetDiscription;

        SkillRange.text = $"{skillInfo.Range}";
        SkillScope.text = GetScopeText(skillInfo.Scope);

        LayoutRebuilder.ForceRebuildLayoutImmediate(GridLayoutGroupRectTransform);

        ShowSkillRange(skillInfo.Range, GetEffectArea(skillInfo), skillInfo);

        Open();

    }
    private string GetScopeText(int scope)
    {
        return scope switch
        {
            1 => "목표",
            _ => scope.ToString()
        };
    }




    /// <summary>
    /// 오브젝트 풀 
    /// 
    private void InitializePool()
    {
        if (isPoolInitialized) return;

        cellPool = new Queue<Image>();
        activeCells = new List<Image>();

        // 최대 풀 크기만큼 셀 미리 생성
        for (int i = 0; i < MaxPoolSize; i++)
        {
            var cell = Instantiate(GridCellPrefab, GridLayoutGroup.transform);
            cell.gameObject.SetActive(false);
            cellPool.Enqueue(cell);
        }
        isPoolInitialized = true;
    }
    private Image GetCellFromPool()
    {
        if (cellPool.Count > 0)
        {
            var cell = cellPool.Dequeue();
            cell.gameObject.SetActive(true);
            activeCells.Add(cell);
            return cell;
        }

        var newCell = Instantiate(GridCellPrefab, GridLayoutGroup.transform);
        activeCells.Add(newCell);
        return newCell;
    }

    private void ReturnAllCellsToPool()
    {
        if (activeCells == null || cellPool == null) return;

        foreach (var cell in activeCells)
        {
            if (cell != null)
            {
                cell.transform.SetParent(null);  // 부모 제거
                cell.gameObject.SetActive(false);
                cellPool.Enqueue(cell);
            }
        }
        activeCells.Clear();
    }
    private void OnDestroy()
    {
        // 풀의 모든 오브젝트 제거
        foreach (var cell in cellPool)
        {
            if (cell != null)
            {
                Destroy(cell.gameObject);
            }
        }

        foreach (var cell in activeCells)
        {
            if (cell != null)
            {
                Destroy(cell.gameObject);
            }
        }
    }




    /// <summary>
    /// 스킬 사거리 그리드 표시
    /// </summary>
    /// <param name="range"></param>
    /// <param name="effectArea"></param>
    /// 
    private void CalculateGridSize(int range)
    {
        // 1. 사거리에 따른 그리드 셀 개수 계산 (항상 홀수)
        currentGridSize = (range * 2) + 3;
        currentGridSize = currentGridSize % 2 == 0 ? currentGridSize + 1 : currentGridSize;

        // 2. 2픽셀 간격을 고려한 실제 사용 가능한 공간 계산
        float spacing = 2f;
        float totalSpacing = spacing * (currentGridSize - 1);  // 전체 간격 크기
        float availableSpace = GridSize - totalSpacing;        // 셀들이 실제로 사용할 수 있는 공간

        // 3. 셀 크기 계산 (사용 가능한 공간을 셀 개수로 나눔)
        cellSize = availableSpace / currentGridSize;

        // 4. GridLayoutGroup 설정
        GridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        GridLayoutGroup.spacing = new Vector2(spacing, spacing);
        GridLayoutGroup.constraintCount = currentGridSize;

        // 5. 그리드 크기 고정
        GridLayoutGroupRectTransform.sizeDelta = new Vector2(GridSize, GridSize);

        // 6. 레이아웃 즉시 업데이트
        LayoutRebuilder.ForceRebuildLayoutImmediate(GridLayoutGroupRectTransform);
    }

    private List<Vector2Int> GetEffectArea(SkillInfo skillInfo)
    {
        var effectArea = new List<Vector2Int>();

        switch (skillInfo.Scope)
        {
            case 1: // 단일 대상
                effectArea.Add(new Vector2Int(0, 0));
                break;
            case 2: // 십자 범위
                effectArea.AddRange(new List<Vector2Int> {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1)
            });
                break;
            //TODO: SCOPE > 1 이면 셀 배열 다르게
            case 3: // 3x3 범위 
                for (int x = -1; x <= 1; x++)
                    for (int y = -1; y <= 1; y++)
                        effectArea.Add(new Vector2Int(x, y));
                break;
        }

        return effectArea;
    }

    private void InitializeGrid()
    {
        ReturnAllCellsToPool();

        foreach (Transform child in GridLayoutGroup.transform)
        {
            child.gameObject.SetActive(false);
        }

        // 3. 새로운 그리드 배열 생성
        gridCells = new Image[currentGridSize, currentGridSize];
        centerPos = new Vector2Int(currentGridSize / 2, currentGridSize / 2);

        // 4. 필요한 만큼의 셀을 풀에서 가져와서 그리드 구성
        for (int y = 0; y < currentGridSize; y++)
        {
            for (int x = 0; x < currentGridSize; x++)
            {
                var cell = GetCellFromPool();
                cell.transform.SetParent(GridLayoutGroup.transform, false);  // worldPositionStays를 false로 설정
                cell.transform.localScale = Vector3.one;  // 스케일 초기화
                cell.sprite = emptySprite;
                cell.gameObject.SetActive(true);
                gridCells[x, y] = cell;
            }
        }

        isInitialized = true;
        ResetGrid();
    }

    private void ResetGrid()
    {
        // 스프라이트만 변경하여 초기 상태로
        for (int y = 0; y < currentGridSize; y++)
        {
            for (int x = 0; x < currentGridSize; x++)
            {
                gridCells[x, y].sprite = emptySprite;
            }
        }
        // 시전자 위치 표시
        gridCells[centerPos.x, centerPos.y].sprite = CasterSprite;
    }

    public void ShowSkillRange(int range, List<Vector2Int> effectArea, SkillInfo skillInfo)
    {
        if (!isInitialized)
        {
            InitializeGrid();
            return;
        }

        ResetGrid();

        // 탄착범위가 존재할경우
        if (skillInfo.Scope > 1)
        {
            int startX = centerPos.x - (skillInfo.ScopeWidth / 2);
            int startY = centerPos.y - skillInfo.ScopeHeight;

            for (int y = 0; y < skillInfo.ScopeHeight; y++)
            {
                for (int x = 0; x < skillInfo.ScopeWidth; x++)
                {
                    Vector2Int pos = new Vector2Int(startX + x, startY + y);
                    if (IsValidPosition(pos))
                    {
                        gridCells[pos.x, pos.y].sprite = ScopeSprite;
                    }
                }
            }
        }

        else
        {
            // 단일타겟일 때
            for (int y = -range; y <= range; y++)
            {
                for (int x = -range; x <= range; x++)
                {
                    if (Mathf.Abs(x) + Mathf.Abs(y) <= range)
                    {
                        Vector2Int pos = centerPos + new Vector2Int(x, y);
                        if (IsValidPosition(pos))
                        {
                            gridCells[pos.x, pos.y].sprite = RangeSprite;
                        }
                    }
                }
            }
        }

        // 시전자 위치 재표시
        gridCells[centerPos.x, centerPos.y].sprite = CasterSprite;
    }



    private bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < currentGridSize &&
                pos.y >= 0 && pos.y < currentGridSize;
    }

}
