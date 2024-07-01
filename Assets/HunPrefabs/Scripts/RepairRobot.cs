using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairRobot : MonoBehaviour
{
    private GameObject[] repairTargets;
    private RepairRobot[] otherRobots;
    private GameObject closestTarget;
    public int Hp = 1;

    // �̵� �ӵ�
    public float speed = 5f;

    private void Start()
    {
        otherRobots = FindObjectsOfType<RepairRobot>();
    }

    // Update���� ���������� ȣ���Ͽ� �̵�
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

    // ���� ����� Ÿ�� ã�� �� �̵� ����
    private IEnumerator Repair()
    {
        while (true)
        {
            // "Piece" �±װ� ���� ��� ������Ʈ ã��
            repairTargets = GameObject.FindGameObjectsWithTag("Piece");

            // ���� ����� Ÿ�� �ʱ�ȭ
            closestTarget = null;
            float shortestDistance = Mathf.Infinity;

            // ��� Ÿ���� ��ȸ�ϸ� ���� ����� Ÿ�� ã��
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
            closestTarget = null; // ���� Ÿ���� ��Ȱ��ȭ�Ǹ� ���ο� Ÿ���� ã���� �մϴ�.
        }
    }
}
