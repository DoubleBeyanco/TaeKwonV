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

    // 회전 속도를 조절할 변수들
    [SerializeField] private float rotationSpeedForward = 1000f; // 전진 시 회전 속도
    [SerializeField] private  float rotationSpeedBackward = 3000f; // 후진 시 회전 속도

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

            // 원하는 조건에 따라 회전을 조정합니다.
            float difference = pos.z - initPos.z;

            // 회전 속도를 결정합니다.
            float speed = difference >= 0 ? rotationSpeedForward : rotationSpeedBackward;

            Quaternion targetRotation = Quaternion.Euler(initialRotation.eulerAngles.x + difference * speed, initialRotation.eulerAngles.y, initialRotation.eulerAngles.z);

            // Quaternion.Lerp를 사용하여 부드럽게 회전합니다.
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            yield return null;
        }
    }
}
