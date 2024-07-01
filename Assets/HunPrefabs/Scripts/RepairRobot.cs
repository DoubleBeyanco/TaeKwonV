using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairRobot : MonoBehaviour
{
    private GameObject[] repairTargets;
    private RepairRobot[] otherRobots;
    private GameObject closestTarget;
    public int Hp = 1;

    // 이동 속도
    public float speed = 5f;

    private void Start()
    {
        otherRobots = FindObjectsOfType<RepairRobot>();
    }

    // Update에서 지속적으로 호출하여 이동
    void Update()
    {
        if (closestTarget != null && closestTarget.activeSelf)
        {
            Vector3 direction = (closestTarget.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, closestTarget.transform.position);
            if (distance > 2)
            {
                transform.LookAt(closestTarget.transform.position);
                transform.position += direction * speed * Time.deltaTime;
            }
        }
        if(Hp <= 0 && gameObject.activeSelf)
        {
            closestTarget = null;
            gameObject.SetActive(false);
        }
    }

    public void RepairStart()
    {
        StartCoroutine(Repair());
    }

    public void StopRobot()
    {
        StopAllCoroutines();
    }

    // 가장 가까운 타겟 찾기 및 이동 시작
    private IEnumerator Repair()
    {
        while (true)
        {
            // "Piece" 태그가 붙은 모든 오브젝트 찾기
            repairTargets = GameObject.FindGameObjectsWithTag("Piece");

            // 가장 가까운 타겟 초기화
            closestTarget = null;
            float shortestDistance = Mathf.Infinity;

            // 모든 타겟을 순회하며 가장 가까운 타겟 찾기
            foreach (GameObject target in repairTargets)
            {
                if (!target.activeSelf) continue;

                float distance = Vector3.Distance(transform.position, target.transform.position);
                bool isTargetTaken = false;

                foreach (RepairRobot robot in otherRobots)
                {
                    if (robot != this && robot.ClosestTarget() == target)
                    {
                        isTargetTaken = true;
                        break;
                    }
                }

                if (!isTargetTaken && distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestTarget = target;
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public GameObject ClosestTarget()
    {
        return closestTarget;
    }

    public GameObject[] RepairTargets()
    {
        return repairTargets;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Piece"))
        {
            StartCoroutine(RepairAndDeactivate(other));
        }
    }

    private IEnumerator RepairAndDeactivate(Collider other)
    {
        yield return new WaitForSeconds(2);
        if (other.gameObject.activeSelf)
        {
            other.gameObject.SetActive(false);
            closestTarget = null; // 현재 타겟이 비활성화되면 새로운 타겟을 찾도록 합니다.
        }
    }
}
