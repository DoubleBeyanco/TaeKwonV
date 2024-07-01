using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : MonoBehaviour
{
    public Collider mouse;
    public WhaleMove whaleMove;
    public GameObject mousePoint;
    private void Start()
    {
        mouse.enabled = false;
    }
    public void InflictDamageOnTarget()
    {
       whaleMove.InflictDamageOnTarget();
        mouse.enabled = false;
    }
    public void StartAttack()
    {
        mouse.enabled = true;
    }
    void OnTriggerStay(Collider other)
    {
        // 입 속에 진입한 오브젝트를 감지
        if (!other.gameObject.CompareTag("Whale"))
        {
            Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
            if (otherRigidbody != null && otherRigidbody.isKinematic == false)
            {
                Vector3 direction = (mousePoint.transform.position - other.transform.position).normalized;

                // 고래 입 속으로 오브젝트를 밀어넣음
                otherRigidbody.velocity = direction * 700f; // 적절한 속도로 조정
            }
        }
    }
}

