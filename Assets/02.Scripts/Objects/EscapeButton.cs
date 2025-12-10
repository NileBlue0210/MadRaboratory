using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EscapeButton : MonoBehaviour
{
    [SerializeField] private XRSimpleInteractable interactable; // 탈출 버튼 상호작용 컴포넌트
    public Action OnPressed; // 버튼이 눌렸을 때 호출되는 델리게이트 이벤트

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(OnSelectEntered);

        if (interactable == null)
            Debug.LogError("EscapeButton: Interactable component is not assigned.");
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        PressButton();
    }

    private void PressButton()
    {
        OnPressed?.Invoke();
    }
}
