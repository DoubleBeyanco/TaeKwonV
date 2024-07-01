using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleShipRazer : MonoBehaviour
{
    private BattleShipHitPoint HitPoint;
    private void Start()
    {
        HitPoint = GetComponentInChildren<BattleShipHitPoint>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(!HitPoint.gameObject.activeSelf)
        HitPoint.gameObject.SetActive(true);
        HitPoint.transform.position = other.gameObject.transform.position;
    }
    private void OnTriggerExit(Collider other)
    {
        HitPoint.gameObject.SetActive(false);
    }
}
