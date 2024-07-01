using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    private SphereCollider col;
    float maxr = 20;
    private void Start()
    {
        col = GetComponent<SphereCollider>();
        col.enabled = false;
        StartCoroutine(ColScale());
    }
    private void Update()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 pos = (transform.position - collision.gameObject.transform.position).normalized;
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            collision.gameObject.GetComponent<Rigidbody>().velocity = pos  * 1000;
        }
    }
    private IEnumerator ColScale()
    {
        yield return new WaitForSeconds(3);
        col.enabled = true;
        float r = 0;
        while(r < maxr)
        {
            float x = Time.deltaTime;
                x += Time.deltaTime + Time.deltaTime + Time.deltaTime;
          r += x + Time.deltaTime + 0.05f;
          col.radius = r;

          yield return new WaitForEndOfFrame();
        }
    }
}
