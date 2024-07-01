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
        // �� �ӿ� ������ ������Ʈ�� ����
        if (!other.gameObject.CompareTag("Whale"))
        {
            Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
            if (otherRigidbody != null && otherRigidbody.isKinematic == false)
            {
                Vector3 direction = (mousePoint.transform.position - other.transform.position).normalized;

                // �� �� ������ ������Ʈ�� �о����
                otherRigidbody.velocity = direction * 700f; // ������ �ӵ��� ����
            }
        }
    }
}

