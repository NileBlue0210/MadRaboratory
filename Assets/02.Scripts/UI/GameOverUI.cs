using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button retryButton;
    public Button quitButton;

    private void Start()
    {
        gameOverPanel.SetActive(false);

        retryButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(ReturnToMenu);
    }

    public void ShowGameOver(string reason)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = $"게임 오버!\n({reason})";

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LobbyScene"); //���⿡ �κ� �� �̸� ��Ȯ�� �Է�
    }
}
