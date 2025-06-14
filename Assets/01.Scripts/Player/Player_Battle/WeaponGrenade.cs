using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Component;
using _01.Scripts.Player.Player_Battle;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponGrenade : WeaponBase
{
    [Header("Grenade")] [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform grenadeSpawnPoint;

    private void Awake()
    {
        base.Setup();

        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }

    private void OnEnable()
    {
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }

    public override void StartWeaponAction(int type = 0)
    {
        if (type == 0 && isAttack == false && weaponSetting.currentAmmo > 0)
        {
            StartCoroutine("OnAttack");
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
       
    }

    public override void StartReload()
    {
      
    }

    private IEnumerator OnAttack()
    {
        isAttack = true;

        animator.Play("Fire", -1, 0);
        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (animator.CurrentAnimationsIs("Movement"))
            {
                isAttack = false;
                yield break;
            }

            yield return null;
        }
    }

    public void SpawnGrenadeProjectile()
    {
        GameObject grenadeClone = Instantiate(grenadePrefab,grenadeSpawnPoint.position, Random.rotation);
        grenadeClone.GetComponent<WeaponGreanadeProjectile>().Setup(weaponSetting.damage,transform.parent.forward);

        weaponSetting.currentAmmo--;
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }
}
