using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public PuzzleLight[] getPuzzles; // PuzzleLight�� ������ �ִ� �ڽ� ������Ʈ�� ��� ���� �迭

    public PuzzleLight[,] puzzles = new PuzzleLight[5,5]; // �� �迭 ���� ������Ʈ�� ������ ���·� ��� ���� �迭

    public Door door;

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

        getPuzzles = GetComponentsInChildren<PuzzleLight>(); // ó������ 2���� �迭�� ���� �� ���� �켱 1���� �迭�� �Ҵ�

        
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int k = 0;
        // 1���� �迭�� ��� �ڽĵ�(����Ʈ)�� 2���� �迭�� �ű�
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


    public void TurnSideLights(PuzzleLight puzzle) // Ŭ���� ����Ʈ�� �翷 �� ���Ʒ��� Ű�ų� ���� �޼���
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (puzzles[i, j] == puzzle) // Ŭ���� ������ ��ġ�� ã��
                {
                    // Ŭ���� ������ �翷 �� ���Ʒ��� ����Ʈ�� ���� ��� ���ų� Ŵ
                    TurnOnOff(i + 1, j);
                    TurnOnOff(i - 1, j);
                    TurnOnOff(i , j + 1);
                    TurnOnOff(i , j - 1);
                }
            }
        }
    }

    void TurnOnOff(int i, int j) // ����Ʈ �¿��� ��� �޼���
    {
        if(i >= 0 && i < 5 && j >= 0 && j < 5)
        {
            puzzles[i, j].isLightON = !puzzles[i, j].isLightON;
        }

        SoundManager.Instance?.PlaySFX("buttonPressSound");
    }


    public bool CheckPuzzleClear() // ���� Ŭ���� �޼���
    {
        bool isAllOn = true;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                // ����Ʈ�� �ϳ��� ���������� Ŭ���� �ȵ� ����
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

    public void ClearedPuzzle() // ������ Ŭ�����ϸ� ���� ������ �޼���
    {
        isClear = true;
        GameManager.Instance.PuzzleSolved("GasRoom_Puzzle1"); // ���� Ŭ���� ���¸� GameManager�� �˸�
        door = GameManager.Instance.door1Object.GetComponent<Door>();
        door.ActivateBeacon();
        // CharacterManager.Instance.Player.controller.SetPuzzleActive(false);
        SceneManager.UnloadSceneAsync("PuzzleScene");
        Debug.Log("���� Ŭ����! ���� �����ϴ�.");

        SoundManager.Instance?.PlaySFX("doorOpenSound");
    }

    public void GoBack()
    {
        CharacterManager.Instance.Player.controller.SetPuzzleActive(false);
        SceneManager.UnloadSceneAsync("PuzzleScene");
    }

}
