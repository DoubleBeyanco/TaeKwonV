using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject otherPortal;
    public float transitionTime = 2f; // 이동 시간 설정

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Teleport(other));
        }
    }

    private IEnumerator Teleport(Collider player)
    {
        Vector3 startPosition = player.transform.position;
        Vector3 endPosition = otherPortal.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            player.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = endPosition;
    }
}
