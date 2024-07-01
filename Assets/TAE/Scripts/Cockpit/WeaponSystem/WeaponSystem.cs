using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static CockpitSystem;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] WeaponList;
    [SerializeField] private Transform[] shootPositions;
    [SerializeField] private Transform target;
    [SerializeField] private List<VisualEffect> Effects = new List<VisualEffect>();
    [SerializeField] private VisualEffect ShotgunEffect;

    private GameObject curWeapon;
    private WeaponType curWeaponType;
    private bool isShooting = false;
    public GameObject CurWeapon { get { return curWeapon; } private set { } }
    public WeaponType CurWeaponType {  get { return curWeaponType; } private set { } }

    public int GetWeaponRange(WeaponType weaponType)
    {
        SetWeapon(weaponType);

        switch (weaponType)
        {
            case WeaponType.MG:
                return curWeapon.GetComponent<MachineGun>().Range;
            case WeaponType.SG:
                return curWeapon.GetComponent<ShotGun>().Range;
            case WeaponType.ML:
                return curWeapon.GetComponent<MissileLauncher>().Range;
            default:
                return 0;
        }
    }

    private void SetWeapon(WeaponType weapontype)
    {
        curWeaponType = weapontype;

        switch (curWeaponType)
        {
            case WeaponType.MG:
                curWeapon = WeaponList[0];
                break;
            case WeaponType.SG:
                curWeapon = WeaponList[1];
                break;
            case WeaponType.ML:
                curWeapon = WeaponList[2];
                break;
            default:
                curWeapon = WeaponList[0];
                break;
        }
    }
    
    public GameObject GetWeapon(WeaponType weapontype)
    {
        switch (weapontype)
        {
            case WeaponType.MG:
                return WeaponList[0];
            case WeaponType.SG:
                return WeaponList[1];
            case WeaponType.ML:
                return WeaponList[2];
            default:
                return null;
        }
    }

    private void TrackingTarget()
    {
        foreach (Transform t in shootPositions)
        {
            t.LookAt(target);
        }
    }
    public void StopShooting()
    {
        StopAllCoroutines();
        isShooting = false;
    }

    public void ShootingActive(bool trigger)
    {
        TrackingTarget();

        bool continuousFire = IsContinuousFire(curWeaponType, curWeapon);

        if (trigger)
        {
            if (continuousFire)
            {
                if (!isShooting)
                {
                    StartCoroutine(StartShootingCoroutine(curWeapon));
                }
            }
            else
            {
                if (!isShooting)
                {
                    StartShooting(curWeapon);
                    isShooting = true; // 한 번만 발사
                }
            }
        }
        else
        {
            if (isShooting)
            {
                isShooting = false;
            }
        }
    }

    private bool IsContinuousFire(WeaponType weaponType, GameObject weapon)
    {
        switch (weaponType)
        {
            case WeaponType.MG:
                return weapon.GetComponent<MachineGun>().ContinuousFire;
            case WeaponType.SG:
                return weapon.GetComponent<ShotGun>().ContinuousFire;
            case WeaponType.ML:
                return weapon.GetComponent<MissileLauncher>().ContinuousFire;
            default:
                return false;
        }
    }

    private IEnumerator StartShootingCoroutine(GameObject weapon)
    {
        isShooting = true;
        while (isShooting)
        {
            StartShooting(weapon);
            yield return new WaitForSeconds(0.1f); // 연속 발사 속도 조절
        }
    }

    private void StartShooting(GameObject weapon)
    {
        switch (curWeaponType)
        {
            case WeaponType.MG:
                curWeapon.GetComponent<MachineGun>().Fire(shootPositions, target);
                break;
            case WeaponType.SG:
                curWeapon.GetComponent<ShotGun>().Fire(shootPositions, target);
                ShotgunEffect.Play();
                break;
            case WeaponType.ML:
                curWeapon.GetComponent<MissileLauncher>().Fire(shootPositions, target);
                break;
        }
    }
}
