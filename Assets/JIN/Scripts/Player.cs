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
        // ó������ Boss ������ �ٶ󺸵��� ����
        LookAtBossPosition();
    }

    void Update()
    {
        if (!movingAway)
        {
            // ���� �ð� �Ŀ� �־������� ����
            StartCoroutine(MoveAwayAfterDelay(2f));
        }
        else
        {
            // Boss�� �ٶ󺸸� �̵�
            Vector3 directionToBoss = (boss.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToBoss);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 500f * Time.deltaTime);

            // ��ǥ ������ �������� ��
            if (transform.position == targetPosition)
            {
                movingAway = false;
                LookAtBossPosition(); // �ٽ� Boss�� �ٶ󺸵��� ȸ��
            }
        }
    }

    // Boss�� �ٶ󺸴� ȸ�� �Լ�
    void LookAtBossPosition()
    {
        Vector3 direction = (boss.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // ���� �ð� �Ŀ� Player�� �־����� �ϴ� �ڷ�ƾ
    private IEnumerator MoveAwayAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Random ��ġ ���
        float distance = 1200f;
        Vector3 directionToBoss = (transform.position - boss.position).normalized;
        targetPosition = boss.position + directionToBoss * distance;

        movingAway = true;
    }
}