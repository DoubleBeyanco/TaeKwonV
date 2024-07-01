using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ast : MonoBehaviour
{
    private void Start()
    {
        transform.position = new Vector3(Random.Range(-5000, 5000f), Random.Range(20041.1f, 23341.1f), Random.Range(-5000, 5000f));
        transform.localScale = new Vector3(Random.Range(0.5f, 3), Random.Range(0.5f, 3), Random.Range(0.5f, 3));
    }
}
