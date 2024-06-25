using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AimmingSystem : MonoBehaviour
{
    [SerializeField] private GameObject virtualCrossHair;
    [SerializeField] private GameObject actualCrossHair;

    public float weaponRange;
    public void actualAimCalc(Vector3 _rot, int _range)
    {
        if (_rot.x > 180)
        {
            _rot.x -= 360f;
        }

        if (_rot.z > 180)
        {
            _rot.z -= 360f;
        }


        float targetPosY = Mathf.Clamp(_rot.x / 100, - 0.5f, 0.5f);
        float targetPosX = Mathf.Clamp(_rot.z / 100, - 0.5f, 0.5f);

        Vector3 currentPos = actualCrossHair.transform.localPosition;
        Vector3 targetPos = new Vector3(-targetPosX, targetPosY, 0);

        actualCrossHair.transform.localPosition = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * 10);

        virtualAimCalc(_rot, _range);
    }

    private void virtualAimCalc(Vector3 _rot, int _range)
    {
        float offsetX = (_range * 0.6f);
        float offsetY = (_range * 0.3f);

        float targetPosY = Mathf.Clamp(_rot.x + 1.65f, transform.localPosition.y - offsetY + 1.65f, transform.localPosition.y + offsetY);
        float targetPosX = Mathf.Clamp(_rot.z, transform.localPosition.x - offsetX, transform.localPosition.x + offsetX);

        Vector3 currentPos = virtualCrossHair.transform.localPosition;
        Vector3 targetPos = new Vector3(-targetPosX, targetPosY, _range);

        virtualCrossHair.transform.localPosition = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * 20);
    }

    /*
    ½ÇÁ¦ ¿¡ÀÓ ÁÂÇ¥(virtual)
    zÃà ÁÂÇ¥°¡ 100ÀÏ¶§ xÃà ÁÂÇ¥°¡ 60, yÃà ÁÂÇ¥ 30
     */
}
