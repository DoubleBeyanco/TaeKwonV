using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform boss;
    private bool movingAway = false;
    private Vector3 targetPosition;

    void Start()
    {
        // 처음에는 Boss 방향을 바라보도록 설정
        LookAtBossPosition();
    }

    void Update()
    {
        if (!movingAway)
        {
            // 일정 시간 후에 멀어지도록 설정
            StartCoroutine(MoveAwayAfterDelay(2f));
        }
        else
        {
            // Boss를 바라보며 이동
            Vector3 directionToBoss = (boss.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToBoss);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 500f * Time.deltaTime);

            // 목표 지점에 도착했을 때
            if (transform.position == targetPosition)
            {
                movingAway = false;
                LookAtBossPosition(); // 다시 Boss를 바라보도록 회전
            }
        }
    }

    // Boss를 바라보는 회전 함수
    void LookAtBossPosition()
    {
        Vector3 direction = (boss.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // 일정 시간 후에 Player를 멀어지게 하는 코루틴
    private IEnumerator MoveAwayAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Random 위치 계산
        float distance = 1200f;
        Vector3 directionToBoss = (transform.position - boss.position).normalized;
        targetPosition = boss.position + directionToBoss * distance;

        movingAway = true;
    }
}