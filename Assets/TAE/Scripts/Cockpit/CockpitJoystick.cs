using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CockpitJoystick : MonoBehaviour
{
    public delegate void AimingModeDelegate(bool _value);
    private AimingModeDelegate aimingModeCallback = null;
    public AimingModeDelegate AimingModeCallback { get { return aimingModeCallback; } set { aimingModeCallback = value; } }

    [SerializeField] private InputData input;
    [SerializeField] private AimingSystem aim;

    private XRSimpleInteractable interactable;
    public bool isActive { get; private set; } = false;
    public bool IsPrimaryButtonPressed { get; private set; } = false;

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
        }
    }

    private IEnumerator TrackingController()
    {
        while (isActive)
        {
            input._rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool _primaryButton);
            input._rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool _trigger);

            IsPrimaryButtonPressed = _primaryButton; // Primary ��ư ���� ����

            if (_primaryButton)
            {
                AimingMode(_trigger);
                TrackingAim();
            }
            else
            {
                //AimingMode(false); // Primary ��ư�� ������ �� �߻縦 ����
                TrackingRotCal();
            }

            yield return null;
        }
    }

    private void TrackingRotCal()
    {
        if (input._rightController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot))
        {
            // ���� ���� ȸ���� ������Ʈ
            Quaternion targetRotation = Quaternion.Euler(0, 0, rot.eulerAngles.z);
            transform.localRotation = targetRotation;
        }
    }

    private void TrackingAim()
    {
        if (input._rightController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot))
        {
            // ���� ���� ��忡���� x, z �� ��� ������Ʈ
            Quaternion targetRotation = Quaternion.Euler(rot.eulerAngles.x, 0, rot.eulerAngles.z);
            transform.localRotation = targetRotation;
        }
    }

    private void AimingMode(bool _value)
    {
        aimingModeCallback?.Invoke(_value);
    }
}
