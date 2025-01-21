using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class AnimationData
{
    // [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string runParameterName = "Run";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string dieParameterName = "Die";
    [SerializeField] private string isDieParameterName = "IsDie";
    // [SerializeField] private string turnL90ParameterName = "TurnL90";
    // [SerializeField] private string turnR90ParameterName = "TurnR90";
    // [SerializeField] private string turnL180ParameterName = "TurnL180";
    // [SerializeField] private string turnR180ParameterName = "TurnR180";
    //90도돌기
    //180도돌기
    [SerializeField] private string coverParameterName = "Cover";
    [SerializeField] private string coverTypeParameterName = "CoverType";
    //엄폐
    //엄폐중 방향전환

    //파쿠르
    public string moveOnCoverParameterName = "MoveOnCover";


    [SerializeField] private string attackParameterName = "Attack";
    //공격, 스킬, 스킬2, 스킬3
    [SerializeField] private string attackTypeParameterName = "AttackType";

    //맞았을 때
    [SerializeField] private string hitParameterName = "Hit";


    // public int GroundHash { get; private set; }
    public int RunHash { get; private set; }
    public int IdleHash { get; private set; }
    public int DieHash { get; private set; }
    public int IsDieHash { get; private set; }
    // public int TurnL90Hash { get; private set; }
    // public int TurnL180Hash { get; private set; }
    // public int TurnR90Hash { get; private set; }
    // public int TurnR180Hash { get; private set; }
    public int CoverHash { get; private set; }
    public int CoverTypeHash { get; private set; }
    public int MoveOnCoverHash { get; private set; }
    public int AttackHash { get; private set; }
    public int AttackTypeHash { get; private set; }
    public int HitHash { get; private set; }

    public AnimationData()
    {
        // GroundHash = Animator.StringToHash(groundParameterName);
        RunHash = Animator.StringToHash(runParameterName);
        IdleHash = Animator.StringToHash(idleParameterName);

        DieHash = Animator.StringToHash(dieParameterName);
        IsDieHash = Animator.StringToHash(isDieParameterName);

        // TurnL90Hash = Animator.StringToHash(turnL90ParameterName);
        // TurnL180Hash = Animator.StringToHash(turnL180ParameterName);
        // TurnR90Hash = Animator.StringToHash(turnR90ParameterName);
        // TurnR180Hash = Animator.StringToHash(turnR180ParameterName);

        CoverHash = Animator.StringToHash(coverParameterName);
        CoverTypeHash = Animator.StringToHash(coverTypeParameterName);

        MoveOnCoverHash = Animator.StringToHash(moveOnCoverParameterName);

        AttackHash = Animator.StringToHash(attackParameterName);
        AttackTypeHash = Animator.StringToHash(attackTypeParameterName);

        HitHash = Animator.StringToHash(hitParameterName);

    }


    //캐릭터 죽을때 시체 사라지는 타임 체크
    public float DieAnimationLength { get; private set; }

    public void SetDieAnimationLength(float length)
    {
        DieAnimationLength = length;
    }
}