using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Player.Player_Battle;
using UnityEngine;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int>{}

//무기가 활성화 될때 해당 무기의 탄 수 정보를 갱신한다.
//onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
//이벤트 클래스에 호출할 메소드 등록(외부 클래스)
//weapon.onAmmoEvent.AddListener(UpdateAmmoHUD);
//이벤트 클래스에 등록되는 메소드(이벤트 클래스의 Invoke()가 호출될 때 자동 호출)
//private void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
// {
//     textAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
// }
[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int>{}

public enum WeaponType{ Main = 0, Melee}

public abstract class WeaponBase : MonoBehaviour
{
    [Header("WeaponBase")] [SerializeField]
    protected WeaponType weaponType;        //무기종류

    [SerializeField] protected WeaponSetting weaponSetting; //무기설정

    protected float lastAttackTime = 0f;
    protected bool isReload = false;
    protected bool isAttack = false;
    protected PlayerAnimatorController animator;
    
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();
    [HideInInspector]
    public MagazineEvent onMagazineEvent = new MagazineEvent();
    
    public PlayerAnimatorController Animator => animator;

    public WeaponName WeaponName => weaponSetting.WeaponName;
    public int currentMagazine => weaponSetting.currentMagazine;

    public int MaxMagazine => weaponSetting.maxMagazine;

    public abstract void StartWeaponAction(int type = 0);
    public abstract void StopWeaponAction(int type = 0);
    public abstract void StartReload();

    protected void Setup()
    {
        animator = GetComponent<PlayerAnimatorController>();
    }

}
