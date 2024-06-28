using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool isShoot = false;
    private GameObject owner;
    private Vector3 direction;
    private float speed;
    private float range;
    private float bullet_DMG;
    private Vector3 startPosition;

    private void Update()
    {
        if (isShoot)
        {
            MoveBullet();
            CheckRange();
        }
    }

    private void MoveBullet()
    {
        this.transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void CheckRange()
    {
        if (Vector3.Distance(startPosition, this.transform.position) >= range)
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
 
    }

    public void Prepare(GameObject _owner, Vector3 _targetPosition, float _speed, float _range, float _damage)
    {
        owner = _owner;
        direction = (_targetPosition - this.transform.position).normalized; // 타겟 방향 계산
        speed = _speed;
        range = _range / 2;
        bullet_DMG = _damage;
        startPosition = this.transform.position; // 시작 위치 저장
    }

    public void Shoot()
    {
        isShoot = true;
    }
}
