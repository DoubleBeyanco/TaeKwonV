using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerHun : MonoBehaviour
{
    public int hp = 0;

    private void Start()
    {
        hp = 100;
    }
    private void Update()
    {
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Whale"))
        {
            hp = hp - 20;
        }
    }
}
