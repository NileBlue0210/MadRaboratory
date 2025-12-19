using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRPlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    private InputAction jumpAction;
    private InputAction runAction;

    [Header("Player Informations")]
    private XRPlayer xrPlayer;
    private float defaultSpeed = 0f;
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private float jumpForce = 7f;
    private float velocityY = 0f;
    private float jumpCount = 0f;   // 현재 점프 횟수
    [SerializeField] private int maxJumpCount = 2;   // 최대 점프 횟수

    [Header("Dash Variables")]
    public bool IsRun = false;
    public bool DashBlocked = false;

    [Header("Player Control Events")]
    private Coroutine footstepCoroutine;
    private bool isPuzzleActive = false;

    void Awake()
    {
        xrPlayer = GetComponent<XRPlayer>();

        if (xrPlayer.ActionAsset != null)
        {
            InputActionMap leftHandMap = xrPlayer.ActionAsset.FindActionMap("XRI LeftHand Interaction");
            InputActionMap rightHandMap = xrPlayer.ActionAsset.FindActionMap("XRI RightHand Interaction");

            runAction = leftHandMap.FindAction("Run");
            jumpAction = rightHandMap.FindAction("Jump");
        }
        else
        {
            Debug.LogError("Action Asset is not assigned in XRPlayer.");
        }

        if (xrPlayer.MoveProvider != null)
        {
            defaultSpeed = xrPlayer.MoveProvider.moveSpeed;  // 기본 이동속도 저장
        }
        else 
        {
            Debug.LogError("Move Provider is not assigned in XRPlayer.");
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (xrPlayer.CharacterController.isGrounded && velocityY < 0)
        {
            velocityY = -1f;    // 지면에 닿아 있을 때 약간의 음수 값을 줘서 중력 누적 방지
            jumpCount = 0; // 지면에 닿아 있을 때 점프 횟수 초기화

            // 발자국 소리 처리
            // HandleFootstepSound();
        }

        // 중력 적용
        SetGravity();
    }

    private void OnEnable()
    {
        // 점프 액션 키 매핑
        jumpAction.started += OnJump;
        jumpAction.Enable();

        // 달리기 액션 키 매핑
        runAction.started += OnRun;
        runAction.canceled += StopRun;
        runAction.Enable();
    }

    private void OnDisable()
    {
        // 점프 액션 키 매핑 해제
        jumpAction.started -= OnJump;
        jumpAction.Disable();

        // 달리기 액션 키 매핑 해제
        runAction.started -= OnRun;
        runAction.canceled -= StopRun;
    }

    private void SetGravity()
    {
        velocityY += Physics.gravity.y * Time.deltaTime;

        Vector3 move = new Vector3(0, velocityY, 0);
        xrPlayer.CharacterController.Move(move * Time.deltaTime);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (jumpCount >= maxJumpCount)
            return;

        if (jumpCount == 0)
        {
            velocityY = jumpForce;
        }
        else if (jumpCount == 1)
        {
            velocityY = jumpForce * 1.7f; // 2단 점프 시 첫 점프보다 약간 낮은 힘으로 점프
        }

        jumpCount++;
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        if (DashBlocked)
            return;

        xrPlayer.MoveProvider.moveSpeed += runSpeed;
        IsRun = true;
    }

    private void StopRun(InputAction.CallbackContext context)
    {
        xrPlayer.MoveProvider.moveSpeed = defaultSpeed;
        IsRun = false;
    }

    public IEnumerator DashCooldown()
    {
        // 대시 상태 종료 및 대시 불가능 상태 설정
        DashBlocked = true;
        IsRun = false;
        xrPlayer.MoveProvider.moveSpeed = defaultSpeed;

        // 대시 쿨타임
        yield return new WaitForSeconds(5f);

        DashBlocked = false;
    }

    /// <summary>
    /// 퍼즐 씬 로드 시 플레이어 컨트롤 비활성화
    /// </summary>
    /// <param name="active"></param>
    public void SetPuzzleActive(bool active)
    {
        isPuzzleActive = active;
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;

        // 플레이어 움직임 비활성화
        if (xrPlayer.MoveProvider != null)
        {
            xrPlayer.MoveProvider.enabled = active;
        }
        
        ActionBasedContinuousTurnProvider turnProvider = xrPlayer.GetComponent<ActionBasedContinuousTurnProvider>();

        // 플레이어 회전 비활성화
        if (turnProvider != null)
        {
            turnProvider.enabled = active;
        }

        // 플레이어 조작 비활성화
        this.enabled = active;
    }

    // private void HandleFootstepSound()
    // {
    //     bool isGrounded = xrPlayer.CharacterController.isGrounded;
    //     bool isMoving = xrPlayer.MoveProvider.inputVector.magnitude > 0.1f;

    //     if (xrPlayer.CharacterController.isGrounded && isMoving && footstepCoroutine == null)
    //     {
    //         footstepCoroutine = StartCoroutine(PlayFootstepsRepeatedly());
    //     }

    //     if (!isGrounded || !isMoving)
    //     {
    //         if (footstepCoroutine != null)
    //         {
    //             StopCoroutine(footstepCoroutine);
    //             footstepCoroutine = null;
    //         }
    //     }
    // }

    // private IEnumerator PlayFootstepsRepeatedly()
    // {
    //     while (true)
    //     {
    //         SoundManager.Instance?.PlayPlayerFootstep(IsRun);

    //         yield return new WaitForSeconds(IsRun ? 0.3f : 0.5f);
    //     }
    // }
}
