using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialType
{
    Basic,
    Lobby,
    PoisonMap,
    PoisonMapPuzzle,
    BeaconGimmick
}

/// <summary>
/// 플레이 방법, 또는 지역의 특징 등을 설명하는 튜토리얼 클래스
/// </summary>
public class Tutorial : MonoBehaviour
{
    private TutorialUI ui;

    [SerializeField] private TutorialType type;

    void Start()
    {
        ui = FindObjectOfType<TutorialUI>();
    }

    void Update()
    {
        
    }

    // 이미 표시한 튜토리얼은 name을 리스트에 저장한 후, 같은 name과 충돌했을 경우, 표시하지 않도록 한다
    private void OnTriggerEnter(Collider other)
    {
        if (this.GetComponent<Tutorial>() != null) // 튜토리얼 트리거와 충돌했을 때
        {
            string tutorialHeaderText = "";   // 튜토리얼 제목 텍스트
            string tutorialBodyText = "";   // 튜토리얼 본문 텍스트
            float waitTime = 5f; // UI표시 시간

            switch (this.type)
            {
                case TutorialType.Basic:
                    tutorialHeaderText = "Controlls";
                    tutorialBodyText = "MOVE: Left Joystick\r\nMOVE VIEW: Right Joystick\r\nRun: Left_PrimaryButton\r\nJUMP: Right_PrimaryButton\r\nINTERACT: Trigger Button";
                    waitTime = 10f;

                    break;
                case TutorialType.Lobby:
                    tutorialHeaderText = "Clear Conditions";
                    tutorialBodyText = "All beacons must be activated to open the exit door.\r\nSolve all puzzles in each map to escape.";
                    break;
                case TutorialType.PoisonMap:
                    tutorialHeaderText = "Poisonous Area";
                    tutorialBodyText = "This is a poisonous area.\r\nHealth decreases continuously, and falling into the poison swamp will cause more damage.";

                    break;
                case TutorialType.PoisonMapPuzzle:
                    tutorialHeaderText = "Puzzle Gimmick";
                    tutorialBodyText = "You can solve puzzles by interacting with them.\r\nClicking a ball changes the color of balls in a cross pattern.\r\nChange all balls to the same color to succeed.";

                    break;
                case TutorialType.BeaconGimmick:
                    tutorialHeaderText = "Key Item Acquisition Method";
                    tutorialBodyText = "You can acquire the key item by moving the platform appropriately and climbing up to it.\r\nAfter acquiring the key item, you can clear the map by placing it in a beacon of the same color as the item.";
                    waitTime = 10f;

                    break;
                default:
                    Debug.Log("알 수 없는 튜토리얼 오브젝트입니다.");
                    break;
            }

            ui.ShowTutorial(type, tutorialHeaderText, tutorialBodyText, waitTime);
        }
    }
}
