using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public interface IWeapon
{
    [Flags]
    public enum WeaponType
    {
        None = 0,
        MG = 1 << 0,
        SG = 1 << 1,
        ML = 1 << 2,
    }

    public WeaponType CurWeaponType { get { return CurWeaponType; } set { CurWeaponType = value; } }
    private int Range { get { return Range; } set { Range = value; } }
    private VisualEffectAsset Effect { get { return Effect; } set { Effect = value; } }
    public float Damage { get { return Damage; } set { Damage = value; } }
    private GameObject Bullet { get { return Bullet; } set { Bullet = value; } }
    public void Fire();
    public void ViveratePattern();
}
    
