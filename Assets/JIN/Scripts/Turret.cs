using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target; // ȸ���� ��� ������Ʈ
    public float rotationSpeed = 5f; // ȸ�� �ӵ�

    public GameObject missilePrefab; // �߻��� Missile ������
    public float shootInterval = 5f; // �߻� ����
    public float missileSpeed = 10f; // Missile �߻� �ӵ�

    private Transform effect; // Effect ������Ʈ�� ���� ����
    private bool isEffectActive = false; // Effect Ȱ�� ���¸� Ȯ���ϴ� ����

    void Start()
    {
        // Effect ������Ʈ ã�� (�ڽ� �߿���)
        effect = transform.Find("Effect");

        // �ֱ������� ShootMissile �޼ҵ� ȣ��
        InvokeRepeating("ShootMissile", shootInterval, shootInterval);
    }

    void Update()
    {
        if (target != null)
        {
            // ����� y �� ��ġ�� �ڽź��� ������ ���θ� Ȯ��
            if (target.position.y >= transform.position.y)
            {
                // ����� �ٶ󺸱� ���� ���� ���� ���
                Vector3 targetDirection = (target.position - transform.position).normalized;

                // ��ǥ ȸ�� ���� ��� (Quaternion�� �̿��Ͽ� �ڿ������� ȸ�� ���)
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // ���� ���⿡�� ��ǥ ȸ�� ��������� ȸ���� �ε巴�� ����
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            // ���� Target ������Ʈ�� �ڽź��� y �� ���� ���� ��ġ�� ������ �ٶ��� ����
        }
    }

    void ShootMissile()
    {
        if (target != null)
        {
            // ����� �ٶ󺸴� ���� ���� ���
            Vector3 direction = (target.position - transform.position).normalized;

            // Missile �������� ����� �ٶ󺸴� �������� �߻�
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.LookRotation(direction));

            // Missile�� �ӵ� ����
            missile.GetComponent<Rigidbody>().velocity = direction * missileSpeed;

            // 5�� �Ŀ� Missile �ı�
            Destroy(missile, 5f);

            // Effect ������Ʈ Ȱ��ȭ
            if (effect != null && !isEffectActive)
            {
                effect.gameObject.SetActive(true);
                isEffectActive = true; // Effect Ȱ�� ���·� ����
                // ���� �ð� �� Effect ��Ȱ��ȭ
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
            isEffectActive = false; // Effect ��Ȱ�� ���·� ����
        }
    }
}