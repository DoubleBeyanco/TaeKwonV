using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimmingSystem : MonoBehaviour
{
    [SerializeField] private GameObject virtualCrossHair;
    [SerializeField] private GameObject actualCrossHair;

    // 비례 계수
    public float scale = 0.003f; // 가상의 에임에 비례하여 실제 에임이 움직이는 비율

    public void actualAimCalc(Vector3 _rot)
    {
        if (_rot.x > 180)
        {
            _rot.x -= 360f;
        }

        if (_rot.z > 180)
        {
            _rot.z -= 360f;
        }

        virtualAimCalc(_rot);

        // x와 y 좌표를 클램핑하여 목표 위치 계산
        float targetPosY = Mathf.Clamp(_rot.x, transform.position.x - 1.5f, transform.position.x + 1.5f);
        float targetPosX = Mathf.Clamp(_rot.z, transform.position.y - 0.84375f, transform.position.y + 0.84375f);
        Debug.Log("targetPosX: " + targetPosX);
        Debug.Log("targetPosY: " + targetPosY);

        // 현재 위치 가져오기
        Vector3 currentPos = actualCrossHair.transform.localPosition;

        // 가상의 에임 위치에서 비례적으로 실제 에임 위치를 계산
        Vector3 virtualPos = virtualCrossHair.transform.localPosition;
        Vector3 targetPos = new Vector3(virtualPos.x * scale, virtualPos.y * scale, 0);

        // Lerp를 사용하여 부드럽게 이동
        actualCrossHair.transform.localPosition = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * 5f);
    }

    private void virtualAimCalc(Vector3 _rot)
    {
        float targetPosY = Mathf.Clamp(_rot.x, transform.position.x - 150f, transform.position.x + 150f);
        float targetPosX = Mathf.Clamp(_rot.z, transform.position.y - 84.375f, transform.position.y + 84.375f);

        Vector3 currentPos = virtualCrossHair.transform.localPosition;
        Vector3 targetPos = new Vector3(-targetPosX, -targetPosY, 300);

        virtualCrossHair.transform.localPosition = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * 5f);
    }
}
