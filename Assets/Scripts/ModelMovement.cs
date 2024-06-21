using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ModelMovement : MonoBehaviour
{
    [SerializeField] private GameObject Controller;
    [SerializeField] private float speed = 0.5f;
    private Rigidbody rb;

    private float gear;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MoveWithGear();
    }
    public void Setgear(float _value)
    {
        gear = _value;
    }

    private void MoveWithGear()
    {
        transform.position += Vector3.forward * gear * Time.deltaTime * speed;
    }
    private void oldway()
    {
        float pos = Controller.transform.eulerAngles.x;

        if (pos > 180)
        {
            pos = pos - 360;
        }

        transform.position += Vector3.forward * pos * Time.deltaTime * speed;
        Debug.Log("Pos: " + pos);
    }
}
