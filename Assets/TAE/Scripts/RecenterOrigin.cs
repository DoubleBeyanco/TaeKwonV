using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;

public class RecenterOrigin : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] Transform origin;
    [SerializeField] Transform target;
    [SerializeField] InputData input;

    private void Recenter()
    {
        XROrigin xrOrigin = GetComponent<XROrigin>();
        xrOrigin.MoveCameraToWorldLocation(target.position);
        xrOrigin.MatchOriginUpCameraForward(target.up, target.forward);
    }

    private void Update()
    {
        input._rightController.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool _Rvalue);
        input._leftController.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool _Lvalue);
        if (_Rvalue || _Lvalue)
        {
            Recenter();
        }
    }


}
