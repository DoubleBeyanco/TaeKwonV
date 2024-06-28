using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingSystem : MonoBehaviour
{
    [SerializeField] private GameObject virtualCrossHair;
    [SerializeField] private GameObject actualCrossHair;

    public void AimCalc(Vector3 _rot, int _range)
    {
        if (_rot.x > 180)
        {
            _rot.x -= 360f;
        }

        if (_rot.z > 180)
        {
            _rot.z -= 360f;
        }

        float targetPosY = Mathf.Clamp(_rot.x / 100, -0.84375f, 0.84375f);
        float targetPosX = Mathf.Clamp(_rot.z * 1.5f / 100, -1.5f, 1.5f);

        Vector3 currentPos = actualCrossHair.transform.localPosition;
        Vector3 targetPos = new Vector3(-targetPosX, targetPosY, 0);

        actualCrossHair.transform.localPosition = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * 10);
        virtualAimCalc(_range);
    }

    private void virtualAimCalc(int _range)
    {
        virtualCrossHair.transform.localPosition = new Vector3(actualCrossHair.transform.localPosition.x * (0.4f * _range), actualCrossHair.transform.localPosition.y * (0.4f * _range), _range);
        virtualCrossHair.transform.localPosition = virtualCrossHair.transform.localPosition + new Vector3(0, 7.29f * (_range / 100), 0);
    }
}
