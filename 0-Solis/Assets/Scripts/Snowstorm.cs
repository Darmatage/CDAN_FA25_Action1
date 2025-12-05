    using UnityEngine;

    public class FollowCamera : MonoBehaviour
    {
        public Transform targetCamera; // Assign your Main Camera here in the Inspector

        void LateUpdate()
        {
            if (targetCamera != null)
            {
                transform.position = targetCamera.position;
                // If you need it to stay at a specific offset from the camera:
                // transform.position = targetCamera.position + new Vector3(0, 0, 10); 
            }
        }
    }