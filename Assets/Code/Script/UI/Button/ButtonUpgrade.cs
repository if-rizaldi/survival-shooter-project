using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonUpgrade : MonoBehaviour
{
    [SerializeField] private string upgradeName;
    [SerializeField] private Upgrade upgradeType;

    [SerializeField] private float value;

    private PickToolButton button;
    [SerializeField] private PlayerData player;
    TextMeshProUGUI buttonDisplayName;
    void Start()
    {
        button = this.gameObject.GetComponent<PickToolButton>();
        buttonDisplayName = button.gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        buttonDisplayName.text = upgradeName;
    }

    public void ApplyUpgrade()
    {
        if (button.isButtonSelected)
        {
            player.ApplyUpgrade(upgradeType, value);
        }

    }


}
