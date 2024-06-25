using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabHandPos : MonoBehaviour
{
    public HandPresence[] HandPose;
    private HandPresence RefHandPose;
    private HandPresence CurHand = null;

    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRoatitions;
    private void Awake()
    {
        /*XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        XRSimpleInteractable

        interactable.selectEntered.AddListener(SetupPose);
        interactable.selectExited.AddListener(UnsetPose);*/

        RefHandPose = GetComponentInChildren<HandPresence>();

    }

    private void Start()
    {
        RefHandPose.gameObject.SetActive(false);
    }


    public void SetupPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            if (arg.interactorObject.transform.GetComponent<HandControl>().controllerCharacteristics.HasFlag(InputDeviceCharacteristics.Right))
            {
                HandPose[1].handAnimator.enabled = false;
                HandPose[1].gameObject.SetActive(false);
                RefHandPose.gameObject.SetActive(true);
                CurHand = HandPose[1];
            }
            else if (arg.interactorObject.transform.GetComponent<HandControl>().controllerCharacteristics.HasFlag(InputDeviceCharacteristics.Left))
            {
                HandPose[0].handAnimator.enabled = false;
                HandPose[0].gameObject.SetActive(false);
                RefHandPose.gameObject.SetActive(true);
                CurHand = HandPose[0];
            }

            SetHandDataValues(CurHand, RefHandPose);
            SetHandData(CurHand, finalHandPosition, finalHandRotation, finalFingerRoatitions);
            
        }
    }

    public void UnsetPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            if (arg.interactorObject.transform.GetComponent<HandControl>().controllerCharacteristics.HasFlag(InputDeviceCharacteristics.Right))
            {
                HandPose[1].handAnimator.enabled = true;
                HandPose[1].gameObject.SetActive(true);
                RefHandPose.gameObject.SetActive(false);
                CurHand = HandPose[1];
            }
            else if (arg.interactorObject.transform.GetComponent<HandControl>().controllerCharacteristics.HasFlag(InputDeviceCharacteristics.Left))
            {
                HandPose[0].handAnimator.enabled = true;
                HandPose[0].gameObject.SetActive(true);
                RefHandPose.gameObject.SetActive(false);
                CurHand = HandPose[0];
            }

            SetHandData(CurHand, startingHandPosition, startingHandRotation, startingFingerRotations);

        }
    }

    public void SetHandDataValues(HandPresence h1, HandPresence h2)
    {
        startingHandPosition = new Vector3 (h1.root.localPosition.x / h1.root.localScale.x, h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);
        finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x, h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);

        startingHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        finalFingerRoatitions = new Quaternion[h2.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; ++i)
        {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRoatitions[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SetHandData(HandPresence h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for (int i = 0; newBonesRotation.Length > i; ++i)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }
}

        
