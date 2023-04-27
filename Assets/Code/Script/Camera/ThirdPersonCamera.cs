using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{

    [SerializeField] private Transform follow;

    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeSmoothness = 0.5f;
    [SerializeField] private bool isCameraShake = true;

    Vector3 originalPos;
    Quaternion originalRot;
    float fieldView;
    Camera cam;

    void Awake()
    {
        originalPos = this.transform.localPosition;
        originalRot = this.transform.localRotation;
        transform.LookAt(follow);
        cam = this.gameObject.GetComponent<Camera>();
        fieldView = cam.fieldOfView;
    }

   

    public void PlayerDamagedCameraEffect(Component sender, object data)
    {
        if (isCameraShake)
        {
            if (data is float)
            {

                float amount = (float)data;


                GameObject player = sender.gameObject;
                if (player.GetComponent<PlayerStats>())
                {

                    PlayerStats playerStats = player.GetComponent<PlayerStats>();
                    if (playerStats.isPlayerDamaged)
                    {

                        StartCoroutine(CameraShake(shakeDuration));

                    }
                }


            }
        }




    }

    IEnumerator CameraShake(float duration)
    {

        Quaternion randomRot = Random.rotation;
        for(float i = 0; i < duration; i+=Time.deltaTime )
        {
            this.transform.rotation = Quaternion.Lerp(originalRot, randomRot, shakeSmoothness);
        }

        yield return new WaitForSeconds(duration);
        transform.LookAt(follow);
        yield break;




    }
}
