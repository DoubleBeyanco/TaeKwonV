using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CockpitJoystick : MonoBehaviour
{
    public delegate void AimmingModeDelegate(bool _value);
    private AimmingModeDelegate aimmingModeCallback = null;
    public AimmingModeDelegate AimmingModeCallback { get { return aimmingModeCallback; } set { aimmingModeCallback = value; } }
    [SerializeField] private InputData input;
    [SerializeField] private AimmingSystem aim;

    //[HideInInspector] public GameObject[]
    private XRSimpleInteractable interactable;
    private bool isActive = false;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();

        interactable.selectEntered.AddListener(ControlBody);
        interactable.selectExited.AddListener(ControlBodyDeActive);
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
            input._rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool _PrimaryButton);
            input._rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool _trigger);

            if (_PrimaryButton)
            {
                AimmingMod(_trigger);
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

    private void AimmingMod(bool _value)
    {
        aimmingModeCallback?.Invoke(_value);
    }

}
