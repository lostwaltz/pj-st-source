using EnumTypes;
using UnityEngine;

public class EnemyUnit : Unit
{
    private EnemyType enemyType;
    public EnemyType EnemyType => enemyType;



    public override void Init(int unitIndex, Vector2 initCoord, Vector3 initRotate, UnitInstance instance)
    {
        type = UnitType.EnemyUnit;
        base.Init(unitIndex, initCoord, initRotate, instance);

        if (instance.UnitBase.Key >= 100900)
        {
            BossColliderSize();
        }
    }

    public void InitEnemy(int unitIndex, Vector2 initCoord, Vector3 initRotate, UnitInstance instance, EnemyType enemyType)
    {
        this.enemyType = enemyType;
        Init(unitIndex, initCoord, initRotate, instance);
    }

    public override string GetInfo()
    {
        return base.GetInfo();
    }

    private void BossColliderSize()
    {
        if (TryGetComponent<CapsuleCollider>(out var capsuleCollider))
        {
            capsuleCollider.height = 8f;
            capsuleCollider.radius = 2f;
            capsuleCollider.center = new Vector3(0f, 3.5f, 0.5f);
        }
    }

}