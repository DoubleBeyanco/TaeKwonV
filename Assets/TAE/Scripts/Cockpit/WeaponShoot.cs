using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponShoot : MonoBehaviour
{
    public enum WeaponType 
    {
        MG,
        SG,
        HM
    }

    [SerializeField] VisualEffect[] effects;
    [SerializeField] Transform target;

    private WeaponType curWeapon = WeaponType.MG;
    public WeaponType CurWeapon
    {
        get { return curWeapon; }
        set { curWeapon = value; }
    }
    private void Awake()
    {
        foreach(VisualEffect e in effects)
        {
            e.Stop();
        }
        
    }

    public void ActiveShooting()
    {
        ShootTarget();
    }

    private void ShootTarget()
    {
        GameObject gun = effects[(int)curWeapon].gameObject;
        gun.transform.LookAt(target);
        effects[(int)curWeapon].Play();

    }
}
