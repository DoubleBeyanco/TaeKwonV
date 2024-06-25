using UnityEngine;

public class Missile : MonoBehaviour
{
    public Transform target;
    public float speed = 10f;
    public float rotateSpeed = 5f;
    public float zRotationSpeed = 360f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetDirection = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion zRotation = Quaternion.Euler(0, 0, zRotationSpeed * Time.fixedDeltaTime);

        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime) * zRotation;

        rb.MoveRotation(newRotation);
        rb.velocity = transform.forward * speed;
    }
}