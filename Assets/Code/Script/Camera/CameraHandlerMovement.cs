using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandlerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject targetToFollow;
    [SerializeField]
    private float smoothness = 1f;
    [SerializeField]
    private Vector3 offset;

   
     void FixedUpdate()
    {
        if(targetToFollow == null)
        {
            targetToFollow = GameObject.FindGameObjectWithTag("Player");
            if(GameObject.FindGameObjectWithTag("Player") == null)
                targetToFollow = this.gameObject;
        }
        Vector3 desiredPosition = targetToFollow.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(this.transform.position, desiredPosition, smoothness);
        transform.position = smoothedPosition;

    }
}
