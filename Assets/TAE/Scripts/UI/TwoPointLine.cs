using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TwoPointLine : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        line.positionCount = 2;
        line.SetPosition(0, pointA.position);
        line.SetPosition(1, pointB.position);
    }
}
