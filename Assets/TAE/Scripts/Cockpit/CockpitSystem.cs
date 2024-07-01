using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class CockpitSystem : MonoBehaviour
{
    public enum WeaponType
    {
        MG,
        SG,
        ML,
    }
    [SerializeField] private InputData input;

    private CockpitJoystick stick;
    private AimingSystem aim;
    private WeaponSystem weapon;

    private WeaponType currentWeaponType;
    private GameObject currentWeapon;

    private bool PrimaryButton;
    private bool GrabButton;

    private void Awake()
    {
        stick = GetComponentInChildren<CockpitJoystick>();
        aim = GetComponentInChildren<AimingSystem>();
        weapon = GetComponentInChildren<WeaponSystem>();
    }

    private void Start()
    {
        currentWeaponType = WeaponType.SG; // 기본 무기 타입 설정
        currentWeapon = weapon.GetWeapon(currentWeaponType);
        stick.AimingModeCallback = AimMode;

        input._rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool gripValue);
        input._rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryValue);
        PrimaryButton = primaryValue;
        GrabButton = gripValue;
    }
    private void Update()
    {
        if (!PrimaryButton || !GrabButton)
        {
            weapon.StopShooting();
        }
    }

    public void SwitchWeapon(WeaponType newWeaponType)
    {
        currentWeaponType = newWeaponType;
        currentWeapon = weapon.GetWeapon(newWeaponType);
    }

    private void AimMode(bool isAiming)
    {
        int range = weapon.GetWeaponRange(currentWeaponType);

        aim.AimCalc(stick.transform.eulerAngles, range);
        weapon.ShootingActive(isAiming);
    }
}
