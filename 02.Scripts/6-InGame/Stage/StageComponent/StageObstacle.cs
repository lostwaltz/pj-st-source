using System;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleCoverType
{
    HighCover = 0,  // HC
    LowCover        // LC
}

public class StageObstacle : StageComponent, IDamagable
{
    public ObstacleSO obstacleData;
    public ObstacleCoverType type;
    public Transform[] coverPoints;
    [Tooltip("기준점 : 좌측 하단")]
    public Vector2Int size;
    
    [Header("Durability")]
    [SerializeField] private int durability;
    public bool IsDead { get; private set; }
    public event Action OnDestruct; 
    
    public override void Initialize(Placement newPlacement)
    {
        base.Initialize(newPlacement);
        gameObject.SetActive(true);
    }

    public void Initialize(Placement newPlacement, List<Vector2> occupied)
    {
        Initialize(newPlacement);

        for (int i = 0; i < occupied.Count; i++)
        {
            // 해당 cell에 비용 추가
            if (StageManager.Instance.cellMaps.TryGetValue(occupied[i], out var cell))
                cell.AdjustObstacleCost(placement.cost);    
        }
        
    }
    
    public void TakeDamage(int damage)
    {
        if(IsDead || durability < 0)
            return;
        
        durability = Mathf.Max(durability - damage, 0);
        if (durability <= 0)
        {
            IsDead = true;
            OnDestruct?.Invoke();
        }
    }
}
