using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitSystem : MonoBehaviour
{
    // 인터페이스가 아니라 Scriptable Object로 바꿔보자.
    private IWeapon.WeaponType curWeaponType;
    private GameObject[] WeaponList;
    private WeaponSystem weapon;
    private CockpitJoystick stick;
    private AimmingSystem aim;
    private GameObject CurWeapon;

    private bool wasTriggerPressed = false; // Track the previous state of the trigger button


    private void Awake()
    {
        weapon = GetComponentInChildren<WeaponSystem>();
        stick = GetComponentInChildren<CockpitJoystick>();
        aim = GetComponentInChildren<AimmingSystem>();
    }

    private void Start()
    {
        stick.AimmingModeCallback = AimMode;
        WeaponList = weapon.GetWeaponObject();
        curWeaponType = IWeapon.WeaponType.MG;
        weapon.curweapon = curWeaponType;

        
    }

    private void AimMode(bool _value)
    {
        aim.actualAimCalc(stick.transform.eulerAngles, 100);
        ShootingActive(_value);
    }

    private void ShootingActive(bool _trigger)
    {
        weapon.TrackingTarget();

        if (_trigger && !wasTriggerPressed)
        {
            // Trigger was just pressed
            weapon.StartShooting();
        }
        // Update the previous state of the trigger
        wasTriggerPressed = _trigger;
    }
}
