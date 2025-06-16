using System;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public bool introScene { get; private set; } = true;
    public static UiManager Instance { get; private set; }
    private Dictionary<Type, UiBase> ui = new();

    private void Awake()
    {
        if (UiManager.Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            var uiBase = this.transform.GetComponentsInChildren<UiBase>(true);

            for (int i = 0; i < uiBase.Length; i++)
            {
                uiBase[i].Init();
            }
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    public void IntroScene(bool _isIntro)
    {
        introScene = _isIntro;
    }

    public void Add<T>(UiBase _ui) where T : UiBase
    {
        var key = typeof(T);

        if (!ui.ContainsKey(key)) ui.Add(key, _ui);
        else Service.Log($"{key.Name}라는 Ui는 이미 추가된 상태");
    }

    public void Remove<T>(UiBase _ui) where T : UiBase
    {
        var key = typeof(T);

        if (ui.ContainsKey(key)) ui.Remove(key);
        else Service.Log($"{key.Name}라는 Ui는 추가된적이 없음");
    }

    public T Get<T>() where T : UiBase
    {
        var key = typeof(T);

        if (ui.ContainsKey(key)) return ui[key] as T;
        else Service.Log($"{key.Name}라는 Ui는 추가된적이 없음");

        return null;
    }

    public void Show<T>(bool _isActive) where T : UiBase
    {
        var key = typeof(T);

        if (ui.ContainsKey(key)) ui[key].Show(_isActive);
        else Service.Log($"{key.Name}라는 Ui는 추가된적이 없음");
    }
}
