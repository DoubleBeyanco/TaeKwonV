using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private MachineGun MG;
    [SerializeField] private ShotGun SG;
    [SerializeField] private MissileLauncher ML;
    [HideInInspector] public IWeapon.WeaponType curweapon;

    private GameObject[] weaponobj;

    public GameObject[] GetWeaponObject()
    {
        List<GameObject> temp = new List<GameObject>();
        temp.Add(MG.gameObject);
        temp.Add(SG.gameObject);
        temp.Add(ML.gameObject);
        return temp.ToArray();
    }
    public void StartShooting()
    {
        ShootTarget();
    }

    public void TrackingTarget()
    {
        transform.LookAt(target);
    }

    private void ShootTarget()
    {
        switch (curweapon)
        {
            case IWeapon.WeaponType.MG:
                MG.Fire();
                break;
            case IWeapon.WeaponType.SG:
                SG.Fire();
                break;
            case IWeapon.WeaponType.ML:
                ML.Fire();
                break;
        }
    }
}
