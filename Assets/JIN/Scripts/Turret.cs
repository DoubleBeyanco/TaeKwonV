using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target; // 회전할 대상 오브젝트
    public float rotationSpeed = 5f; // 회전 속도

    public GameObject missilePrefab; // 발사할 Missile 프리팹
    public float shootInterval = 5f; // 발사 간격
    public float missileSpeed = 10f; // Missile 발사 속도

    private Transform effect; // Effect 오브젝트를 위한 변수
    private bool isEffectActive = false; // Effect 활성 상태를 확인하는 변수

    void Start()
    {
        // Effect 오브젝트 찾기 (자식 중에서)
        effect = transform.Find("Effect");

        // 주기적으로 ShootMissile 메소드 호출
        InvokeRepeating("ShootMissile", shootInterval, shootInterval);
    }

    void Update()
    {
        if (target != null)
        {
            // 대상의 y 축 위치가 자신보다 높은지 여부를 확인
            if (target.position.y >= transform.position.y)
            {
                // 대상을 바라보기 위한 방향 벡터 계산
                Vector3 targetDirection = (target.position - transform.position).normalized;

                // 목표 회전 각도 계산 (Quaternion을 이용하여 자연스러운 회전 계산)
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // 현재 방향에서 목표 회전 방향까지의 회전을 부드럽게 적용
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            // 만약 Target 오브젝트가 자신보다 y 축 값이 낮은 위치에 있으면 바라보지 않음
        }
    }

    void ShootMissile()
    {
        if (target != null)
        {
            // 대상을 바라보는 방향 벡터 계산
            Vector3 direction = (target.position - transform.position).normalized;

            // Missile 프리팹을 대상을 바라보는 방향으로 발사
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.LookRotation(direction));

            // Missile에 속도 설정
            missile.GetComponent<Rigidbody>().velocity = direction * missileSpeed;

            // 5초 후에 Missile 파괴
            Destroy(missile, 5f);

            // Effect 오브젝트 활성화
            if (effect != null && !isEffectActive)
            {
                effect.gameObject.SetActive(true);
                isEffectActive = true; // Effect 활성 상태로 설정
                // 일정 시간 후 Effect 비활성화
                StartCoroutine(DisableEffectAfterDelay(1f));
            }
        }
    }

    IEnumerator DisableEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (effect != null)
        {
            effect.gameObject.SetActive(false);
            isEffectActive = false; // Effect 비활성 상태로 설정
        }
    }
}