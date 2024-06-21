using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CockpitThrottle : MonoBehaviour
{
    [SerializeField] private InputData input;

    private XRSimpleInteractable grabinteractable;
    private bool isActive = false;
    private Vector3 initPos;
    private Quaternion initialRotation;

    // ȸ�� �ӵ��� ������ ������
    [SerializeField] private float rotationSpeedForward = 1000f; // ���� �� ȸ�� �ӵ�
    [SerializeField] private  float rotationSpeedBackward = 3000f; // ���� �� ȸ�� �ӵ�

    private void Awake()
    {
        grabinteractable = GetComponent<XRSimpleInteractable>();

        grabinteractable.selectEntered.AddListener(ControlBody);
        grabinteractable.selectExited.AddListener(ControlBodyDeActive);
        initialRotation = transform.rotation;
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
        }
    }

    private IEnumerator TrackingController()
    {
        input._leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 _initpos);
        initPos = _initpos;

        while (isActive)
        {
            input._leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 pos);

            // ���ϴ� ���ǿ� ���� ȸ���� �����մϴ�.
            float difference = pos.z - initPos.z;

            // ȸ�� �ӵ��� �����մϴ�.
            float speed = difference >= 0 ? rotationSpeedForward : rotationSpeedBackward;

            Quaternion targetRotation = Quaternion.Euler(initialRotation.eulerAngles.x + difference * speed, initialRotation.eulerAngles.y, initialRotation.eulerAngles.z);

            // Quaternion.Lerp�� ����Ͽ� �ε巴�� ȸ���մϴ�.
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            yield return null;
        }
    }
}
