using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    ObjectData GetInteractableInfo();
}

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
    private bool isDead = false;

    [Header("Player Condition Events")]
    private Coroutine lowHpWarningCoroutine;
    private bool isLowHpWarningActive = false;


    public event Action OnTakeDamage;

    void Start()
    {
        if (healthBar == null || staminaBar == null)
        {
            Debug.LogError("HealthBar or StaminaBar is not assigned in XRPlayerCondition.");

            return;
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
        if (!isDead && health <= 0)
        {
            Die("Health has dropped to 0!");

            return;
        }

        // 체력, 스테미너 게이지 업데이트
        healthBar.curValue = health;
        staminaBar.curValue = Stamina;

        if (health <= 30 && !isLowHpWarningActive)
        {
            lowHpWarningCoroutine = StartCoroutine(PlayLowHpWarning());
            isLowHpWarningActive = true;

            SoundManager.Instance?.SetBGMVolume(0.05f);
        }

        // 스테미너 관리
        StaminaAmountOfChange();
    }

    public void TakePhysicalDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);
        OnTakeDamage?.Invoke();
    }

    private IEnumerator PlayLowHpWarning()
    {
        while (true)
        {
            SoundManager.Instance.PlayDamageSound(); // ��� �ݺ������� ����� ���� ���
            yield return new WaitForSeconds(1.5f); // 1.5�� ���� (���ϸ� �� ª��/��� ���� ����)
        }
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

    private void Die(string dieReason)
    {
        if (isDead) return;

        isDead = true;

        GameManager.Instance.GameOver(dieReason);
    }
}
