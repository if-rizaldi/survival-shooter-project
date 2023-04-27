using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public float damage;

    [Range(1f,10f)]
    public float fireRate;
    public bool isOwned;

    private PlayerData gun;


}
