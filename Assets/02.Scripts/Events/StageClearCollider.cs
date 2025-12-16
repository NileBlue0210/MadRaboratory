using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (UIManager.Instance == null)
            return;

        if (other.CompareTag("Player"))
        {
            UIManager.Instance.ShowStageClearUI();
        }
    }
}
