using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Transform target;
    public float speed = 10f;
    public float rotateSpeed = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(MoveStart());
        
    }
    private IEnumerator MoveStart()
    {
        yield return new WaitForSeconds(3f);
        while (true)
        {

            if (target == null)
            {
                yield break;
            }
            Vector3 targetDirection = (target.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);

            rb.MoveRotation(newRotation);
            rb.velocity = transform.forward * speed;
    
            yield return new WaitForEndOfFrame();
        }
    }
}
