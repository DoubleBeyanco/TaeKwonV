using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CockPitMovement : MonoBehaviour
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
            input._rightController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot);

            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 2);
            yield return null;
        }

        yield break;
    }

}
