using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MissileLauncher : MonoBehaviour, IWeapon
{
    public int Range = 300;
    public float Damage = 1f;
    public float Speed = 1f;
    public float RotateSpeed = 5f;
    public bool ContinuousFire = false;
    public GameObject Bullet;

    private Transform Target;

    public IEnumerator LockOn()
    {
        yield return null;
    }

    public void Fire(Transform[] shootPositions, Transform target)
    {
        GameObject bullet = Instantiate(Bullet, shootPositions[1].position, Quaternion.identity);
        Bullet b = bullet.GetComponent<Bullet>();
        b.Prepare(CockpitSystem.WeaponType.ML, this.gameObject, target.position, Speed, Range, Damage);
        b.MissilePrepare(RotateSpeed, Target);
        b.Shoot();
    }

    public void VivePattern(InputData _controller)
    {
        // 진동 구현해야 함.
    }
}
