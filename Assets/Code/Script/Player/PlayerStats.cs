using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    [HideInInspector] public int playerInitialLife;
    [HideInInspector] public float playerMaxHealth;
    [HideInInspector] public float playerSpeed;
    [HideInInspector] public float playerStrength;
    [HideInInspector] public float buffDuration;
    [HideInInspector] public float buffMultiplier;
    [HideInInspector] public float luck;
    [HideInInspector] public float shieldAmount;
    [HideInInspector] public float dodgePower;

    [HideInInspector] public bool isPlayerAbleToDodge;

    [HideInInspector] public bool isPlayerDodge;

    //[HideInInspector] public bool isPlayerReadyToDodge;




    private float currentSpeed;
    private float currentStrength;
    private bool isShield;
    private bool isInvincible;
    private float playerHealth;
    private int lifeBuff;
    [HideInInspector] public int playerLifeRemaining;
    [HideInInspector] public bool isAndroidControl;


    [Header("Event")]
    public GameEvent onPlayerHealthChanged;
    [SerializeField] private GameEvent onPlayerGameOver;
    [SerializeField] private GameEvent onPlayerLifeChanged;
    [SerializeField] private GameEvent onPlayerLifeBuffed;

    [HideInInspector] public float playerAccel = 500f;
    [HideInInspector] public float playerJumpHeight = 50f;

    [HideInInspector] public bool isPlayerDamaged;
    [HideInInspector] public Vector3 enemyPosition;



    void Start()
    {
        if (playerData)
            lifeBuff = playerData.lifeBuff;

        playerData.PlayerStart();
        if (playerData)
        {
            playerMaxHealth = playerData.healthAmount;
            playerInitialLife = playerData.lifeAmount;
            playerSpeed = playerData.speedAmount;
            playerStrength = playerData.strengthAmount;
            buffDuration = playerData.buffDuration;
            buffMultiplier = playerData.buffMultiplier;
            luck = playerData.playerLuck;
            isPlayerAbleToDodge = playerData.isPlayerAbleToDodge;
            isAndroidControl = playerData.isAndroidControl;


        }

        //shieldAmount *= buffMultiplier;
        playerHealth = playerMaxHealth;
        playerLifeRemaining = playerInitialLife;

        isPlayerDamaged = false;
        enemyPosition = Vector3.zero;

        //Debug.Log("Start game with " + playerLifeRemaining + " life");

    }




    public void PlayerDamaged(float damage)
    {
        playerHealth -= damage;
        StartCoroutine(PlayerGetDamagedEffect(0.1f));
        onPlayerHealthChanged.Raise(this, playerHealth);

        if (playerHealth <= 0)
        {
            PlayerDead();
        }
    }

    public void OnRightClick()
    {
        Debug.Log("isPlayerAbleToDodge : " + isPlayerAbleToDodge);
        if (isPlayerAbleToDodge)
        {
            StartCoroutine(PlayerDodge(0.5f));
        }


    }


    IEnumerator PlayerDodge(float dodgeDuration)
    {
        isPlayerDodge = true;
        yield return new WaitForSeconds(dodgeDuration);
        isPlayerDodge = false;
        yield break;

    }


    IEnumerator PlayerGetDamagedEffect(float time)
    {
        isPlayerDamaged = true;

        yield return new WaitForSeconds(time);

        isPlayerDamaged = false;

        enemyPosition = Vector3.zero;

        yield break;
    }

    public void PlayerHealed(float healAmount)
    {
        playerHealth += healAmount;
        onPlayerHealthChanged.Raise(this, playerHealth);

    }

    public IEnumerator PlayerStrengthen(float value)
    {
        playerStrength += value;

        yield return new WaitForSeconds(buffDuration);

        playerStrength -= value;

        yield return null;
        yield break;

    }

    public IEnumerator PlayerSpeedUp(float value)
    {
        playerSpeed += value;
        yield return new WaitForSeconds(buffDuration);
        playerSpeed -= value;
        yield return null;
        yield break;
    }





    public void PlayerDead()
    {
        PlayerResetHealth();
        //Play Some Animation
        ClearAllPlayerBuff();


        if (lifeBuff > 0)
        {
            lifeBuff--;
            onPlayerLifeBuffed.Raise(this, lifeBuff);
            Debug.Log("life buff berkurang");
        }
        else if (lifeBuff <= 0)
        {
            playerLifeRemaining--;
            onPlayerLifeChanged.Raise(this, playerLifeRemaining);
            Debug.Log("nyawa berkurang");
        }

        if (playerLifeRemaining <= 0)
        {
            onPlayerGameOver.Raise(this, true);
        }

        Debug.Log("Life :" + playerLifeRemaining + "Buff :" + lifeBuff);

    }



    public void PlayerResetHealth()
    {
        playerHealth = playerMaxHealth;
    }

    public void ClearAllPlayerBuff()
    {
        currentSpeed = playerSpeed;
        currentStrength = playerStrength;
        playerHealth = playerMaxHealth;
        isShield = false;
        isInvincible = false;


    }
}
