using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Upgrade{
    HealthUp,
    PowerUp,
    Life,
}

[CreateAssetMenu(menuName = "Scriptable Object/Player Data")]
public class PlayerData : ScriptableObject
{
    public int playerInitialLife;
    public float playerMaxHealth;
    public float playerSpeed;
    public float playerStrength;
    public float buffDuration;
    public float buffMultiplier;
    public float playerLuck;


    public bool isAndroidControl;
    public bool isPlayerAbleToDodge;
    [HideInInspector] public float healthAmount;
    [HideInInspector] public int lifeAmount;
    [HideInInspector] public float speedAmount;
    [HideInInspector] public float strengthAmount;
    [HideInInspector] public float armorAmount;
    [HideInInspector] public Weapon currentWeapon;
    [HideInInspector] public Vector3 playerLastPosition;

    public int SurvivalModeEnemyCount;

    float healthBuff;
    [HideInInspector] public int lifeBuff;
     float strengthBuff;
    public int playerLastScore;

    public int playerScore;
    public int playerHighestScore;

    public int[] playerScoreList;
    private int scoreIndex = 0;


    public void PlayerStart()
    {
        healthAmount = playerMaxHealth + healthBuff;
        lifeAmount = playerInitialLife;
        speedAmount = playerSpeed;
        strengthAmount = playerStrength + strengthBuff;

        healthBuff = 0;
        lifeBuff = 0;
        strengthBuff = 0;

    }

    public void ApplyUpgrade(Upgrade upgradeType, float value)
    {
        if(upgradeType == Upgrade.HealthUp)
        {
            playerMaxHealth += value;
        }

        if(upgradeType == Upgrade.Life)
        {
            lifeBuff++;
        }
        if(upgradeType == Upgrade.PowerUp)
        {
            playerStrength += value;
        }
    }

    public void PlayerEquipItem(PlayerEquipment equipmentType, float value)
    {
        if(equipmentType == PlayerEquipment.HealthSupplement)
        {
            healthBuff += value;        
            Debug.Log(healthBuff);  
        }

        if(equipmentType == PlayerEquipment.TimeMachine)
        {
            int life = (int) value;
            lifeBuff += life;
            Debug.Log("life nambah");            
        }

        if(equipmentType == PlayerEquipment.GunBooster)
        {
            strengthBuff += value;            
        }
        if(equipmentType == PlayerEquipment.DodgingShoe)
        {
            isPlayerAbleToDodge = true;
        }

    }




    public void SurvivalModeReset()
    {
        playerInitialLife = 2;
        playerMaxHealth = 100;
        playerSpeed = 5;
        playerStrength = 10;
        buffDuration = 10;
        buffMultiplier = 0.3f;
        playerLuck = 0.4f;
        playerScore = 0;
        SurvivalModeEnemyCount = 10;
    }

    public void ScoreCalulation(int score)
    {
        Array.Resize(ref playerScoreList, scoreIndex + 1);
        playerScoreList[scoreIndex] = score;
        scoreIndex++;
        if(score > playerHighestScore)
        {
            playerHighestScore = playerScore;
        }
        playerLastScore = score;
    }


}
