using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public ObjectData objectData;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }

    public Color ChangeColor(ObjectColor itemColor)
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

    /// <summary>
    /// 인터랙션 오브젝트의 정보를 반환하는 메소드
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public ObjectData GetInteractableInfo()
    {
        return objectData;
    }
}
