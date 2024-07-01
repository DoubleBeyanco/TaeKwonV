using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject missilePrefab; // �߻��� Missile ������
    public float shootInterval = 5f; // �߻� ����
    public float missileSpeed = 10f; // Missile �߻� �ӵ�

    private Transform player; // Player�� Transform

    void Start()
    {
        // Player�� Transform ��������
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // �ֱ������� ShootMissile �޼ҵ� ȣ��
        InvokeRepeating("ShootMissile", shootInterval, shootInterval);
    }

    void ShootMissile()
    {
        if (player != null)
        {
            // Player�� �ٶ󺸴� ���� ���� ���
            Vector3 direction = (player.position - transform.position).normalized;

            // Missile �������� Player�� �ٶ󺸴� �������� �߻�
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.LookRotation(direction));

            // Missile�� �ӵ� ����
            missile.GetComponent<Rigidbody>().velocity = direction * missileSpeed;
        }
    }
}