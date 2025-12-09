using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour, IInteractable
{
    public ObjectData objectData;

    private new Renderer renderer;
    [SerializeField] private bool isActivated = false;
    public IBeaconActivate beaconActivate;
    public GameObject door;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        beaconActivate = door.GetComponent<IBeaconActivate>();
        renderer.material.color = ChangeColor(objectData.objectColor);
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        PickupableItem item = collision.gameObject.GetComponent<PickupableItem>();

        if (item != null)
        {
            if (ReceiveItem(item))
            {
                item.OnPlace(); // �������� ���ܿ� ��ġ�Ǿ��� ��, ������ �۵� �޼ҵ� ȣ��
                GameManager.Instance.ReportBeaconActivated(true);
                ActivateGimmick();
            }
            else
            {
                Debug.Log("�ùٸ� Ű �������� �ƴմϴ�.");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        PickupableItem item = collision.gameObject.GetComponent<PickupableItem>();

        if (item != null)
        {
            isActivated = false; // ��Ȱ��ȭ ���·� ����
            GameManager.Instance.ReportBeaconActivated(false);
            ActivateGimmick();
        }
    }

    /// <summary>
    /// �������� ���ܿ� ��ġ�Ǿ��� ��, �ùٸ� ��ȣ�ۿ� ���������� ������ ���� �Ǻ�
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool ReceiveItem(PickupableItem item)
    {
        bool result = false;

        if (item.objectData.objectColor == objectData.objectColor)
        {
            result = true;

            isActivated = true;
        }

        return result;
    }
    
    /// <summary>
    /// ���ܰ� ����� ����� ���۽�Ű�� �޼ҵ�
    /// </summary>
    /// memo : ����� ����� ���ܿ� �����ų �ʿ䰡 �ִ�. ����� ����Ǿ� ���� ����
    private void ActivateGimmick()
    {
        if (isActivated && beaconActivate != null)
        {
            beaconActivate.ActivateBeacon();
            GameManager.Instance.ReportBeaconActivated(true);
        }
        else
        {
            beaconActivate.DeactivateBeacon();
            GameManager.Instance.ReportBeaconActivated(false);
        }
    }

    // memo : PickupableItem�� ChangeColor�ڵ�� �ߺ��̴�. ��ӹ޾Ƽ� ���� ���� ���� �� �ѵ�..Item, Ȥ�� Object��� Ŭ������ ����� ��ӹ޴°� ���� ������?
    private Color ChangeColor(ObjectColor itemColor)
    {
        switch (itemColor)
        {
            case ObjectColor.RED:
                return Color.red;
            case ObjectColor.BLUE:
                return Color.blue;
            case ObjectColor.GREEN:
                return Color.green;
            case ObjectColor.YELLOW:
                return Color.yellow;
            default:
                return Color.white;
        }
    }

    public ObjectData GetInteractableInfo()
    {
        return objectData;
    }
}
