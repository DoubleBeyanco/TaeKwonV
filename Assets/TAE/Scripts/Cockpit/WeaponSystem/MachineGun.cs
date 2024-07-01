using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour, IWeapon
{
    public int Range = 300;
    public float Damage = 1f;
    public float Speed = 100f;
    public bool ContinuousFire = true;
    public GameObject Bullet;

    public void Fire(Transform[] shootPositions, Transform target)
    {
        
        foreach (Transform pos in shootPositions)
        {
            GameObject bullet = Instantiate(Bullet, pos.position, Quaternion.identity);
            Bullet b = bullet.GetComponent<Bullet>();
            b.Prepare(CockpitSystem.WeaponType.MG, this.gameObject, target.position, Speed, Range, Damage);
            b.Shoot();
        }
    }
    public void VivePattern(InputData _controller)
    {
        // 진동 구현해야 함.
    }
}
