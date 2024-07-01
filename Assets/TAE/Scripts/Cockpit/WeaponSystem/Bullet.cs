using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    private CockpitSystem.WeaponType CurBulletType;
    private Rigidbody rb;
    private BoxCollider col;
    private VisualEffect effect;
    private GameObject owner;
    private Vector3 direction;
    private Vector3 startPosition;
    private float speed;
    private float range;
    private float bullet_DMG;
    private Transform target;
    private float rotateSpeed = 5f;

    private bool isShoot = false;
    private bool isLauched = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        effect = GetComponent<VisualEffect>();
    }

    private void Update()
    {
        if (CurBulletType == CockpitSystem.WeaponType.MG && isShoot)
        {
            effect.Play();
            MoveBullet();
            CheckRange();
        }
        else if (CurBulletType == CockpitSystem.WeaponType.SG && isShoot)
        {
            effect.Stop();
            ShotGunBullet();
            MoveBullet();
            CheckRange();
        }
        else if (CurBulletType == CockpitSystem.WeaponType.ML && !isLauched && isShoot)
        {
            effect.Stop();
            isLauched = true;
            StartCoroutine(HomingStart(target));
            Invoke("DestroyBullet", 20f);
        }
    }

    private void MoveBullet()
    {
        this.transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void ShotGunBullet()
    {
        col.size = Vector3.Lerp(this.col.size, new Vector3(3, 3, this.col.size.z), Time.deltaTime);
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
        DestroyBullet();
    }
    public void Shoot()
    {
        isShoot = true;
    }

    public void Prepare(CockpitSystem.WeaponType _curType, GameObject _owner, Vector3 _targetPosition, float _speed, float _range, float _damage)
    {

        CurBulletType = _curType;
        owner = _owner;
        direction = (_targetPosition - this.transform.position).normalized; // 타겟 방향 계산
        speed = _speed;
        range = _range / 2;
        bullet_DMG = _damage;
        startPosition = this.transform.position; // 시작 위치 저장
    }

    public void MissilePrepare(float _rotateSpeed, Transform _target)
    {
        rotateSpeed = _rotateSpeed;
        target = _target;
    }


    private IEnumerator HomingStart(Transform _target)
    {
        yield return new WaitForSeconds(3f);
        while (true)
        {

            if (_target == null)
            {
                yield break;
            }
            Vector3 targetDirection = (_target.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);

            rb.MoveRotation(newRotation);
            rb.velocity = transform.forward * speed;

            yield return new WaitForEndOfFrame();
        }
    }
}
