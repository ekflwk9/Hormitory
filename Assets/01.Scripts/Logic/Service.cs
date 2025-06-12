using UnityEditor;
using UnityEngine;

public static class AnimName
{
    public static int FadeIn = Animator.StringToHash("FadeIn");
    public static int FadeOut = Animator.StringToHash("FadeOut");
    public static int Idle = Animator.StringToHash("Idle");
    public static int Hit = Animator.StringToHash("Hit");
}

public static class StringMap
{
    public const string Data = "Data";
    public const string Touch = "Touch";
    public const string Sound = "Sound";
    public const string Item = "Item";
    public const string Image = "Image";
    public const string Icon = "Icon";
    public const string Count = "Count";
    public const string Info = "Info";
    public const string Name = "Name";
    public const string Battle = "Battle";
    public const string Puzzle = "Puzzle";
    public const string Ending = "Ending";
}

public static class Service
{
    /// <summary>
    /// 예외처리된 특정 자식 이름의 컴포넌트를 가져오는 메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_this"></param>
    /// <param name="_childName"></param>
    /// <returns></returns>
    public static T TryGetChildComponent<T>(this MonoBehaviour _this, string _childName) where T : class
    {
        var child = Service.FindChild(_this.transform, _childName);
        if (child == null) return null;

        T component = null;

        if (child.TryGetComponent<T>(out var findComponent)) component = findComponent;
        else Service.Log($"{child.name}에 {typeof(T).Name}이라는 컴포넌트는 존재하지 않음");

        return component;
    }

    /// <summary>
    /// 예외처리가된 특정 자식 오브젝트의 컴포넌트를 가져오는  메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_parent"></param>
    /// <param name="_childName"></param>
    /// <returns></returns>
    public static T TryGetChildComponent<T>(this MonoBehaviour _this) where T : class
    {
        var component = _this.GetComponentInChildren<T>();
        if (component == null) Service.Log($"{_this.name}에 {typeof(T).Name}이라는 컴포넌트는 존재하지 않음");

        return component;
    }

    /// <summary>
    /// 예외처리가 된 자기 자신 컴포넌트를 가져오는 메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_this"></param>
    /// <returns></returns>
    public static T TryGetComponent<T>(this MonoBehaviour _this)
    {
        if (_this.TryGetComponent<T>(out var component)) return component;
        else Service.Log($"{_this.name}에 {typeof(T).Name}이라는 컴포넌트는 존재하지 않음");

        return default(T);
    }

    /// <summary>
    /// 특정 이름의 자식 오브젝트를 반환
    /// </summary>
    /// <param name="_parent"></param>
    /// <param name="_childName"></param>
    /// <returns></returns>
    public static GameObject TryFindChild(this MonoBehaviour _parent, string _childName)
    {
        var child = Service.FindChild(_parent.transform, _childName);
        if (child == null) Service.Log($"{_parent.name}에 {_childName}이라는 자식 오브젝트는 존재하지 않음");

        return child;
    }

    private static GameObject FindChild(Transform _parent, string _childName)
    {
        GameObject findChild = null;

        for (int i = 0; i < _parent.transform.childCount; i++)
        {
            var child = _parent.transform.GetChild(i);
            findChild = child.name == _childName ? child.gameObject : FindChild(child, _childName);
            if (findChild != null) break;
        }

        return findChild;
    }

    public static T FindRresource<T>(string _fileName, string _resource) where T : Object
    {
        var source = Resources.Load<T>($"{_fileName}/{_resource}");

        if (source == null) Service.Log($"{_fileName}에 {_resource}이라는 리소스가 존재하지 않음");
        return source;
    }

    /// <summary>
    /// 에디터 상에서 로그를 띄워주는 
    /// </summary>
    /// <param name="_log"></param>
    public static void ShowEditorWindow(string _log)
    {
#if UNITY_EDITOR
        EditorUtility.DisplayDialog("알림", _log, "확인");
#endif
    }

    /// <summary>
    /// 에디터 전용 로그
    /// </summary>
    /// <param name="_log"></param>
    public static void Log(string _log)
    {
#if UNITY_EDITOR
        Debug.Log(_log);
#endif
    }
}
