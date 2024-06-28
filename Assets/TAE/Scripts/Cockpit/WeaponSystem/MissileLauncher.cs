using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MissileLauncher : MonoBehaviour
{
    public int Range = 300;
    //public VisualEffectAsset Effect;
    public float Damage = 1f;
    public bool ContinuousFire = false;


    public void LockOn()
    {

    }

    public void VivePattern(InputData _controller)
    {
        // 진동 구현해야 함.
    }
}
