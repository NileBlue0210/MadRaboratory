using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitGameButton : MonoBehaviour
{
    [SerializeField] private Button quitGameButton;

    void Start()
    {
        if (quitGameButton != null)
        {
            quitGameButton.onClick.AddListener(Quit);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
