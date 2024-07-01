using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCannon : MonoBehaviour
{
    public PlayerHun player; // Player 오브젝트의 Transform을 지정
    public float missileSpeed = 10f; // Missile의 속도 (Inspector에서 설정 가능)
    public float missileRange = 50f; // Missile의 사거리 (Inspector에서 설정 가능)
    public GameObject missilePrefab; // 발사할 Missile 프리팹 (Inspector에서 설정)
    public float fireRate = 1f; // 미사일 발사 간격 (초 단위, Inspector에서 설정 가능)
    public int Hp = 100;
    private GameObject effect; // 발사 시 활성화될 Effect 오브젝트
    private Transform shootPoint; // Missile이 발사될 위치 (Shoot 오브젝트)
    private float missileLifetime; // Missile의 생존 시간
    private float nextFireTime = 0f; // 다음 발사 시간
    public bool activebool = false;

    void Start()
    {
        Hp = 100;
        // Effect 오브젝트를 자식 오브젝트에서 찾음
        effect = transform.Find("Effect").gameObject;

        // Shoot 오브젝트를 자식 오브젝트에서 찾아 shootPoint로 설정
        shootPoint = transform.Find("Shoot");

        missileLifetime = missileRange / missileSpeed; // 사거리 기반으로 생존 시간 계산
        if(player == null)
        {
            player = FindObjectOfType<PlayerHun>();
        }
    }
    public void TurretUpdate()
    {
        // Player의 위치가 TurretCannon의 위치보다 높을 때만 실행
        if (player.transform.position.y >= transform.position.y)
        {
            // Player 방향으로 회전
            Vector3 direction = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
            float d = Vector3.Distance(player.transform.position, transform.position);

            // 현재 시간이 다음 발사 시간보다 크면 미사일 발사
            if (Time.time >= nextFireTime && d < missileRange)
            {
                FireMissile();
                nextFireTime = Time.time + fireRate; // 다음 발사 시간 갱신
            }
        }
        if(Hp <= 0 && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    void FireMissile()
    {
        // Missile 발사
        GameObject missile = Instantiate(missilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody missileRb = missile.GetComponent<Rigidbody>();
        missileRb.velocity = shootPoint.forward * missileSpeed;

        // Effect 활성화
        effect.SetActive(true);

        // 일정 시간 후 Effect 비활성화
        StartCoroutine(DisableEffect());

        // 일정 시간 후 Missile 파괴
        Destroy(missile, missileLifetime);
    }

    IEnumerator DisableEffect()
    {
        yield return new WaitForSeconds(0.1f); // Effect가 활성화되는 시간 (0.1초 후 비활성화)
        effect.SetActive(false);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnCollisionEnter(Collision collision)
    {
       /* if(collision.gameObject.CompareTag("Bullet"))
        {
            Hp--; 이렇게체력까면됨
        }*/
    }
}