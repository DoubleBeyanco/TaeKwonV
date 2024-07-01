using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public Transform target; // 회전할 대상 오브젝트
    public float rotationSpeed = 5f; // 회전 속도
    public GameObject machineGun; // MachineGun 오브젝트
    public GameObject windCutter; // WindCutter 오브젝트
    public float activationDistance = 100f; // 활성화/비활성화 기준 거리

    private bool machineGunActive = false; // MachineGun의 현재 상태
    private bool windCutterActive = false; // WindCutter의 현재 상태

    void Update()
    {
        if (target != null)
        {
            // 대상의 y 축 위치가 자신보다 높은지 여부를 확인
            if (target.position.y >= transform.position.y)
            {
                // 대상을 바라보기 위한 방향 벡터 계산 (y축 회전만 고려)
                Vector3 targetDirection = (target.position - transform.position).normalized;

                // y축 회전만을 고려한 방향 설정
                targetDirection.y = 0;

                // 목표 회전 각도 계산 (Quaternion을 이용하여 자연스러운 회전 계산)
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // 현재 방향에서 목표 회전 방향까지의 회전을 부드럽게 적용
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // 타겟과의 거리 계산
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // MachineGun의 활성화/비활성화 처리
            if (machineGun != null)
            {
                if (distanceToTarget >= activationDistance)
                {
                    if (!machineGunActive)
                    {
                        machineGun.SetActive(true);

                        // 특정 오브젝트가 ParticleSystem을 가지고 있을 때, Effect를 재생
                        ParticleSystem ps = machineGun.GetComponent<ParticleSystem>();
                        if (ps != null)
                        {
                            ps.Play();
                        }
                        machineGunActive = true;
                    }
                }
                else
                {
                    if (machineGunActive)
                    {
                        machineGun.SetActive(false);
                        machineGunActive = false;
                    }
                }
            }

            // WindCutter의 활성화/비활성화 처리
            if (windCutter != null)
            {
                if (distanceToTarget < activationDistance)
                {
                    if (!windCutterActive)
                    {
                        windCutter.SetActive(true);

                        // 특정 오브젝트가 ParticleSystem을 가지고 있을 때, Effect를 재생
                        ParticleSystem ps = windCutter.GetComponent<ParticleSystem>();
                        if (ps != null)
                        {
                            ps.Play();
                        }
                        windCutterActive = true;
                    }
                }
                else
                {
                    if (windCutterActive)
                    {
                        windCutter.SetActive(false);
                        windCutterActive = false;
                    }
                }
            }
        }
    }
}