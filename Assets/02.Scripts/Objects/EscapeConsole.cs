using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeConsole : MonoBehaviour
{
    [Header("Escape Console Objects")]
    private EscapeButton escapeButton;   // 탈출 버튼
    private OXPanel OXpanel; // 버튼 눌림 표시 패널
    [SerializeField] private GameObject exitDoor; // 출구 문 오브젝트
    private IBeaconActivate doorBeaconActivate; // 출구 문 비콘 활성화 인터페이스

    private void Awake()
    {
        escapeButton = GetComponentInChildren<EscapeButton>();
        OXpanel = GetComponentInChildren<OXPanel>();
        doorBeaconActivate = exitDoor.GetComponent<IBeaconActivate>();

        if (escapeButton == null || OXpanel == null)
        {
            Debug.LogError("EscapeConsole: Missing required components : " + gameObject.name);
        }

        if (doorBeaconActivate == null)
        {
            Debug.LogError("EscapeConsole: IBeaconActivate component is not found on exitDoor : " + exitDoor.name);
        }
    }

    void Start()
    {
        TriggerExitDoor(false); // 초기 상태에서는 출구 문이 닫혀있음
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        escapeButton.OnPressed += HandleButtonPressed;
    }

    private void OnDisable()
    {
        escapeButton.OnPressed -= HandleButtonPressed;
    }

    private void HandleButtonPressed()
    {
        OXpanel.ShowPanelO();
        TriggerExitDoor(true);  // 출구 문 열기
    }

    private void TriggerExitDoor(bool doorOpen)
    {
        if (doorOpen)
        {
            OXpanel.ShowPanelO();
            doorBeaconActivate.ActivateBeacon();
            GameManager.Instance.ReportFinalStageButtonPressed(true);

            return;
        }
        else
        {
            OXpanel.ShowPanelX();
            doorBeaconActivate.DeactivateBeacon();
            GameManager.Instance.ReportFinalStageButtonPressed(false);

            return;
        }
    }
}
