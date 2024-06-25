using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MissileLauncher : MonoBehaviour, IWeapon
{
    [SerializeField] private int Range = 100;
    [SerializeField] private VisualEffectAsset Effect;
    [SerializeField] private GameObject Bullet;
    [HideInInspector] public IWeapon.WeaponType CurWeaponType = IWeapon.WeaponType.ML;
    public float Damage = 10f;
    private GameObject owner;

    private void Awake()
    {
        owner = transform.parent.parent.parent.gameObject;
    }

    public void Fire()
    {
        GameObject go = Instantiate(Bullet);
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.VFX.visualEffectAsset = Effect;
        bullet.Bullet_DMG = Damage;
        bullet.Owner = owner;
        bullet.Shoot(owner, this.transform.forward);

    }

    public void ViveratePattern()
    {

    }

    private void HomingTracking()
    {

    }
}
