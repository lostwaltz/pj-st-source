using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using EnumTypes;
using UnityEngine;

public class CameraSystem : Singleton<CameraSystem>
{
    public static CameraEventHandler EventHandler;

    [Header("Cinemachine")] 
    [SerializeField] private CinemachineVirtualCamera quaterViewCamera;
    [SerializeField] private CinemachineVirtualCamera followCamera;
    [SerializeField] private CinemachineVirtualCamera attackCamera;

    [SerializeField] Transform followTarget;
    private Transform targetUnit;
    private Transform attackTargetUnit;
    private CinemachineVirtualCamera curCamera;

    [Header("Move")] [SerializeField] float moveSpeed = 5f;
    Vector2 moveInput;

    [Header("Rotate")] [SerializeField] float rotateSpeed = 5f;
    float rotate;

    private bool isFollowing;

    [Header("Camera Animation")] 
    [SerializeField] private AnimationCurve chasingAnim;
    [SerializeField] float chasingSpeed = 1f;
    Coroutine followRoutine;

    protected override void Awake()
    {
        base.Awake();
        EventHandler = new CameraEventHandler();
        EventHandler.Subscribe(CameraEventTrigger.OnPlayerTurn, (context) => NextUnit(context.Subject));
        EventHandler.Subscribe(CameraEventTrigger.OnPlayerClicked, (context) => SelectUnit(context.Subject));

        EventHandler.Subscribe(CameraEventTrigger.OnUnitMove, NormalMove);
        EventHandler.Subscribe(CameraEventTrigger.OnUnitActivateSkill, SkillAttack);
        EventHandler.Subscribe(CameraEventTrigger.OnUnitActivatePassive, PassiveActivate);

        Core.InputManager.OnMoveReceived += OnMovePressed;
        Core.InputManager.OnRotateReceived += OnRotatePressed;
        Core.InputManager.OnRotateEndReceived += OnRotateCanceled;

        GameManager gameManager = GameManager.Instance;
        gameManager.SubscribePhaseEvent(GamePhase.EnemyTurn, ClearCamera); //페이즈 바뀔때, 모든카메라를 꺼주기 위함.
        gameManager.SubscribePhaseEvent(GamePhase.End, ClearCamera);

        UpdateCamera(quaterViewCamera);
        // 카메라 초기 위치 적용
        transform.position = Core.DataManager.SelectedStage.cameraPosition;
        transform.eulerAngles = Core.DataManager.SelectedStage.cameraRotation;
    }

    private void LateUpdate()
    {
        if (isFollowing)
        {
            FollowTarget();
            return;
        }

        RotateCamera();
        MoveCamera();
    }

    IEnumerator FollowTargetRoutine()
    {
        while (isFollowing)
        {
            Vector3 pos = targetUnit.position;
            pos.y = 0f;
            transform.position = pos;
            yield return null;
        }

        followRoutine = null;
    }

    void ChaseTarget(Vector3 destination)
    {
        destination.y = 0f;

        if (followRoutine != null)
        {
            StopCoroutine(followRoutine);
            followRoutine = null;
        }

        followRoutine = StartCoroutine(ChaseTargetRoutine(destination));
    }

    IEnumerator ChaseTargetRoutine(Vector3 destination)
    {
        float progress = 0f;
        Vector3 start = transform.position;

        while (progress < 1f)
        {
            transform.position = Vector3.Lerp(start, destination, chasingAnim.Evaluate(progress));

            yield return null;
            progress += Time.deltaTime * chasingSpeed;
        }

        transform.position = Vector3.Lerp(start, destination, 1f);
        followRoutine = null;
    }

    void OnMovePressed(Vector2 direction)
    {
        moveInput = direction;
    }

    void OnRotatePressed(int direction)
    {
        rotate = -direction;
    }

    void MoveCamera()
    {
        Vector3 dirOnCam = quaterViewCamera.transform.forward * moveInput.y +
                           quaterViewCamera.transform.right * moveInput.x;
        dirOnCam.y = 0f;
        Vector3 speed = moveSpeed * Time.deltaTime * dirOnCam;
        transform.position += speed;
    }

    void RotateCamera()
    {
        transform.eulerAngles += rotate * rotateSpeed * Time.deltaTime * Vector3.up;
    }

    void OnRotateCanceled()
    {
        rotate = 0f;
    }

    /// <summary>
    /// 쿼터뷰 - 자기자신을 바라보는 카메라(완)
    /// </summary>
    private void NextUnit(Unit target)
    {
        ClearCamera();
        targetUnit = target.Requirement.cameraPoint;

        isFollowing = false;
        ChaseTarget(targetUnit.position);
    }

    /// <summary>
    /// 쿼터뷰 - 선택한 유닛을 줌인해서 바라보는 카메라 / 적 유닛 선택시에만,
    /// </summary>
    public void SelectUnit(Unit target)
    {
        //TODO:플레이어가 유닛을 클릭하고, 취소버튼 또는 다른것을 클릭한다면? 모두꺼줘야함.

        if (target.type == UnitType.PlayableUnit)
        {
            ClearCamera();
        }
        else if (target.type == UnitType.EnemyUnit)
        {
            NextUnit(target);
        }
    }

    /// <summary>
    /// 쿼터뷰 - 자신유닛을 줌인하는 카메라 + Enemy 유닛도 사용가능
    /// </summary>
    private void PassiveActivate(CameraEventContext context)
    {
        var command = context.Command as PassiveSkill;

        UpdateCamera(quaterViewCamera);

        if (context.Command is SupportFire supportFire)
        {
            command = context.Command as SupportFire;
            targetUnit = command.GetUnit().Requirement.cameraPoint;

            if (supportFire.GetAttackTarget() == null) return;
            attackTargetUnit = supportFire.GetAttackTarget().Requirement.cameraPoint;

            if (targetUnit == null || attackTargetUnit == null) return;

            Vector3 midPoint = (targetUnit.position + attackTargetUnit.position) / 2f;
            ChaseTarget(midPoint);
        }
        else
        {
            isFollowing = true;
            targetUnit = command.GetUnit().Requirement.cameraPoint;
            followTarget.position = targetUnit.position;

            ChaseTarget(targetUnit.position);
        }
    }

    /// <summary>
    /// 숄더뷰 - 일반 공격 스킬 카메라(완)
    /// </summary>
    private void NormalMove(CameraEventContext context)
    {
        var command = context.Command as MoveCommand;

        // ChangedCamera();

        isFollowing = true;
        targetUnit = command.GetUnit().Requirement.cameraPoint;

        if (command.GetUnit().type == UnitType.EnemyUnit)
        {
            // 쿼터뷰 세컨드 - 팔로우
            // Debug.Log("에너미일 때 따라가기");
            if (followRoutine != null)
            {
                StopCoroutine(FollowTargetRoutine());
                followRoutine = null;
            }

            followRoutine = StartCoroutine(FollowTargetRoutine());
        }
        else
        {
            UpdateCamera(followCamera);
        }
    }
    
    /// <summary>
    /// 숄더뷰 - 일반공격스킬(AttackCam)
    /// 쿼터뷰 - 아군유닛 -> 나자신인경우 = targetPosition ? 여러 유닛일 경우 : midPoint(Ex.크세니아)
    /// </summary>
    private void SkillAttack(CameraEventContext context)
    {
        var command = context.Command as AttackCommand;
        if (command == null) 
            return;
        
        targetUnit = command.GetUnit().Requirement.cameraPoint;
        var targets= command.GetAttackTarget(); 
        attackTargetUnit = targets.Count > 0 ? command.GetAttackTarget().First().Requirement.cameraPoint : targetUnit;
        isFollowing = true;

        if (command.GetUnit().type == UnitType.EnemyUnit)
        {
            if (followRoutine != null)
            {
                StopCoroutine(followRoutine);
                followRoutine = null;
            }

            //attackTargetUnit = command.GetAttackTarget().First().Requirement.cameraPoint;

            Vector3 midPoint = (targetUnit.position + attackTargetUnit.position) / 2f;
            ChaseTarget(midPoint);

            return;
        }
        var targetType = command.GetSkill().Data.SkillBase.TargetType;
        if ((UnitType)targetType == command.GetUnit().type && command.GetUnit() == attackTargetUnit)
        {
            transform.position = targetUnit.position;
            UpdateCamera(quaterViewCamera);
        }
        else if ((UnitType)targetType == command.GetUnit().type && command.GetUnit() != attackTargetUnit)
        {
            List<Unit> attackTargets = command.GetAttackTarget();
            Vector3 averagePosition = Vector3.zero;
            foreach (Unit target in attackTargets)
            {
                averagePosition += target.Requirement.cameraPoint.position;
            }
            //AttackTargets의 전체 포지션을 더해서 1/n 을 해줘야함
            averagePosition /= attackTargets.Count;

            Vector3 midPoint = (targetUnit.position + averagePosition)/2;
            ChaseTarget(midPoint);
            UpdateCamera(quaterViewCamera);
        }
        else
        {
            attackCamera.Follow = targetUnit;
            attackCamera.LookAt = targetUnit;
            UpdateCamera(attackCamera);
        }
    }
    void FollowTarget()
    {
        if (targetUnit == null) return;

        followTarget.position = targetUnit.position;
    }

    void ClearCamera()
    {
        attackCamera.gameObject.SetActive(false);
        followCamera.gameObject.SetActive(false);
        quaterViewCamera.gameObject.SetActive(true);

        UpdateCamera(quaterViewCamera);

        isFollowing = false;
    }

    void UpdateCamera(CinemachineVirtualCamera newCam)
    {
        if (newCam == curCamera) return;

        newCam.gameObject.SetActive(true);
        curCamera?.gameObject.SetActive(false);
        curCamera = newCam;
    }

}