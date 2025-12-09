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

        Debug.Log("퍼즐 오브젝트 활성화됨");
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelectEntered);

        Debug.Log("퍼즐 오브젝트 비활성화됨");
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        LoadPuzzleScene();

        Debug.Log("퍼즐 오브젝트가 선택되었습니다.");
    }

    private void LoadPuzzleScene()
    {
        SceneManager.LoadScene("PuzzleScene", LoadSceneMode.Additive);
    }
}
