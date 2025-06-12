using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponName {AssaultRifle =0}


[System.Serializable]
public class WeaponSetting
{
    public WeaponName WeaponName;
    public int damage;
    public int currentMagazine;             //현재 탄창 수
    public int maxMagazine;                 //최대 탄창 수
    public int currentAmmo;
    public int maxAmmo;
    public float attackRate;                //공격 속도
    public float attackDistance;            //공격 사거리
    public bool isAutomaticAttack;          //연속 공격 여부
}
