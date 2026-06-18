using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // [핵심] Z가 0이면 0(정지), Z가 20 이상이면 1(동기화)로 매핑
        // Z값을 20으로 나누어 0~1 사이의 비율로 만듭니다.
        float parallaxMultiplier = Mathf.Clamp01(transform.position.z / 20f);

        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier, 0, 0);

        lastCameraPosition = cameraTransform.position;
    }
}