using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject missilePrefab; // 발사할 Missile 프리팹
    public float shootInterval = 5f; // 발사 간격
    public float missileSpeed = 10f; // Missile 발사 속도

    private Transform player; // Player의 Transform

    void Start()
    {
        // Player의 Transform 가져오기
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 주기적으로 ShootMissile 메소드 호출
        InvokeRepeating("ShootMissile", shootInterval, shootInterval);
    }

    void ShootMissile()
    {
        if (player != null)
        {
            // Player를 바라보는 방향 벡터 계산
            Vector3 direction = (player.position - transform.position).normalized;

            // Missile 프리팹을 Player를 바라보는 방향으로 발사
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.LookRotation(direction));

            // Missile에 속도 설정
            missile.GetComponent<Rigidbody>().velocity = direction * missileSpeed;
        }
    }
}