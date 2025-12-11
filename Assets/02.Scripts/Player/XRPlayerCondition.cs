using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRPlayerCondition : MonoBehaviour
{
    [Header("Player Status")]
    public float health;
    public float Stamina;
    public float staminaDecreasePerSec = 20f;
    public float staminaRegenPerSec = 10f;

    [Header("UI Elements")]
    public Condition healthBar;
    public Condition staminaBar;
    [SerializeField] private GameObject DieUI;
    private bool isDead = false;


    public event Action OnTakeDamage;

    void Start()
    {
        if (DieUI != null)
            DieUI.SetActive(false);

        if (healthBar == null || staminaBar == null)
        {
            Debug.LogError("HealthBar or StaminaBar is not assigned in XRPlayerCondition.");
        }
        else
        {
            healthBar.maxValue = 100;
            healthBar.curValue = health;

            staminaBar.maxValue = 100;
            staminaBar.curValue = Stamina;
        }
    }

    void Update()
    {
        // 체력, 스테미너 게이지 업데이트
        healthBar.curValue = health;
        staminaBar.curValue = Stamina;

        // 스테미너 관리
        StaminaAmountOfChange();
    }

    public void TakePhysicalDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);
        OnTakeDamage?.Invoke();
    }

    private void StaminaAmountOfChange()
    {
        if (PlayerManager.Instance.Player.Controller != null)
        {
            if (Stamina <= 0)
            {
                Coroutine dashCoroutine = StartCoroutine(PlayerManager.Instance.Player.Controller.DashCooldown());
            }

            if (PlayerManager.Instance.Player.Controller.IsRun)
            {
                Stamina -= staminaDecreasePerSec * Time.deltaTime;
            }
            else
            {
                Stamina += staminaRegenPerSec * Time.deltaTime;
            }

            Stamina = Mathf.Clamp(Stamina, 0, 100);
        }
    }
}
