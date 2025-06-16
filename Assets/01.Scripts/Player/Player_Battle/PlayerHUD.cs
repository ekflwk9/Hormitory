using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    private WeaponBase weapon;                      //현재 정보가 출력되는 무기

    [SerializeField] private Status status;
    
    
    [Header("Weapon Base")] [SerializeField]
    private TextMeshProUGUI textWeaponName;

    [SerializeField] private Image imageWeaponIcon;             //무기 아이콘
    [SerializeField] private Sprite[] spriteWeaponIcons;        //무기 아이콘에 사용되는 sprite 배열
    [SerializeField] private Vector2[] sizeWeaponIcons;
    
    [Header("Ammo")][SerializeField] private TextMeshProUGUI textAmmo;// 현재/최대 탄수 출력

    [Header("Magazine")] [SerializeField] private GameObject magazineUIPrefab; //탄창 UI 프리펩
    [SerializeField] private Transform magazineParent;   //탄창 UI가 배치되는 Panel
    [SerializeField] private int maxMagazineCount;      //처음 생성하는 최대 탄창 수
    
    private List<GameObject> magazineList;  //탄창 UI 리스트
    private void Awake()
    {
        status.onHPEvent.AddListener(UpdateHPHUD);
    }
    //
    // public void SetupAllWeapons(WeaponBase[] weapons)
    // {
    //     SetupMagazine();
    //     
    //     for (int i = 0; i < weapons.Length; ++i)
    //     {
    //         weapons[i].onAmmoEvent.AddListener(UpdateAmmoUHD);
    //         weapons[i].onMagazineEvent.AddListener(UpdateMagazineHUD);
    //     }
    // }
    //
    // public void SwitchingWeapon(WeaponBase newWeapon)
    // {
    //     weapon = newWeapon;
    //         
    //     SetupWeapon();
    // }
    // private void SetupWeapon()
    // {
    //     textWeaponName.text = weapon.WeaponName.ToString();
    //     imageWeaponIcon.sprite = spriteWeaponIcons[(int)weapon.WeaponName];
    //     imageWeaponIcon.rectTransform.sizeDelta = sizeWeaponIcons[(int)weapon.WeaponName];
    // }
    //
    // private void UpdateAmmoUHD(int currentAmmo, int maxAmmo)
    // {
    //     textAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
    // }
    //
    // private void SetupMagazine()
    // {
    //     //weapon에 등록되어 있는 최대 탄창 갯수만큼 Image Icon 생성
    //     //magazineParent 오브젝트의 자식으로 등록후 모두 비활설화 후 리스트에 저장
    //     magazineList = new List<GameObject>();
    //     for(int i = 0; i < maxMagazineCount; ++i)
    //     {
    //         GameObject clone = Instantiate(magazineUIPrefab);
    //         clone.transform.SetParent(magazineParent);
    //         clone.SetActive(false);
    //         
    //         magazineList.Add(clone);
    //     }
    // }
    //
    // private void UpdateMagazineHUD(int currentMagazine)
    // {
    //     //전부 비황성화하고, currentMagazine 갯수만큼 활성화
    //     for(int i = 0; i < magazineList.Count; ++i)
    //     {
    //         magazineList[i].SetActive(false);
    //     }
    //
    //     if (currentMagazine < 0 || currentMagazine >= magazineList.Count)
    //     {
    //         Debug.LogError($"탄창 인덱스 {currentMagazine}가 범위를 벗어났습니다. 컬렉션 크기: {magazineList.Count}");
    //         return;
    //     }
    //     for (int i = 0; i < currentMagazine; ++i)
    //     {
    //         magazineList[i].SetActive(true);
    //     }
    // }

    private void UpdateHPHUD(float previous, float current)
    {
        if (previous - current > 0)
        {
            UiManager.Instance.Get<HitUi>().HitView();
        }
    }
}
