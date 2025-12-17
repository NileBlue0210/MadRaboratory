using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum HazardType
{
    SLOWING_LIQUID,
    POISON_GAS_AREA,
    POISON_POOL
}


public class EnvironmentalHazard : MonoBehaviour
{
    public HazardType type;
    private float oriSpeed;
    private float effectValue;

    private Coroutine poisoning;
    private Coroutine deepPoisoning;

    private void Start()
    {
        oriSpeed = PlayerManager.Instance.Player.MoveProvider.moveSpeed;
    }


    public void ApplyEffect(XRPlayer player)
    {
        if (type == HazardType.SLOWING_LIQUID)
        {
            effectValue = 2.0f;
            player.MoveProvider.moveSpeed /= effectValue;
        }
        else if (type == HazardType.POISON_GAS_AREA)
        {
            if(poisoning != null)
            {
                StopCoroutine(poisoning);
            }

            effectValue = 1.0f;
            poisoning = StartCoroutine(Poisoning((int)effectValue));
        }
        else if (type == HazardType.POISON_POOL)
        {
            if (deepPoisoning != null)
            {
                StopCoroutine(deepPoisoning);
            }

            effectValue = 2.0f;
            deepPoisoning = StartCoroutine(Poisoning((int)effectValue));
        }
    }

    public void RemoveEffect(XRPlayer player)
    {
        if (type == HazardType.SLOWING_LIQUID)
        {
            player.MoveProvider.moveSpeed = oriSpeed;
        }
        else if (type == HazardType.POISON_GAS_AREA)
        {
            StopCoroutine(poisoning);
        }
        else if (type == HazardType.POISON_POOL)
        {
            StopCoroutine(deepPoisoning);
        }
    }

    public IEnumerator Poisoning(int damage)
    {
        while(true)
        {
            PlayerManager.Instance.Player.Condition.TakePhysicalDamage(damage);
            yield return new WaitForSeconds(2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<XRPlayer>(); 
        if (player != null)
        {
            ApplyEffect(player);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<XRPlayer>(); 
        if (player != null)
        {
            RemoveEffect(player);
        }
    }

}
