using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType
{
    MagicBall,
    HealBuff,
    StrengthBuff,
}

public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleType collectibleType;
    [SerializeField] private float destroyDelay;
    private GameObject player;

    [Header("Event")]
    public GameEvent eventTriggered;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            eventTriggered.Raise(this, collectibleType);
            Destroy(this.gameObject, destroyDelay);
            
        }
    }

}
