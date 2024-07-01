using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOBullet : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 shootSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        shootSpeed = transform.forward * 500;
        rb.velocity = shootSpeed;
        Invoke(nameof(DeactiveDelay), 2);
    }

    private void DeactiveDelay() => gameObject.SetActive(false);

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
}
