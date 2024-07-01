using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleShip : MonoBehaviour
{
    public int HP;
    public float minDistanceBetweenShips = 200f;  // ��Ʋ�� �� �ּ� �Ÿ�
    public float irregularMovementInterval = 2f;  // �ұ�Ģ ������ ����
    private Vector3 irregularOffset;
    private float nextIrregularMove;
    private Animator animator;
    private RepairRobot[] robots;
    public GameObject player;
    private BattleShipRazer razer;
    public float attackRange = 50f;
    public float chaseRange = 1000f;
    public float minChaseRange = 800f;
    public float orbitSpeed = 20.0f;  // ���� �ӵ�
    public float moveSpeed = 5f;
    public float rotSpeed = 10f;
    public TurretCannon[] turrets;
    private bool explode = false;
    private int initialHP;
    private int pieceActive;
    private int repairCount = 0;
    private Vector3 previousPosition = Vector3.zero;
    private bool isChasing = false;
    float yset = 0f;

    void Start()
    {
        HP = 1000; // �Լ��� �ʱ� ü�� ����
        initialHP = HP;
        animator = GetComponent<Animator>();
        robots = GetComponentsInChildren<RepairRobot>();
        turrets = GetComponentsInChildren<TurretCannon>();
        pieceActive = 1000;
        foreach (RepairRobot robot in robots)
        {
            robot.gameObject.SetActive(false);
        }
        razer = GetComponentInChildren<BattleShipRazer>();
        StartCoroutine(YSet());

    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange && isChasing && !explode)
        {
            AttackPlayer();
        }
        if (distanceToPlayer <= chaseRange && !explode)
        {
            isChasing = true;
        }
        else if(!explode)
        {
            LookAtPlayer();
        }

        if(isChasing && !explode)
        {
            ChasePlayer();
        }
        if (HP <= 0 && explode && repairCount == 0)
        {
            PieceActive();
        }
        if (HP <= 0 && !explode)
        {
            foreach (RepairRobot robot in robots)
            {
                robot.gameObject.SetActive(true);
            }
            animator.SetBool("Reset", false);
            animator.SetBool("Explode", true);
            foreach (RepairRobot robot in robots)
            {
                robot.RepairStart();
            }
            explode = true;
        }
        for(int i = 0; i < turrets.Length; i++) 
        {
            if (!turrets[i].gameObject.activeSelf && !turrets[i].activebool) 
            {
                DamageWeakPoint();
                turrets[i].activebool = true;
            }
            if(turrets[i].activebool && explode)
            {
                turrets[i].gameObject.SetActive(true);
                turrets[i].activebool = false;
            }
        }
    }

    private void AttackPlayer()
    {
        float d = Vector3.Distance(player.transform.position, transform.position);
        // �÷��̾ ���� ���� ���� ���� �� ����
        if (d <= attackRange)
        {

            if (razer.gameObject.activeSelf)
            {
                razer.gameObject.SetActive(false);
            }
            foreach (TurretCannon turret in turrets)
            {
                turret.missileRange = attackRange;
                turret.TurretUpdate();
            }
        }
    }

    private void ChasePlayer()
    {

        if (Vector3.Distance(transform.position, player.transform.position) <= minChaseRange)
        {
            // Ÿ�� ������ �����ϴ� ����
            Vector3 orbitAxis = Vector3.up;

            // �ұ�Ģ���� ������ �߰�
            if (Time.time >= nextIrregularMove)
            {
                irregularOffset = Random.insideUnitSphere * 100f;  // �ұ�Ģ�� ������ ����
                irregularOffset.y = 0;  // Y�� ���� ����
                nextIrregularMove = Time.time + irregularMovementInterval;
            }

            // �ٸ� ��Ʋ������ �Ÿ� Ȯ�� �� ����
            Vector3 avoidanceVector = AvoidOtherBattleships();

            // �÷��̾� ������ �����ϸ鼭 �ұ�Ģ���� �����Ӱ� ȸ�� ���� ����
            transform.RotateAround(player.transform.position + irregularOffset, orbitAxis, orbitSpeed * Time.deltaTime);
            transform.position += avoidanceVector * Time.deltaTime;

            // Y�� �� ���� (õõ�� �̵�)
            Vector3 targetPosition = transform.position;
            targetPosition.y = player.transform.position.y - yset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);

            // ���� �̵� ���� ��� �� �ٶ󺸱�
            Vector3 directionToOrbit = (transform.position - previousPosition).normalized;
            if (directionToOrbit != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToOrbit, Vector3.up);
                Quaternion targetRot = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
            }

            // ���� ��ġ�� ���� ��ġ�� ����
            previousPosition = transform.position;
        }
        else
        {
            // �ּ� ���� �Ÿ����� ���� �������� �ʾ��� ���, ������ ���� ������ ����
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            Vector3 targetPosition = player.transform.position - directionToPlayer * minChaseRange;
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Y�� �� ���� (õõ�� �̵�)
            newPosition.y = Mathf.Lerp(transform.position.y, player.transform.position.y - yset, 0.1f * Time.deltaTime);
            transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            Quaternion targetRot = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); // X���� ����
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
        }
    }
    private Vector3 AvoidOtherBattleships()
    {
        Vector3 avoidanceVector = Vector3.zero;
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, minDistanceBetweenShips);

        foreach (Collider col in nearbyObjects)
        {
            if (col.gameObject != gameObject && col.gameObject.CompareTag("BattleShip"))
            {
                Vector3 awayFromOther = transform.position - col.transform.position;
                float distance = awayFromOther.magnitude;

                if (distance < minDistanceBetweenShips)
                {
                    avoidanceVector += awayFromOther.normalized * (minDistanceBetweenShips - distance);
                }
            }
        }

        return avoidanceVector;
    }
    private IEnumerator YSet()
    {
        yset = Random.Range(0, 1000);
        yield return new WaitForSeconds(10);
    }
    private void LookAtPlayer()
    {
        // �÷��̾ �ٶ󺸵��� �Լ��� ������ �����ϴ� ����
        Vector3 d = new Vector3(player.transform.position.x, player.transform.position.y - 110, player.transform.position.z);
        Vector3 direction = (d - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed); // ȸ�� �ӵ� ����
    }

    private void PieceActive()
    {
        if (robots.Length != 0)
        {

            if (robots[0].RepairTargets().Length != 0)
            {
                GameObject[] repairTargets = robots[0].RepairTargets();

                pieceActive = repairTargets.Length;
                foreach (GameObject repairTarget in repairTargets)
                {
                    if (!repairTarget.gameObject.activeSelf)
                    {
                        pieceActive--;
                    }
                    if (pieceActive == 0)
                    {
                        foreach (RepairRobot robot in robots)
                        {
                            robot.StopRobot();
                            robot.gameObject.SetActive(false);
                        }
                        animator.SetBool("Explode", false);
                        animator.SetBool("Reset", true);
                        HP = initialHP;
                        repairCount++;
                        explode = false;
                    }
                }
            }
        }
    }

    public void DamageWeakPoint()
    {
        HP -= (initialHP * 10) / 100;
        if (HP < 0) HP = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
       /*if( other.gameObject.CompareTag("Bullet"))
        {
            HP--; �̷�������ü�±���
        }*/
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BattleShip"))
        {
            // �浹�� ��Ʋ������ ���� ���� ���
            Vector3 direction = (transform.position - collision.gameObject.transform.position).normalized;

            // ƨ��� �ӵ� ��� (������ ���� ����)
            float bounceForce = 10.0f;

            // �ڽ��� �ݴ� �������� ƨ��� ���� ���� �̵�
            GetComponent<Rigidbody>().AddForce(direction * bounceForce, ForceMode.Impulse);
        }
    }
}
