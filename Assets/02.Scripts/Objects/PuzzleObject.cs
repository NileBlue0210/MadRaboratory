using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class PuzzleObject : MonoBehaviour
{
    [SerializeField] private XRSimpleInteractable interactable;

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
        StartCoroutine(LoadPuzzleScene());
    }

    private IEnumerator LoadPuzzleScene()
    {
        // 플레이어 컨트롤 비활성화 후, 퍼즐 씬 로드
        PlayerManager.Instance.Player.Controller.SetPuzzleActive(false);

        // 씬이 로드된 후 퍼즐 오브젝트 정보를 매니저에 할당
        yield return SceneManager.LoadSceneAsync("PuzzleScene", LoadSceneMode.Additive);
        PuzzleManager.Instance.PuzzleObject = gameObject;

        // 퍼즐 설정 코루틴
        StartCoroutine(PuzzleManager.Instance.SetupPuzzleCoroutine());
    }
}
