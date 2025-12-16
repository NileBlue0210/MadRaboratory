using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetryGameButton : MonoBehaviour
{
    [SerializeField] private Button retryGameButton;

    void Start()
    {
        if (retryGameButton != null)
        {
            retryGameButton.onClick.AddListener(RestartGame);
        }
        else
        {
            Debug.LogError("Retry Game Button is not assigned in the inspector.");
        }
    }

    private void RestartGame()
    {
        GameManager.Instance.RetryGame();
    }
}
