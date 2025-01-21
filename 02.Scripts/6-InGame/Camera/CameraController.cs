using System.Linq;
using System.Numerics;
using Cinemachine;
using EnumTypes;
using UnityEngine;
using UnityEngine.Animations;
using Quaternion = System.Numerics.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour
{
    // [SerializeField] Transform cam;
    [Header("Cinemachine")] [SerializeField]
    private CinemachineVirtualCamera quaterViewCamera;

    [SerializeField] private CinemachineVirtualCamera followCamera;
    [SerializeField] private CinemachineVirtualCamera attackCamera;
    [SerializeField] private CinemachineVirtualCamera passiveCamera;
    [SerializeField] Transform followTarget;
    [SerializeField] Transform attackTarget;
    private Transform targetUnit;
    private Transform attackTargetUnit;
    private bool isFollowing;

    [Header("Move")] 
    [SerializeField] float moveSpeed = 5f;
    Vector2 moveInput;
    [Header("Rotate")] 
    [SerializeField] float rotateSpeed = 5f;

    private CinemachineTransposer attackCamBody;
    private CinemachineComposer attackCamAim;

    float rotate;

    private void Start()
    {
        //Core.InputManager.OnMoveReceived += OnMovePressed;
        //Core.InputManager.OnRotateReceived += OnRotatePressed;
        //Core.InputManager.OnRotateEndReceived += OnRotateCanceled;
//
        //GameManager.Instance.CommandController.OnPreProcessed += BindTarget;
        //GameManager.Instance.CommandController.OnPostProcessed += ReleaseTarget;

        //Core.EventManager.Subscribe<ExecuteAttackCommandEvent>(SkillBindTarget);
        //Core.EventManager.Subscribe<ExecutePassiveAttackCommandEvent>(PassiveSkillBindTarget);

        //attackCamBody = attackCamera.GetCinemachineComponent<CinemachineTransposer>();
        //attackCamAim = attackCamera.GetCinemachineComponent<CinemachineComposer>();

        // 카메라 초기 위치 적용
        //transform.position = Core.DataManager.SelectedStage.cameraPosition;
    }

    private void OnDisable()
    {
        //Core.InputManager.OnMoveReceived -= OnMovePressed;
        //Core.InputManager.OnRotateReceived -= OnRotatePressed;
        //Core.InputManager.OnRotateEndReceived -= OnRotateCanceled;
//
        //GameManager.Instance.CommandController.OnPreProcessed -= BindTarget;
        //GameManager.Instance.CommandController.OnPostProcessed -= ReleaseTarget;
    }

    private void LateUpdate()
    {
        if (isFollowing)
        { 
            FollowTarget();
            return;
        }

        MoveCamera();
        RotateCamera();
    }

    void OnMovePressed(Vector2 direction)
    {
        moveInput = direction;
    }

    void OnRotatePressed(int direction)
    {
        rotate = -direction;
    }

    void OnRotateCanceled()
    {
        rotate = 0f;
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

    public void BindTarget(GamePhase phase)
    {
        if (phase != GamePhase.PlayerTurn)
            return;

        Unit newtarget = GameManager.Instance.Interaction.curSelected;

        if (newtarget == null) return;

        isFollowing = true;
        targetUnit = newtarget.Requirement.cameraPoint;
        followCamera.gameObject.SetActive(true);
    }

    //public void SkillBindTarget(ExecuteAttackCommandEvent commandEvent)
    //{
    //    switch (commandEvent.IsDone)
    //    {
    //        case true:
    //            attackCamera.gameObject.SetActive(false);
    //            attackCamBody.m_FollowOffset = new Vector3(0, 0, 0);
    //            attackCamAim.m_TrackedObjectOffset = new Vector3(0, 0, 0);
    //            
    //            break;
    //        case false:
    //            if (commandEvent.PlayerUnit.type == UnitType.EnemyUnit) return;
//
    //            targetUnit = commandEvent.PlayerUnit.Requirement.cameraPoint;
    //            followTarget.position = targetUnit.position;
//
    //            if (commandEvent.TargetUnits.First().type == UnitType.PlayableUnit) //아군유닛일 경우
    //            {
    //                attackTargetUnit = commandEvent.TargetUnits.First().Requirement.cameraPoint;
    //                
    //                Vector3 midPoint = (targetUnit.position + attackTargetUnit.position) / 2f;
    //                attackTarget.position = midPoint;
    //               
    //                attackCamera.Follow = quaterViewCamera.transform;
    //                attackCamera.LookAt = attackTarget; //임의로 중간값을 세팅 _ 오류가 생길 수 도 있는
    //            }
    //            else if(commandEvent.PlayerUnit == commandEvent.TargetUnits.First())// 자기자신일 경우
    //            {
    //                attackCamera.Follow = quaterViewCamera.transform;
    //                attackCamera.LookAt = targetUnit;
    //            }
    //            else //일반공격일경우
    //            {
    //                attackCamBody.m_FollowOffset = new Vector3(-2f, 0.5f, -2f);
    //                attackCamAim.m_TrackedObjectOffset = new Vector3(-1f, 0.5f, 0f);
    //                attackCamera.Follow = targetUnit;
    //                attackCamera.LookAt = targetUnit;
    //            }
//
    //            attackCamera.gameObject.SetActive(true);
    //            break;
    //    }
    //}
//
    //public void PassiveSkillBindTarget(ExecutePassiveAttackCommandEvent commandEvent)
    //{
    //    switch (commandEvent.IsDone)
    //    {
    //        case true:
    //            passiveCamera.gameObject.SetActive(false);
    //            break;
    //        case false:
//
    //            if (commandEvent.PlayerUnit.type == UnitType.EnemyUnit) return;
    //            
    //            passiveCamera.m_Lens.FieldOfView = 60f; // 기본 FOV 설정값 초기화
    //            passiveCamera.transform.position = quaterViewCamera.transform.position; //초기값은 쿼터뷰카메라포지션
    //            
    //            targetUnit = commandEvent.PlayerUnit.Requirement.cameraPoint;
    //            followTarget.position = targetUnit.position;
//
    //            attackTargetUnit = commandEvent.PassiveTargetUnit.Requirement.cameraPoint;
    //            attackTarget.position = attackTargetUnit.position;
    //            
    //            //TODO : 패시브 2명 동시 발생시, target유닛 설정 필요
    //            if (targetUnit == null || attackTargetUnit == null) return;
//
    //            if (commandEvent.PlayerUnit == commandEvent.PassiveTargetUnit) //자가 힐 일 경우
    //            {
    //                Vector3 direction = (targetUnit.position - passiveCamera.transform.position).normalized;
    //                passiveCamera.transform.forward = direction;
    //                passiveCamera.m_Lens.FieldOfView = 30f;
    //            }
    //            else
    //            {
    //                Vector3 midPoint = (targetUnit.position + attackTargetUnit.position) / 2f;
    //                passiveCamera.transform.LookAt(midPoint);
    //                passiveCamera.m_Lens.FieldOfView = 50f;
    //            }
//
    //            passiveCamera.gameObject.SetActive(true);
    //            break;
    //    }
    //}

    public void ReleaseTarget(GamePhase phase)
    {
        if (phase != GamePhase.PlayerTurn)
            return;

        isFollowing = false;
        followCamera.gameObject.SetActive(false);
    }

    void FollowTarget()
    {
        if (targetUnit == null) return;

        followTarget.position = targetUnit.position;
    }
}