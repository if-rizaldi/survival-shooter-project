using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    private Transform player;

    [SerializeField] private EnemyStats enemyStats;

    private float damage;
    private bool isChasing;
    private Vector3 deltaScale = new Vector3(1, 0, 1);
    private float enemyForce;
    private float enemyAttackCooldown = 1f;

    NavMeshHit hit;
    Vector3 playerNavmeshPos;
    float agentAccel;



    void Start()
    {
        enemyStats = this.gameObject.GetComponent<EnemyStats>();
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //minimalisir bug navmesh di udara
        Vector3 enemyPosition = transform.position;
        enemyPosition.y = 0;
        transform.position = enemyPosition;

        isChasing = true;
        agent.speed = enemyStats.enemySpeed;
        damage = enemyStats.enemyDamage;
        enemyForce = 5000f;

        agentAccel = agent.acceleration;
        agent.SetDestination(player.position);
        StartCoroutine(CheckPlayerPosition());
    }


    private IEnumerator CheckPlayerPosition()
    {
        while (true)
        {


            if (isChasing)
            {

                NavMeshData navmeshPos = new NavMeshData();
                playerNavmeshPos = player.transform.position;
                playerNavmeshPos.y = navmeshPos.position.y;

                if (agent.isOnNavMesh)
                {
                    agent.SetDestination(playerNavmeshPos);

                    //ubah enemy chasing behaviour
                    if (Vector3.Distance(this.transform.position, player.transform.position) < 15)
                    {
                        agent.speed = 3 * enemyStats.enemySpeed;
                        agent.acceleration = 2 * agentAccel;

                    }
                    else
                    {
                        agent.speed = enemyStats.enemySpeed;
                        agent.acceleration = agentAccel;

                    }
                }
                else
                {
                    float randomScale = 5f;
                    Vector3 randomPos = new Vector3(Random.Range(-1, 1) * randomScale, navmeshPos.position.y, Random.Range(-1, 1) * randomScale);
                    transform.position = Vector3.MoveTowards(this.transform.position, randomPos, 10f);
                }



            }
            else
            {
                agent.SetDestination(this.transform.position);

            }

    
            yield return new WaitForSeconds(0.25f);

        }

    }


    private IEnumerator EnemyDamageCooldown(float timeDelay)
    {

        isChasing = false;
        StopCoroutine(CheckPlayerPosition());
        yield return new WaitForSeconds(timeDelay);
        isChasing = true;
        StartCoroutine(CheckPlayerPosition());
        yield break;
    }


    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Player")
        {
            PlayerStats playerStats;
            playerStats = other.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.PlayerDamaged(damage);
                playerStats.enemyPosition = this.gameObject.transform.position;
                StartCoroutine(EnemyDamageCooldown(enemyAttackCooldown));

            }
            else
                Debug.LogError("Please attach the player stat script so this object can damage you");
        }
    }
}
