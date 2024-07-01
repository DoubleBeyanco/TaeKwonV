using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Robot2 : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;


    void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;

        // ��ġ�� ����Ǿ����� Ȯ���մϴ�.
        bool isMoving = currentPosition != lastPosition;

        // �ִϸ����� �Ű������� ������Ʈ�մϴ�.
        animator.SetBool("isMoving", isMoving);

        // ������ ��ġ�� ������Ʈ�մϴ�.
        lastPosition = currentPosition;
    }

    // �浹�� �߻����� �� ȣ��˴ϴ�.
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� "PlayerBullet" �Ǵ� "PlayerSkill" �±׸� ������ �ִ��� Ȯ���մϴ�.
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            // Guard �ִϸ��̼��� Ʈ�����ϴ� �ڷ�ƾ�� �����մϴ�.
            StartCoroutine(TriggerGuardAnimation());
        }
        else if (collision.gameObject.CompareTag("PlayerSkill"))
        {
            // Down �ִϸ��̼��� Ʈ�����ϴ� �ڷ�ƾ�� �����մϴ�.
            StartCoroutine(TriggerDownAnimation());
        }
    }

    // Guard �ִϸ��̼��� Ʈ�����ϴ� �ڷ�ƾ�Դϴ�.
    private IEnumerator TriggerGuardAnimation()
    {
        // isHit �Ű������� true�� �����Ͽ� Guard �ִϸ��̼��� �����մϴ�.
        animator.SetBool("isHit", true);

        // Guard �ִϸ��̼��� ���� ������ ����մϴ�.
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // isHit �Ű������� false�� �����Ͽ� ���� ���·� ���ư��ϴ�.
        animator.SetBool("isHit", false);
    }

    // Down �ִϸ��̼��� Ʈ�����ϴ� �ڷ�ƾ�Դϴ�.
    private IEnumerator TriggerDownAnimation()
    {
        // isSkillHit �Ű������� true�� �����Ͽ� Down �ִϸ��̼��� �����մϴ�.
        animator.SetBool("isSkillHit", true);

        // Down �ִϸ��̼��� ���� ������ ����մϴ�.
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Down �ִϸ��̼��� ���� �� GetUp �ִϸ��̼��� Ʈ�����մϴ�.
        yield return StartCoroutine(TriggerGetUpAnimation());
    }

    // GetUp �ִϸ��̼��� Ʈ�����ϴ� �ڷ�ƾ�Դϴ�.
    private IEnumerator TriggerGetUpAnimation()
    {
        // getup �ִϸ��̼��� �����մϴ�.
        animator.SetBool("getup", true);

        // getup �ִϸ��̼��� ���� ������ ����մϴ�.
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // getup �ִϸ��̼��� ������ �ٽ� �ʱ�ȭ�մϴ�.
        animator.SetBool("getup", false);
        animator.SetBool("isSkillHit", false);
    }
}