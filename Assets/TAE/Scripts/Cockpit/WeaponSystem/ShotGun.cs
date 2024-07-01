using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class ShotGun : MonoBehaviour, IWeapon
{
    public int Range = 150;
    public float Damage = 1f;
    public float Speed = 1f;
    public GameObject Bullet;
    public bool ContinuousFire = false;

    public void Fire(Transform[] shootPositions, Transform target)
    {
        GameObject bullet = Instantiate(Bullet, shootPositions[1].position, Quaternion.identity);
        Bullet b = bullet.GetComponent<Bullet>();
        b.Prepare(CockpitSystem.WeaponType.SG, this.gameObject, target.position, Speed, Range, Damage);
        b.Shoot();
    }

    public void VivePattern(InputData _controller)
    {
        // 진동 구현해야 함.
    }
}
