// PauseUI.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance;

    [Header("UI Panels")]
    public GameObject pauseMenuPanel;

    [Header("Buttons")]
   
    public Button saveButton;
    public Button loadButton;
   

    public static bool isGamePaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        isGamePaused = false;
        Time.timeScale = 1f;

        if (saveButton != null)
        {
            saveButton.onClick.AddListener(OnSaveButtonClicked);
        }
        if (loadButton != null)
        {
            loadButton.onClick.AddListener(OnLoadButtonClicked);
        }
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.IsGamePaused = true;
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.IsGamePaused = false;
        }
    }

    void OnSaveButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveCurrentGameData();
        }
        else
        {
            Debug.LogError("GameManager.Instance not found. Cannot save game.");
        }
    }

    void OnLoadButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadAndApplyGameData();
        }
        else
        {
            Debug.LogError("GameManager.Instance not found. Cannot load game.");
        }
    }

  
}