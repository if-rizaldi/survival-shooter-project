using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelectingButton : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    private PickToolButton button;
    TextMeshProUGUI buttonDisplayName;

    void Start()
    {
        button = this.gameObject.GetComponent<PickToolButton>();
        buttonDisplayName = button.gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        buttonDisplayName.text = weapon.weaponName;

        if (!weapon.isOwned)
        {
            button.isButtonSelected = false;
            button.buttonHighlighted.SetActive(false);
            button.buttonColor.SetActive(false);
            this.gameObject.GetComponent<Button>().interactable = false;

            
        }

         if(button.buttonManager.continueButton != null)
            {

                Button continueButton;
                continueButton = button.buttonManager.continueButton.GetComponent<Button>();
                continueButton.onClick.AddListener(ApplyWeapon);
                Debug.Log("listener added");
            }
    }

    public void ApplyWeapon()
    {
        if (button.isButtonSelected)
        {
            PlayerData player;
            player = Resources.Load<PlayerData>("SurvivalModePlayerData");

            player.currentWeapon = weapon;
           
        }

    }


}
