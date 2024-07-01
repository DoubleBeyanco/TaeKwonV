using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCannon : MonoBehaviour
{
    public PlayerHun player; // Player ������Ʈ�� Transform�� ����
    public float missileSpeed = 10f; // Missile�� �ӵ� (Inspector���� ���� ����)
    public float missileRange = 50f; // Missile�� ��Ÿ� (Inspector���� ���� ����)
    public GameObject missilePrefab; // �߻��� Missile ������ (Inspector���� ����)
    public float fireRate = 1f; // �̻��� �߻� ���� (�� ����, Inspector���� ���� ����)
    public int Hp = 100;
    private GameObject effect; // �߻� �� Ȱ��ȭ�� Effect ������Ʈ
    private Transform shootPoint; // Missile�� �߻�� ��ġ (Shoot ������Ʈ)
    private float missileLifetime; // Missile�� ���� �ð�
    private float nextFireTime = 0f; // ���� �߻� �ð�
    public bool activebool = false;

    void Start()
    {
        Hp = 100;
        // Effect ������Ʈ�� �ڽ� ������Ʈ���� ã��
        effect = transform.Find("Effect").gameObject;

        // Shoot ������Ʈ�� �ڽ� ������Ʈ���� ã�� shootPoint�� ����
        shootPoint = transform.Find("Shoot");

        missileLifetime = missileRange / missileSpeed; // ��Ÿ� ������� ���� �ð� ���
        if(player == null)
        {
            player = FindObjectOfType<PlayerHun>();
        }
    }
    public void TurretUpdate()
    {
        // Player�� ��ġ�� TurretCannon�� ��ġ���� ���� ���� ����
        if (player.transform.position.y >= transform.position.y)
        {
            // Player �������� ȸ��
            Vector3 direction = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
            float d = Vector3.Distance(player.transform.position, transform.position);

            // ���� �ð��� ���� �߻� �ð����� ũ�� �̻��� �߻�
            if (Time.time >= nextFireTime && d < missileRange)
            {
                FireMissile();
                nextFireTime = Time.time + fireRate; // ���� �߻� �ð� ����
            }
        }
        if(Hp <= 0 && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    void FireMissile()
    {
        // Missile �߻�
        GameObject missile = Instantiate(missilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody missileRb = missile.GetComponent<Rigidbody>();
        missileRb.velocity = shootPoint.forward * missileSpeed;

        // Effect Ȱ��ȭ
        effect.SetActive(true);

        // ���� �ð� �� Effect ��Ȱ��ȭ
        StartCoroutine(DisableEffect());

        // ���� �ð� �� Missile �ı�
        Destroy(missile, missileLifetime);
    }

    IEnumerator DisableEffect()
    {
        yield return new WaitForSeconds(0.1f); // Effect�� Ȱ��ȭ�Ǵ� �ð� (0.1�� �� ��Ȱ��ȭ)
        effect.SetActive(false);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnCollisionEnter(Collision collision)
    {
       /* if(collision.gameObject.CompareTag("Bullet"))
        {
            Hp--; �̷���ü�±���
        }*/
    }
}