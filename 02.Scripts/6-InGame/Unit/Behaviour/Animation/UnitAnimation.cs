using System;
using UnityEngine;

public enum UnitAttackType
{
    NormalAttack = 0,
    Skill1,
    Skill2,
    Skill3,
    Passive,
    SpecialSkill,
    SpecialPassive,
}

[Serializable]
public struct UnitAnimationSetting
{
    public float obstacleRecoverTime;
}

[RequireComponent(typeof(Animator))]
public class UnitAnimation : MonoBehaviour
{
    private Animator animator;
    public Animator Animator { get { return animator; } }

    public UnitAnimationSetting setting;
    public AnimationData Data { get; private set; }

    private int currentStateHash = 0;
    private int prevStateHash = 0;
    public bool isOnObstacle = false;


    public void Initialize()
    {
        animator = GetComponent<Animator>();
        Data = new AnimationData();

        // Die 애니메이션 길이 설정 시체 사라지는용도
        var controller = animator.runtimeAnimatorController;
        foreach (var clip in controller.animationClips)
        {
            if (clip.name.Contains("Die"))
            {
                Data.SetDieAnimationLength(clip.length);
                break;
            }
        }

        SetIdle();
    }

    public void SetIdle()
    {
        animator.SetBool(currentStateHash, false);
        currentStateHash = Data.IdleHash;

        animator.SetBool(Data.IdleHash, true);
    }

    public void SetCover(int coverType)
    {
        animator.SetBool(currentStateHash, false);
        currentStateHash = Data.CoverHash;

        animator.SetInteger(Data.CoverTypeHash, coverType);
        animator.SetBool(currentStateHash, true);
    }

    public void SetRun()
    {
        animator.SetBool(currentStateHash, false);
        currentStateHash = Data.RunHash;

        animator.SetBool(Data.RunHash, true);
    }

    public void SetAttack(int type)
    {
        animator.SetBool(currentStateHash, false);
        prevStateHash = currentStateHash;

        animator.SetInteger(Data.AttackTypeHash, type);
        animator.SetTrigger(Data.AttackHash);
    }

    public void ReturnAttack()
    {
        animator.SetBool(prevStateHash, true);
    }

    public void ReactHit()
    {
        animator.SetTrigger(Data.HitHash);
    }

    public void ReactObstacle()
    {
        if (!isOnObstacle)
        {
            animator.SetTrigger(Data.MoveOnCoverHash);
            isOnObstacle = true;

            Invoke("RecoverObstacle", setting.obstacleRecoverTime);
        }
    }

    void RecoverObstacle()
    {
        isOnObstacle = false;
    }

    public void SetDead()
    {
        animator.SetBool(Data.IsDieHash, true);
        animator.SetTrigger(Data.DieHash);
    }

}
