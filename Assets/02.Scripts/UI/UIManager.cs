using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전체 UI를 관리하는 매니저 클래스
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Singleton Instance")]
    private static UIManager instance; // UI 매니저 전역 변수

    public static UIManager Instance // UI 매니저 인스턴스 접근용 프로퍼티
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("UIManager").AddComponent<UIManager>();
            }

            return instance;
        }
    }

    [Header("UI Components")]
    [SerializeField] private StageClearUI stageClearUI; // 스테이지 클리어 UI 컴포넌트
    [SerializeField] private GameOverUI gameOverUI; // 게임 오버 UI 컴포넌트

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }
    }

    private void Start()
    {
        if (stageClearUI == null || gameOverUI == null)
        {
            Debug.LogError("UIManager: Missing UI Components in UIManager.");

            return;
        }
    }

    public void ShowStageClearUI()
    {
        stageClearUI.gameObject.SetActive(true);
    }

    public void HideStageClearUI()
    {
        stageClearUI.gameObject.SetActive(false);
    }

    public void ShowGameOverUI(string reason)
    {
        gameOverUI.gameObject.SetActive(true);
        gameOverUI.ShowGameOver(reason);
    }

    public void HideGameOverUI()
    {
        gameOverUI.gameObject.SetActive(false);
    }
}
