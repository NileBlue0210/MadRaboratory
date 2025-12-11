using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;


public class XRPlayer : MonoBehaviour
{
    [Header("Components")]
    public CharacterController CharacterController;
    public InputActionAsset ActionAsset;
    public DynamicMoveProvider MoveProvider;
    [HideInInspector] public XRPlayerController Controller;// 플레이어의 이동 및 카메라 회전을 담당하는 PlayerController 클래스의 인스턴스
    [HideInInspector] public XRPlayerCondition Condition; // 플레이어의 상태를 나타내는 Condition 클래스의

    private void Awake()
    {
        PlayerManager.Instance.Player = this;

        Controller = GetComponent<XRPlayerController>();// PlayerController 컴포넌트를 가져와서 controller 변수에 할당
        Condition = GetComponent<XRPlayerCondition>();// PlayerCondition 컴포넌트를 가져와서 condition 변수에 할당
    }
}
