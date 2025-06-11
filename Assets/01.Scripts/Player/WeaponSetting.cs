using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponName {AssaultRifle =0}

[System.Serializable]
public class WeaponSetting
{
    public WeaponName WeaponName;
    public int currentAmmo;
    public int maxAmmo;
    public float attackRate;
    public float attackDistance;
    public bool isAutomaticAttack;
}
