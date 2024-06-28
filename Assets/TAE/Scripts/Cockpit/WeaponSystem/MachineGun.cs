using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MachineGun : MonoBehaviour
{
    public int Range = 300;
    public float Damage = 1f;
    public float Speed = 100f;
    public GameObject Bullet;
    public bool ContinuousFire = true;

    public void VivePattern(InputData _controller)
    {
        // 진동 구현해야 함.
    }
}
