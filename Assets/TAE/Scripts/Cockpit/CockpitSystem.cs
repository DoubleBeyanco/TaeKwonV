using System.Collections;
using UnityEngine;

public class CockpitSystem : MonoBehaviour
{
    public enum WeaponType
    {
        MG,
        SG,
        ML,
    }

    [SerializeField] private GameObject[] weaponList;
    [SerializeField] private Transform target;
    [SerializeField] private Transform[] shootPositions;
    [SerializeField] private Transform Test;

    private CockpitJoystick stick;
    private AimingSystem aim;

    private WeaponType currentWeaponType;
    private GameObject currentWeapon;
    private bool isShooting = false;

    private void Awake()
    {
        stick = GetComponentInChildren<CockpitJoystick>();
        aim = GetComponentInChildren<AimingSystem>();
    }

    private void Start()
    {
        currentWeaponType = WeaponType.MG; // 기본 무기 타입 설정
        currentWeapon = weaponList[(int)currentWeaponType];
        stick.AimingModeCallback = AimMode;
    }

    public void TrackingTarget()
    {
        foreach (Transform t in shootPositions)
        {
            t.LookAt(target);
        }
    }

    public void SwitchWeapon(WeaponType newWeaponType)
    {
        currentWeaponType = newWeaponType;
        currentWeapon = weaponList[(int)currentWeaponType];
    }

    private void AimMode(bool isAiming)
    {
        int range = GetWeaponRange(currentWeaponType, currentWeapon);

        aim.AimCalc(stick.transform.eulerAngles, range);
        ShootingActive(isAiming);
    }

    private int GetWeaponRange(WeaponType weaponType, GameObject weapon)
    {
        switch (weaponType)
        {
            case WeaponType.MG:
                return weapon.GetComponent<MachineGun>().Range;
            case WeaponType.SG:
                return weapon.GetComponent<ShotGun>().Range;
            case WeaponType.ML:
                return weapon.GetComponent<MissileLauncher>().Range;
            default:
                return 0;
        }
    }

    private void ShootingActive(bool trigger)
    {
        TrackingTarget();

        if (!stick.isActive || !stick.IsPrimaryButtonPressed)
        {
            StopShooting();
            return;
        }

        bool continuousFire = IsContinuousFire(currentWeaponType, currentWeapon);

        if (trigger)
        {
            if (continuousFire)
            {
                if (!isShooting)
                {
                    StartCoroutine(StartShootingCoroutine(currentWeapon));
                }
            }
            else
            {
                if (!isShooting)
                {
                    StartShooting(currentWeapon);
                    isShooting = true; // 한 번만 발사
                }
            }
        }
        else
        {
            if (isShooting)
            {
                StopShooting();
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
        switch (currentWeaponType)
        {
            case WeaponType.MG:
                ShootMachineGun(weapon);
                break;
            case WeaponType.SG:
                ShootShotGun(weapon);
                break;
            case WeaponType.ML:
                ShootMissileLauncher(weapon);
                break;
        }
    }

    private void StopShooting()
    {
        if (isShooting)
        {
            StopAllCoroutines();
            isShooting = false;
        }
    }

    private void ShootMachineGun(GameObject weapon)
    {
        MachineGun mg = weapon.GetComponent<MachineGun>();
        foreach (Transform pos in shootPositions)
        {
            GameObject bullet = Instantiate(mg.Bullet, pos.position, Quaternion.identity);
            Bullet b = bullet.GetComponent<Bullet>();
            b.Prepare(this.gameObject, target.position, mg.Speed, mg.Range, mg.Damage);
            b.Shoot();
        }
    }

    private void ShootShotGun(GameObject weapon)
    {
        // ShotGun 발사 로직 구현
    }

    private void ShootMissileLauncher(GameObject weapon)
    {
        // MissileLauncher 발사 로직 구현
    }
}
