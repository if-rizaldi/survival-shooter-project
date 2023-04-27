using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffCollectible : MonoBehaviour
{
    [SerializeField] private Buff buffData;
    


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
           buffData.ApplyBuff();
            
        }
    }

}
