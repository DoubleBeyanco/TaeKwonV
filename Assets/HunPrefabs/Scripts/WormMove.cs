using System.Collections;
using UnityEngine;

public class WormMove : MonoBehaviour
{
    WormMove[] worm;
    WhaleCenter[] whale;
    Vector3 randomMovement = Vector3.zero;
    Quaternion targetRotation;
    float targetZRotation;
    float animSpeed = 0;
    public bool Attack = false;
    public int hp = 5000;
    private int maxhealth = 5000;
    private int respawnCount = 2;

    private Animator anim;
    public GameObject wormMid;
    public GameObject wormSmall;

    public GameObject enemy;
    public GameObject razer;
    public GameObject bloom;
    public float minEnemyDis = 20;
    public float maxEnemyDis = 500;
    public float bressDis = 200;
    public float movementFrequency = 2; // ��鸲 �ֱ�
    public float movementAmplitude = 1f; // ��鸲 ����
    Vector3 movementOffset = Vector3.zero; // ��ǥ����

    // �̵� ���� ���� ����
    Vector3 referencePoint = new Vector3(0, 20000, 0);
    public float maxDistanceFromOrigin = 5000f; // �������κ��� �ִ� �Ÿ�

    public float minDistanceBetweenWorms = 50f; // �� ���� �ּ� �Ÿ�
    public float minDistanceBetweenWhale = 1500f; // ������ �ּ� �Ÿ�
    public float attackCancelDistance = 600f; // ���� ��� �Ÿ�

    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(RandVector());
        StartCoroutine(Rot());
        StartCoroutine(RandZRont());
        worm = FindObjectsOfType<WormMove>(); // �� �� ��� WormMove �ν��Ͻ��� �����ɴϴ�
        whale = FindObjectsOfType<WhaleCenter>();
        if (enemy == null)
        {
            PlayerHun enemyP = FindObjectOfType<PlayerHun>();
            enemy = enemyP.gameObject;
        }
    }

    private void Update()
    {
        bool anyWormAttacking = false;

        // � ���� ���� ������ Ȯ��
        for (int i = 0; i < worm.Length; i++)
        {
            if (worm[i] != null && worm[i].Attack)
            {
                anyWormAttacking = true;
                break;
            }
        }

        // ���� ���¿� ���� ���� �ൿ ������Ʈ
        for (int i = 0; i < worm.Length; i++)
        {
            if (worm[i] != null && worm[i].gameObject != this.gameObject)
            {
                float otherWormDistance = Vector3.Distance(worm[i].gameObject.transform.position, this.transform.position);

                if (otherWormDistance < minDistanceBetweenWorms && Attack)
                {
                    Attack = false;
                    StartCoroutine(RandVector());
                }

                // ���� ���� �ƴ� ��쿡�� �� �� �Ÿ� ����
                if (!Attack && !anyWormAttacking && otherWormDistance < minDistanceBetweenWorms)
                {
                    Vector3 directionAwayFromWorm = (this.transform.position - worm[i].gameObject.transform.position).normalized;
                    this.transform.position = worm[i].gameObject.transform.position + directionAwayFromWorm * minDistanceBetweenWorms;
                }
            }
        }
        for (int i = 0; i < whale.Length; i++)
        {
            if (whale[i] != null)
            {
                float whaleDistance = Vector3.Distance(whale[i].gameObject.transform.position, this.transform.position);
                if (whaleDistance < minDistanceBetweenWhale && Attack)
                {
                    Attack = false;
                    StartCoroutine(RandVector());
                }
                if (!Attack && whaleDistance < minDistanceBetweenWhale)
                {
                    Vector3 direction = (this.transform.position - whale[i].gameObject.transform.position).normalized;
                    this.transform.position = whale[i].gameObject.transform.position + direction * minDistanceBetweenWhale;
                }
            }
        }
        if (Attack == false && animSpeed > 0)
        {
            animSpeed -= 0.003f;
        }

        float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

        // ���� ���·� ��ȯ ����
        if (!anyWormAttacking && !Attack && distanceToEnemy < maxEnemyDis)
        {
            Attack = true;
            StopCoroutine(RandVector());
        }

        // ���� ���� ����� �ൿ
        if (Attack)
        {
            // ���� ��� ����
            if (distanceToEnemy > attackCancelDistance)
            {
                if (animSpeed > 0)
                {
                    animSpeed -= 0.003f;
                }
                else if (animSpeed <= 0)
                {
                    StartCoroutine(RandVector());
                    razer.SetActive(false);
                    Attack = false;
                }
            }

            if (hp > 30)
                randomMovement = (enemy.transform.position - transform.position).normalized;

            if (distanceToEnemy < minEnemyDis && hp > 30)
            {
                animSpeed += 0.003f;
                if (animSpeed >= 1.5f)
                {
                    animSpeed = 0;
                }
                else if (animSpeed >= 0.5f)
                {
                    animSpeed += 0.03f;
                }
            }
            else if (distanceToEnemy < bressDis && hp <= 30)
            {
                randomMovement = (enemy.transform.position - transform.position).normalized * Time.deltaTime;
                if (animSpeed <= 0.5f)
                {
                    animSpeed += 0.003f;
                }
                else if (animSpeed > 0.5f)
                {
                    animSpeed -= 0.003f;
                }
                if(razer.activeSelf == false && respawnCount == 2)
                {
                    razer.SetActive(true);
                }
                else if (razer.activeSelf == false && respawnCount == 1)
                {
                    razer.SetActive(true);
                    LineRenderer line = GetComponentInChildren<LineRenderer>();
                    line.startWidth = 20;
                    line.endWidth = 15;
                }
                else if(razer.activeSelf == false && respawnCount == 0)
                {
                    razer.SetActive(true);
                    LineRenderer line = GetComponentInChildren<LineRenderer>();
                    line.startWidth = 10;
                    line.endWidth = 5;
                }
            }
        }
        else if (distanceToEnemy > bressDis && hp <= 30)
        {
            randomMovement = (enemy.transform.position - transform.position).normalized * 2;
            if (animSpeed > 0)
            {
                animSpeed -= 0.003f;
            }
        }
        if (!Attack && hp <= 30)
        {
            razer.SetActive(false);
        }

        anim.SetFloat("WormAttack", animSpeed);


        movementOffset = new Vector3(
            Mathf.Sin(Time.time * movementFrequency) * movementAmplitude,
            Mathf.Sin(Time.time * movementFrequency * 0.5f) * movementAmplitude,
            0);

        transform.position += (randomMovement + movementOffset) * Time.deltaTime * 50; // �ӵ� ����

        // �̵� ���� ����
        Vector3 difference = transform.position - referencePoint;
        float distance = difference.magnitude;
        if (distance > maxDistanceFromOrigin)
        {
            Vector3 direction = (transform.position - referencePoint).normalized;
            transform.position = referencePoint + direction * maxDistanceFromOrigin;
        }

        if (randomMovement != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(randomMovement);
        }
        if (!bloom.activeSelf && hp <= 30)
        {
            bloom.SetActive(true);
            if (respawnCount == 2)
            {
                maxEnemyDis = 600;
                attackCancelDistance = 800;

            }
            else if (respawnCount == 1)
            {
                maxEnemyDis = 400;
                attackCancelDistance = 600;
            }
            else if (respawnCount == 0)
            {
                maxEnemyDis = 200;
                attackCancelDistance = 400;
            }
        }

        // ü���� 0 ������ �� ó��
        if (hp <= 0)
        {
            if (respawnCount == 2)
            {
                respawnCount--;
                for (int i = 0; i < 1; i++)
                {
                    GameObject newWorm = Instantiate(wormMid, transform.position, transform.rotation);
                    newWorm.GetComponent<WormMove>().respawnCount = respawnCount;
                    newWorm.GetComponent<WormMove>().hp = maxhealth - 2500;
                    newWorm.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    attackCancelDistance = attackCancelDistance - 200;
                    attackCancelDistance = 300;
                    bressDis = bressDis - 100;
                }
                Destroy(gameObject);
            }
            else if (respawnCount == 1)
            {
                respawnCount--;
                for (int i = 0; i < 2; i++)
                {
                    GameObject newWorm = Instantiate(wormSmall, transform.position, transform.rotation);
                    newWorm.GetComponent<WormMove>().respawnCount = respawnCount;
                    newWorm.GetComponent<WormMove>().hp = maxhealth - 4600;
                    newWorm.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    attackCancelDistance = attackCancelDistance - 400;
                    attackCancelDistance = 200;
                    bressDis = bressDis - 200;
                }
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }


    private IEnumerator RandVector()
    {
        while (true)
        {
            do
            {
                randomMovement = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f)
                ).normalized; // ���͸� ����ȭ�Ͽ� ������ �ӵ��� �̵�
            } while (randomMovement == Vector3.zero); // ���Ͱ� (0, 0, 0)�� �ƴϵ��� ����

            targetRotation = Quaternion.LookRotation(randomMovement);
            yield return new WaitForSeconds(10f); // �ð� ���� ����
        }
    }

    private IEnumerator RandZRont()
    {
        while (true)
        {
            targetZRotation = Random.Range(-360f, 360f);
            float randSecond = Random.Range(0, 2);
            yield return new WaitForSeconds(randSecond);
        }
    }

    private IEnumerator Rot()
    {
        while (true)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2f * Time.deltaTime);
            Quaternion currentRotation = transform.rotation;
            Vector3 euler = currentRotation.eulerAngles;
            euler.z = Mathf.LerpAngle(euler.z, targetZRotation, 2f * Time.deltaTime);
            transform.rotation = Quaternion.Euler(euler);
            yield return null;
        }
    }
}
