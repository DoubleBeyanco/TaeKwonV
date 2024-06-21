using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelMovement : MonoBehaviour
{
    [SerializeField] private GameObject Controller;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.position += Vector3.forward * Time.deltaTime * Controller.transform.rotation.x;
        Debug.Log("eulerAngle : " + Controller.transform.rotation.x);

       /* if (Controller.transform.rotation.x > 0)
        {
            transform.position += Vector3.forward * Time.deltaTime * Controller.transform.rotation.x;
            Debug.Log("eulerAngle : " + Controller.transform.rotation.x);
        }
        else
        {
            rb.AddForce(Vector3.back * Time.deltaTime * -(Controller.transform.rotation.x));
            Debug.Log("eulerAngle - : " + Controller.transform.rotation.x);
        }*/

        /*if (Controller.transform.eulerAngles.z > 0)
        {
            rb.AddForce(Vecor)
        }*/


    }
}
