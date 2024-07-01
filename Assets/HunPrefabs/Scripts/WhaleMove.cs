using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleMove : MonoBehaviour
{
    public float speed = 5f;
    public Animator animator;
    public float rotationSpeed = 5f;
    public float detectionRadius = 1000f;
    public float changeTargetInterval = 5f;
    public Collider attackTrigger;

    private Rigidbody rb;
    private Transform target;
    private bool isMoving = false;
    private bool isAttacking = false;
    private Transform currentTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.position = new Vector3(-1000, 22000, 0);
        StartCoroutine(ChangeTargetPositionRoutine());
        attackTrigger.enabled = false;
    }

    void Update()
    {
        if (isMoving && target != null && !isAttacking)
        {
            MoveTowardsTarget();
        }
    }

    IEnumerator ChangeTargetPositionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeTargetInterval);
            if (!isAttacking) // 공격 중이 아닐 때만 타겟을 변경
            {
                SetNewTargetPosition();
            }
        }
    }

    void SetNewTargetPosition()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        float closestDistance = detectionRadius;
        Transform closestTarget = null;

        foreach (var hitCollider in hitColliders)
        {
            UFOBody ufoBody = hitCollider.GetComponent<UFOBody>();
            if (ufoBody != null)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = hitCollider.transform;
                }
            }
        }

        if (closestTarget != null)
        {
            target = closestTarget;
            isMoving = true;
            isAttacking = false;
            animator.SetBool("moving", true);
        }
        else
        {
            isMoving = false;
            animator.SetBool("moving", false);
        }
    }

    void MoveTowardsTarget()
    {
        Vector3 targetDirection = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        float rotationStep = rotationSpeed * Time.deltaTime;
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationStep);
        rb.MoveRotation(newRotation);
        rb.velocity = transform.forward * speed;

        if (Vector3.Distance(transform.position, target.position) < 500f)
        {
            isMoving = false;
            isAttacking = true;
            animator.SetBool("moving", false);
            animator.SetBool("Attack", true);
            currentTarget = target;
        }
    }

    // 애니메이션 이벤트로 호출할 공격 처리 함수
    public void InflictDamageOnTarget()
    {
        attackTrigger.enabled = true;
        animator.SetBool("Attack", false);
        Invoke("AttackTriggerFalse", 0.5f);
    }

    private void AttackTriggerFalse()
    {
        attackTrigger.enabled = false;
        isAttacking = false; // 공격 상태 종료
    }
}
