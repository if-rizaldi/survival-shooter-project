using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private float enemyHealth = 100f;
    [SerializeField] private int scoreForPlayer;

    [Header("Event")]
    [SerializeField] private GameEvent onEnemyCountChanged;
    [SerializeField] private GameEvent onPlayerScoreChanged;

    public float enemyDamage;

    public float force;
    public float enemySpeed;
 
    void Awake()
    {
        onEnemyCountChanged.Raise(this, 1);
    }

    public void EnemyDamaged(float amount)
    {
        enemyHealth -=amount;
        if(enemyHealth <= 0f)
        {
            EnemyDeath(scoreForPlayer);
        }
    }

    public void EnemyDeath(int score)
    {
        onEnemyCountChanged.Raise(this, -1);
        onPlayerScoreChanged.Raise(this, score);
        Destroy(this.gameObject);
    }

   
}
