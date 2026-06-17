using UnityEngine;

namespace My2DGame
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private float speedModifier = 1.0f;

        private Transform cameraTransform;
        private Vector3 lastCameraPosition;
        private float initialZ;

        private void Start()
        {
            cameraTransform = Camera.main.transform;
            lastCameraPosition = cameraTransform.position;
            initialZ = transform.position.z;
        }

        private void LateUpdate()
        {
            // 1. 카메라가 이동한 거리 계산
            Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

            // 2. Z축 거리에 따른 패럴랙스 강도 계산
            // 카메라(z=0)에서 배경(initialZ)까지의 거리를 이용하여 이동 비례 결정
            // (뒤에 있을수록 0에 가깝게, 앞에 있을수록 1에 가깝게)
            float parallaxFactor = Mathf.Abs(initialZ) / 10f; // 10은 기준 거리(조절 가능)

            // 3. 이동 적용
            // 카메라가 이동한 만큼 배경도 함께 이동하되, Z축 비례만큼 줄여줌
            transform.position += new Vector3(deltaMovement.x * (1f - parallaxFactor), 0, 0);

            lastCameraPosition = cameraTransform.position;
        }
    }
}
