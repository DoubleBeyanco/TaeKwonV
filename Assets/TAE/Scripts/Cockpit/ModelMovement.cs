using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class ModelMovement : MonoBehaviour
{
    [SerializeField] private GameObject Controller;
    [SerializeField] private InputData input;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float rotspeed = 0.5f;
    [SerializeField] private float rotationSensitivity = 2f;
    private Rigidbody rb;

    private float gear;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        input._rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool _value);

        if (!_value)
        {
            RotateWithStick();
        }

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


    private void RotateWithStick()
    {
        float rot = Controller.transform.eulerAngles.z;

        if (rot > 180)
        {
            rot = rot - 360;
        }


        if (rot < -rotationSensitivity)
        {
            transform.eulerAngles += new Vector3(0f, rotspeed, 0f);
            
        }
        else if (rot > rotationSensitivity)
        {
            transform.eulerAngles -= new Vector3(0f, rotspeed, 0f);
        }
        
    }
}
