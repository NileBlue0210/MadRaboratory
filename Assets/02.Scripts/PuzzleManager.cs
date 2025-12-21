using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    public static PuzzleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("PuzzleManager").AddComponent<PuzzleManager>();
            }
            return instance;
        }
    }

    private bool isClear = false;

    public PuzzleLight[] getPuzzles;

    public PuzzleLight[,] puzzles = new PuzzleLight[5,5];

    [HideInInspector] public Door Door;

    [HideInInspector] public PuzzleObject puzzleObject;
    public bool isPuzzleActive = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        getPuzzles = GetComponentsInChildren<PuzzleLight>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int k = 0;

        for (int i = 0; i< 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                puzzles[i, j] = getPuzzles[k++];
            }
        }
    }

    private void Update()
    {
        if(CheckPuzzleClear() && isClear == false)
        {
            ClearedPuzzle();
        }
    }


    public void TurnSideLights(PuzzleLight puzzle)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (puzzles[i, j] == puzzle)
                {
                    TurnOnOff(i + 1, j);
                    TurnOnOff(i - 1, j);
                    TurnOnOff(i , j + 1);
                    TurnOnOff(i , j - 1);
                }
            }
        }
    }

    void TurnOnOff(int i, int j)
    {
        if(i >= 0 && i < 5 && j >= 0 && j < 5)
        {
            puzzles[i, j].isLightON = !puzzles[i, j].isLightON;
        }

        SoundManager.Instance?.PlaySFX("buttonPressSound");
    }


    public bool CheckPuzzleClear()
    {
        bool isAllOn = true;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (puzzles[i, j].isLightON == false)
                {
                    isAllOn = false;
                    break;
                }
            }
        }

        if (isAllOn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ClearedPuzzle()
    {
        isClear = true;
        GameManager.Instance.PuzzleSolved("GasRoom_Puzzle1");

        Door = GameManager.Instance.door1Object.GetComponent<Door>();
        Door.ActivateBeacon();

        SceneManager.UnloadSceneAsync("PuzzleScene");
        SoundManager.Instance?.PlaySFX("doorOpenSound");

        PlayerManager.Instance.Player.Controller.SetPuzzleActive(true); // 플레이어 컨트롤러 활성화
        puzzleObject.ResetEvent();
    }

    public void GoBack()
    {
        SceneManager.UnloadSceneAsync("PuzzleScene");
        PlayerManager.Instance.Player.Controller.SetPuzzleActive(true); // 플레이어 컨트롤러 활성화
        puzzleObject.ResetEvent();
    }
}
