using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class PuzzleObject : MonoBehaviour
{
    [SerializeField] private XRSimpleInteractable interactable;

    [Header("Puzzle Event Informations")]
    public GameObject PlayerAnchor;
    public GameObject PuzzleAnchor;
    private XRPlayer player;
    private PuzzleManager puzzleUIObject;

    [Header("Volatility Player Informations")]
    private Vector3 backupPosition = Vector3.zero;   // 퍼즐 시작 전 플레이어 위치 정보
    private Quaternion backupRotation = Quaternion.identity;    // 퍼즐 시작 전 플레이어 회전 정보

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(OnSelectEntered);

        if (interactable == null)
            Debug.LogError("Interactable component is not assigned.");
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // 퍼즐 씬 로드 중일 때 상호작용 하지 않음
        if (SceneManager.GetActiveScene().name == "PuzzleScene")
            return;

        StartCoroutine(LoadPuzzleScene());
    }

    private IEnumerator LoadPuzzleScene()
    {
        // 플레이어 컨트롤 비활성화 후, 퍼즐 씬 로드
        PlayerManager.Instance.Player.Controller.SetPuzzleActive(false);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("PuzzleScene", LoadSceneMode.Additive);
        yield return asyncLoad;

        puzzleUIObject = FindObjectOfType<PuzzleManager>();

        if (puzzleUIObject == null)
        {
            Debug.LogError("PuzzleObject not found in PuzzleScene.");
        }

        // 씬 로드 후, 이벤트 세팅
        SetPuzzleEvent();
    }

    public void SetPuzzleEvent()
    {
        player = PlayerManager.Instance.Player;
        PuzzleManager.Instance.puzzleObject = this;
        PuzzleManager.Instance.isPuzzleActive = true;

        // 상호작용 전 플레이어 위치, 회전값 백업
        backupPosition = player.transform.position;
        backupRotation = player.transform.rotation;

        // 상호작용을 위해 플레이어 오브젝트 위치, 회전값 변경
        player.transform.position = PlayerAnchor.transform.position;
        player.transform.rotation = PlayerAnchor.transform.rotation;

        // Additive로 새로 연 퍼즐 씬의 퍼즐 UI 정보 취득
        PuzzleManager puzzleObj = FindObjectOfType<PuzzleManager>();

        if (puzzleObj != null)
        {
            // 상호작용을 위해 퍼즐 UI 오브젝트 위치, 회전값 변경
            puzzleObj.transform.position = PuzzleAnchor.transform.position;
            puzzleObj.transform.Rotate(0, 180, 0);
        }
        else
        {
            Debug.LogError("PuzzleObject not found in PuzzleScene.");
        }
    }

    /// <summary>
    /// 퍼즐 이벤트 종료 시, 관련 정보를 초기화한다
    /// </summary>
    public void ResetEvent()
    {
        // 플레이어 위치, 회전값 복구
        player.transform.position = backupPosition;
        player.transform.rotation = backupRotation;

        // 퍼즐 UI 초기화
        PuzzleManager.Instance.puzzleObject = null;
        PuzzleManager.Instance.isPuzzleActive = false;

        // 백업 데이터 초기화
        backupPosition = Vector3.zero;
        backupRotation = Quaternion.identity;
    }
}
