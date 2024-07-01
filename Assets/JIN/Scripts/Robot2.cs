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

        // 위치가 변경되었는지 확인합니다.
        bool isMoving = currentPosition != lastPosition;

        // 애니메이터 매개변수를 업데이트합니다.
        animator.SetBool("isMoving", isMoving);

        // 마지막 위치를 업데이트합니다.
        lastPosition = currentPosition;
    }

    // 충돌이 발생했을 때 호출됩니다.
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 "PlayerBullet" 또는 "PlayerSkill" 태그를 가지고 있는지 확인합니다.
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            // Guard 애니메이션을 트리거하는 코루틴을 시작합니다.
            StartCoroutine(TriggerGuardAnimation());
        }
        else if (collision.gameObject.CompareTag("PlayerSkill"))
        {
            // Down 애니메이션을 트리거하는 코루틴을 시작합니다.
            StartCoroutine(TriggerDownAnimation());
        }
    }

    // Guard 애니메이션을 트리거하는 코루틴입니다.
    private IEnumerator TriggerGuardAnimation()
    {
        // isHit 매개변수를 true로 설정하여 Guard 애니메이션을 시작합니다.
        animator.SetBool("isHit", true);

        // Guard 애니메이션이 끝날 때까지 대기합니다.
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // isHit 매개변수를 false로 설정하여 원래 상태로 돌아갑니다.
        animator.SetBool("isHit", false);
    }

    // Down 애니메이션을 트리거하는 코루틴입니다.
    private IEnumerator TriggerDownAnimation()
    {
        // isSkillHit 매개변수를 true로 설정하여 Down 애니메이션을 시작합니다.
        animator.SetBool("isSkillHit", true);

        // Down 애니메이션이 끝날 때까지 대기합니다.
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Down 애니메이션이 끝난 후 GetUp 애니메이션을 트리거합니다.
        yield return StartCoroutine(TriggerGetUpAnimation());
    }

    // GetUp 애니메이션을 트리거하는 코루틴입니다.
    private IEnumerator TriggerGetUpAnimation()
    {
        // getup 애니메이션을 시작합니다.
        animator.SetBool("getup", true);

        // getup 애니메이션이 끝날 때까지 대기합니다.
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // getup 애니메이션을 끝내고 다시 초기화합니다.
        animator.SetBool("getup", false);
        animator.SetBool("isSkillHit", false);
    }
}