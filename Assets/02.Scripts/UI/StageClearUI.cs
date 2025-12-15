using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageClearUI : MonoBehaviour
{
    [SerializeField] private Button quitGameButton;
    [SerializeField] private TextMeshProUGUI stageClearText;

    private void Start()
    {
        // 각 컴포넌트 유효성 검사
        if (quitGameButton == null || stageClearText == null)
        {
            Debug.LogError("StageClearUI: Missing UI Components in StageClearUI.");

            return;
        }

        // UI가 활성화 될 때 하위 컴포넌트들이 비활성화 상태라면 활성화
        if (quitGameButton != null && quitGameButton.gameObject.activeSelf == false)
        {
            quitGameButton.gameObject.SetActive(true);
        }

        if (stageClearText != null && stageClearText.gameObject.activeSelf == false)
        {
            stageClearText.gameObject.SetActive(true);
        }
    }
}
