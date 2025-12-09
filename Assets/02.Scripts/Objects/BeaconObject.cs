using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeaconObject : InteractableObject
{
    [SerializeField] private Animator doorAnimator;

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        // 매치 아이템에 맞게 색상 변경
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = ChangeColor(objectData.objectColor);
    }

    public void OpenDoor()
    {
        doorAnimator.SetBool("Open", true);
    }

    public void CloseDoor()
    {
        doorAnimator.SetBool("Open", false);
    }

    private void OnCollisionExit(Collision collision)
    {
        KeyItem keyItem = collision.gameObject.GetComponent<KeyItem>();

        if (keyItem != null)
        {
            CloseDoor();
        }
    }
}
