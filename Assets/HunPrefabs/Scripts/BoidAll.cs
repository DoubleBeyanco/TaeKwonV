using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidAll : MonoBehaviour
{
    public Material[] mt;
    private Control[] controls;

    private void Awake()
    {
        controls = GetComponentsInChildren<Control>();
    }
    private void Start()
    {
        Invoke("SetMat",1f);
    }
    private void SetMat()
    {
        for(int b = 0;  b < controls.Length; b++) 
        {
            controls[b].SetMat(mt[b]);
        }

    }

}
