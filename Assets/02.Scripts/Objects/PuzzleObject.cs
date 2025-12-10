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
        LoadPuzzleScene();
    }

    private void LoadPuzzleScene()
    {
        SceneManager.LoadScene("PuzzleScene", LoadSceneMode.Additive);
    }
}
