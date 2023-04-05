using UnityEngine;

namespace EpicToonFX
{
    public class ETFXMouseOrbit : MonoBehaviour
    {
        public Transform target;
        public float distance = 5.0f;
        public float xSpeed = 120.0f;
        public float ySpeed = 120.0f;
        public float yMinLimit = -20f;
        public float yMaxLimit = 80f;
        public float distanceMin = .5f;
        public float distanceMax = 15f;
        public float smoothTime = 2f;
        private float _rotationYAxis = 0.0f;
        private float _rotationXAxis = 0.0f;
        private float _velocityX = 0.0f;
        private float _velocityY = 0.0f;

        // Use this for initialization
        private void Start()
        {
            var angles = this.transform.eulerAngles;
            _rotationYAxis = angles.y;
            _rotationXAxis = angles.x;
            // Make the rigid body not change rotation
            if (this.GetComponent<Rigidbody>())
            {
                this.GetComponent<Rigidbody>().freezeRotation = true;
            }
        }

        private void LateUpdate()
        {
            if (target)
            {
                if (Input.GetMouseButton(1))
                {
                    _velocityX += xSpeed * Input.GetAxis("Mouse X") * distance * 0.02f;
                    _velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
                }

                _rotationYAxis += _velocityX;
                _rotationXAxis -= _velocityY;
                _rotationXAxis = ClampAngle(_rotationXAxis, yMinLimit, yMaxLimit);
                //Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
                var toRotation = Quaternion.Euler(_rotationXAxis, _rotationYAxis, 0);
                var rotation = toRotation;

                distance = Mathf.Clamp(distance - (Input.GetAxis("Mouse ScrollWheel") * 5), distanceMin, distanceMax);
                if (Physics.Linecast(target.position, this.transform.position, out var hit))
                {
                    distance -= hit.distance;
                }

                var negDistance = new Vector3(0.0f, 0.0f, -distance);
                var position = (rotation * negDistance) + target.position;

                this.transform.rotation = rotation;
                this.transform.position = position;
                _velocityX = Mathf.Lerp(_velocityX, 0, Time.deltaTime * smoothTime);
                _velocityY = Mathf.Lerp(_velocityY, 0, Time.deltaTime * smoothTime);
            }
        }
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
            {
                angle += 360F;
            }

            if (angle > 360F)
            {
                angle -= 360F;
            }

            return Mathf.Clamp(angle, min, max);
        }
    }
}