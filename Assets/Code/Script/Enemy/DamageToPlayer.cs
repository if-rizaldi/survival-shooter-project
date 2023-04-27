using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageToPlayer : MonoBehaviour
{
    [SerializeField]
    private float damage = 5f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerStats playerStats;
            playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.PlayerDamaged(damage);

            }
            else
                Debug.LogError("Please attach the player stat script so this object can damage you");
        }
    }

}
