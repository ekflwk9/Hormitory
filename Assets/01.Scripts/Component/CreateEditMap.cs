using UnityEngine;

/// <summary>
/// 맵 생성 전용 열거형 데이터
/// </summary>
public enum CreateType
{
    Back,
    Forward,
    Right,
    Left,
    Up,
    Down,
}

public class CreateEditMap : MonoBehaviour
{
    [Space(10f)]
    [Header("어떤 오브젝트인가?")]
    [SerializeField] private GameObject mapObject;

    [Space(10f)]
    [Header("어느 방향으로 나아갈 것인가?")]
    [SerializeField] private CreateType direction;

    [Header("이동 간격은 몇인가?")]
    [SerializeField] private float moveValue;

    [Space(10f)]
    [Header("몇 개 생성할 것인가?")]
    [SerializeField] private int spawnCount;

    private void Start()
    {
        SetFullMap(direction);
    }

    private void SetFullMap(CreateType _direction)
    {
        var nextPos = mapObject.transform.position;
        var newDiretion = Direction();

        for (int i = 0; i < spawnCount; i++)
        {
            var spawnMapObject = Instantiate(mapObject);
            nextPos += newDiretion;

            spawnMapObject.transform.position = nextPos;
        }
    }

    private Vector3 Direction()
    {
        var posValue = Vector3.zero;

        switch (direction)
        {
            case CreateType.Back:
                posValue.z = -1;
                break;

            case CreateType.Forward:
                posValue.z = 1;
                break;

            case CreateType.Right:
                posValue.x = 1;
                break;

            case CreateType.Left:
                posValue.x = -1;
                break;

            case CreateType.Up:
                posValue.y = 1;
                break;

            case CreateType.Down:
                posValue.y = -1;
                break;
        }

        posValue *= moveValue;
        return posValue;
    }
}
