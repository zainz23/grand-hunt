using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Play with values to get desired perspective
// We can toggle back and forth between top-down view
// Or we can stick with 3rd person (this script) for cam
namespace Com.MyCompany.MyGame
{
    public class ThirdPersonCamera_v2 : MonoBehaviour
    {
        private const float Y_ANGLE_MIN = 5.0f;
        private const float Y_ANGLE_MAX = 50.0f;

        public Transform lookAt;    // Transform that the camera is going to be looking at
        public Transform camTransform;  // Transform of Camera

        private Camera cam;
        // Fields to calculate position and distance of camera
        private float distance = 1.0f;
        // Position
        private float currentX = 0.0f;
        private float currentY = 0.0f;
        private float sensivityX = 4.0f;
        private float sensivityY = 4.0f;

        private void Start()
        {
            camTransform = transform;
            cam = Camera.main;
        }

        private void Update()
        {
            currentX += Input.GetAxis("Mouse X");
            currentY += Input.GetAxis("Mouse Y");

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }

        // After the movement of the player is done...
        private void LateUpdate()
        {
            // Put camera on top of player then we apply rotation x the direction
            // (Put camera in center, got angle which hes looking at, then put camera behind that distance)

            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            camTransform.position = lookAt.position + rotation * dir;
            camTransform.LookAt(lookAt.position);
        }

    }

}
