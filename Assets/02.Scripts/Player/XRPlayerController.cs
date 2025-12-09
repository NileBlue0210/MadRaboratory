using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    void Awake()
    {
        xrPlayer = GetComponent<XRPlayer>();

        if (xrPlayer.ActionAsset != null)
        {
            InputActionMap rightHandMap = xrPlayer.ActionAsset.FindActionMap("XRI RightHand Interaction");
            jumpAction = rightHandMap.FindAction("Jump");
            runAction = rightHandMap.FindAction("Run");
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
        }

        velocityY += Physics.gravity.y * Time.deltaTime;    // 중력 적용

        Vector3 move = new Vector3(0, velocityY, 0);
        xrPlayer.CharacterController.Move(move * Time.deltaTime);
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
        xrPlayer.MoveProvider.moveSpeed += runSpeed;
    }

    private void StopRun(InputAction.CallbackContext context)
    {
        xrPlayer.MoveProvider.moveSpeed = defaultSpeed;
    }
}
