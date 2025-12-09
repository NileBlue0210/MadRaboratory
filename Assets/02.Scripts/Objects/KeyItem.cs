using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyItem : InteractableObject
{
    [SerializeField] private XRGrabInteractable grabInteractable;

    protected override void Awake()
    {
        grabInteractable.selectEntered.AddListener(x => OnGrabbed());
        grabInteractable.selectExited.AddListener(x => OnReleased());

    }

    protected override void Start()
    {
        // 매치 아이템에 맞게 색상 변경
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = ChangeColor(objectData.objectColor);
    }

    protected override void Update()
    {
        
    }

    private void OnGrabbed()
    {
        Debug.Log("열쇠 아이템을 잡았습니다.");
    }

    private void OnReleased()
    {
        Debug.Log("열쇠 아이템을 놓았습니다.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        BeaconObject beacon = collision.gameObject.GetComponent<BeaconObject>();

        if (beacon != null)
        {
            beacon.OpenDoor();
        }
    }
}
