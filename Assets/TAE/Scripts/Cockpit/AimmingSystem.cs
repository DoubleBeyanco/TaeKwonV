using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimmingSystem : MonoBehaviour
{
    [SerializeField] private GameObject virtualCrossHair;
    [SerializeField] private GameObject actualCrossHair;

    // ��� ���
    public float scale = 0.003f; // ������ ���ӿ� ����Ͽ� ���� ������ �����̴� ����

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

        // x�� y ��ǥ�� Ŭ�����Ͽ� ��ǥ ��ġ ���
        float targetPosY = Mathf.Clamp(_rot.x, transform.position.x - 1.5f, transform.position.x + 1.5f);
        float targetPosX = Mathf.Clamp(_rot.z, transform.position.y - 0.84375f, transform.position.y + 0.84375f);
        Debug.Log("targetPosX: " + targetPosX);
        Debug.Log("targetPosY: " + targetPosY);

        // ���� ��ġ ��������
        Vector3 currentPos = actualCrossHair.transform.localPosition;

        // ������ ���� ��ġ���� ��������� ���� ���� ��ġ�� ���
        Vector3 virtualPos = virtualCrossHair.transform.localPosition;
        Vector3 targetPos = new Vector3(virtualPos.x * scale, virtualPos.y * scale, 0);

        // Lerp�� ����Ͽ� �ε巴�� �̵�
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
