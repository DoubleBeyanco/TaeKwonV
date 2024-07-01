using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public Transform target; // ȸ���� ��� ������Ʈ
    public float rotationSpeed = 5f; // ȸ�� �ӵ�
    public GameObject machineGun; // MachineGun ������Ʈ
    public GameObject windCutter; // WindCutter ������Ʈ
    public float activationDistance = 100f; // Ȱ��ȭ/��Ȱ��ȭ ���� �Ÿ�

    private bool machineGunActive = false; // MachineGun�� ���� ����
    private bool windCutterActive = false; // WindCutter�� ���� ����

    void Update()
    {
        if (target != null)
        {
            // ����� y �� ��ġ�� �ڽź��� ������ ���θ� Ȯ��
            if (target.position.y >= transform.position.y)
            {
                // ����� �ٶ󺸱� ���� ���� ���� ��� (y�� ȸ���� ���)
                Vector3 targetDirection = (target.position - transform.position).normalized;

                // y�� ȸ������ ����� ���� ����
                targetDirection.y = 0;

                // ��ǥ ȸ�� ���� ��� (Quaternion�� �̿��Ͽ� �ڿ������� ȸ�� ���)
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // ���� ���⿡�� ��ǥ ȸ�� ��������� ȸ���� �ε巴�� ����
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Ÿ�ٰ��� �Ÿ� ���
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // MachineGun�� Ȱ��ȭ/��Ȱ��ȭ ó��
            if (machineGun != null)
            {
                if (distanceToTarget >= activationDistance)
                {
                    if (!machineGunActive)
                    {
                        machineGun.SetActive(true);

                        // Ư�� ������Ʈ�� ParticleSystem�� ������ ���� ��, Effect�� ���
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

            // WindCutter�� Ȱ��ȭ/��Ȱ��ȭ ó��
            if (windCutter != null)
            {
                if (distanceToTarget < activationDistance)
                {
                    if (!windCutterActive)
                    {
                        windCutter.SetActive(true);

                        // Ư�� ������Ʈ�� ParticleSystem�� ������ ���� ��, Effect�� ���
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