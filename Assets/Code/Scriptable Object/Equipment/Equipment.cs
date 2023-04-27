using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerEquipment
{
    Torch,
    Armor,
    SpeedShoe,
    DodgingShoe,
    GunBooster,
    TimeMachine,
    HealthSupplement,
    RegenSupplement,

}

[CreateAssetMenu(menuName = "Scriptable Object/Equipment")]
public class Equipment : ScriptableObject
{
    public string equipmentName;
    public PlayerEquipment equipmentType;
    public float value;
    public bool isOwned;

    private PlayerData player;

    public void ApplyEquipment()
    {
        player = Resources.Load<PlayerData>("SurvivalModePlayerData");
        player.PlayerEquipItem(equipmentType, value);
    }


}
