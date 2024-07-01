using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlackHole : MonoBehaviour
{
    public float velock = 5000;
 
    private void OnTriggerStay(Collider other)
    {
        Vector3 pos = (transform.position - other.gameObject.transform.position).normalized;
        if (other.gameObject.GetComponent<Transform>())
        {
            other.gameObject.GetComponent<Transform>().position += pos * Time.deltaTime * 1000;
        }

    }
}
