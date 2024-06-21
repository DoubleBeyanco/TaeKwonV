using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CockpitJoystick : MonoBehaviour
{
    [SerializeField] private InputData input;

    private XRSimpleInteractable grabinteractable;
    private bool isActive = false;

    private void Awake()
    {
        grabinteractable = GetComponent<XRSimpleInteractable>();

        grabinteractable.selectEntered.AddListener(ControlBody);
        grabinteractable.selectExited.AddListener(ControlBodyDeActive);
    }

    private void ControlBody(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            isActive = true;
            StartCoroutine(TrackingContorller());
        }
    }

    private void ControlBodyDeActive(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            isActive = false;
            transform.rotation = Quaternion.identity;
        }
    }

    private IEnumerator TrackingContorller()
    {
        while (isActive)
        {
            input._rightController.TryGetFeatureValue(OculusUsages.thumbrest, out bool value);

            if (value)
            {
                JustCal();
            }
            else
            {
                TrackingPosResetAndCal();
            }

            

            yield return null;
        }

        yield break;
    }

    private void JustCal()
    {
        input._rightController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 2);
    }

    private void TrackingPosResetAndCal()
    {
        input._rightController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot);
        input._rightController.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out Vector3 vel);

        if (rot.eulerAngles.x > 0)
        {
            transform.eulerAngles -= vel;
        }
    }

}
