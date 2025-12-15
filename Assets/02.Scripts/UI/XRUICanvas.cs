using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRUICanvas : MonoBehaviour
{
    [SerializeField] private Transform targetCamera; // 추적 대상 카메라 트랜스폼
    [SerializeField] private float followSpeed = 0.3f; // 따라오는 속도
    [SerializeField] private float distance = 2f; // 카메라로부터의 거리
    private Vector3 currentVelocity = Vector3.zero; // 현재 UI 위치

    void Start()
    {
        
    }

    void Update()
    {
        if (targetCamera == null)
            return;

        LazyFollow();
    }

    /// <summary>
    /// UI가 플레이어 시야를 천천히 따라오도록 함
    /// </summary>
    private void LazyFollow()
    {
        Vector3 targetPosition = targetCamera.position + (targetCamera.forward * distance);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, followSpeed);
        transform.LookAt(transform.position + targetCamera.rotation * Vector3.forward, targetCamera.rotation * Vector3.up);
    }
}
