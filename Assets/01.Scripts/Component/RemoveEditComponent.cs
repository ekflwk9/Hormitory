using UnityEngine;

public class RemoveEditComponent : MonoBehaviour
{
    private void Reset()
    {
        //설마 같은 컴포넌트를 100개 넣는 싸이코는 없겠지
        for (int i = 0; i < 100; i++)
        {
            FindComponent<BoxCollider>(this.transform);
            FindComponent<Rigidbody>(this.transform, true);
            FindComponent<MeshCollider>(this.transform, true);
            FindComponent<CapsuleCollider>(this.transform, true);
            FindComponent<CreateEditMap>(this.transform, true);
        }
    }

    private void FindComponent<T>(Transform _parent, bool _isAllRemove = false) where T : Component
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            var child = _parent.GetChild(i);
            var component = child.GetComponent<T>();

            if (!_isAllRemove && (child.name.Contains("Wall") || child.name.Contains("Floor"))) continue;
            else if (component != null) DestroyImmediate(component);

            FindComponent<T>(child.transform, _isAllRemove);
        }
    }
}
