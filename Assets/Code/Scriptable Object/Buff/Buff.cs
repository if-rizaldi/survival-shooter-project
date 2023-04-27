using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerBuff
{
    Healing,
    Life,
    Strength,
    Speed,
}

[CreateAssetMenu(menuName = "Scriptable Object/Buff")]
public class Buff : ScriptableObject
{
    public PlayerBuff buffType;
    public float value;

    private PlayerStats player;

    public void ApplyBuff()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if(buffType == PlayerBuff.Healing)
        {
            player.PlayerHealed(value);
        }

        if(buffType == PlayerBuff.Life)
        {
            player.playerLifeRemaining++;
        }

        if(buffType == PlayerBuff.Strength)
        {
            player.StartCoroutine(player.PlayerStrengthen(value));
        }
          if(buffType == PlayerBuff.Speed)
        {
            player.StartCoroutine(player.PlayerSpeedUp(value));
        }


    }

}
