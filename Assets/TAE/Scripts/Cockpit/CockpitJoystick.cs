using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CockpitJoystick : MonoBehaviour
{
    [SerializeField] private InputData input;
    [SerializeField] private AimmingSystem aim;

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
            StartCoroutine(TrackingController());
        }
    }

    private void ControlBodyDeActive(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            isActive = false;
            transform.localRotation = Quaternion.identity;
            Debug.Log("Deactivate :" + transform.eulerAngles);
        }
    }

    private IEnumerator TrackingController()
    {
        while (isActive)
        {
            input._rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool _value);

            if (_value)
            {
                AimmingMod();
                TrackingAim();
            }
            else
            {
                TrackingRotCal();
            }

            yield return null;
        }

        yield break;
    }

    private void TrackingRotCal()
    {
        input._rightController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot);

        // 현재 로컬 회전을 업데이트
        Quaternion targetRotation = Quaternion.Euler(0, 0, rot.eulerAngles.z);

        transform.localRotation = targetRotation;
    }

    private void TrackingAim()
    {
        input._rightController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot);

        // 실제 에임 모드에서는 x, z 축 모두 업데이트
        Quaternion targetRotation = Quaternion.Euler(rot.eulerAngles.x, 0, rot.eulerAngles.z);

        transform.localRotation = targetRotation;
    }

    private void AimmingMod()
    {
        aim.actualAimCalc(transform.localEulerAngles);
    }
}
