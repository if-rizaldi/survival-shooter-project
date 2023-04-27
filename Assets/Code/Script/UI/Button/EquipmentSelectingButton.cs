using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EquipmentSelectingButton : MonoBehaviour
{
    [SerializeField] private Equipment equipment;


    private PickToolButton button;

    TextMeshProUGUI buttonDisplayName;

    void Start()
    {
        button = this.gameObject.GetComponent<PickToolButton>();
        buttonDisplayName = button.gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        buttonDisplayName.text = equipment.equipmentName;

        if (!equipment.isOwned)
        {
            button.isButtonSelected = false;
            button.buttonHighlighted.SetActive(false);
            button.buttonColor.SetActive(false);
            this.gameObject.GetComponent<Button>().interactable = false;



        }

        if (button.buttonManager.continueButton != null)
        {
            Button continueButton;
            continueButton = button.buttonManager.continueButton.GetComponent<Button>();
            continueButton.onClick.AddListener(ApplyEquipment);
        }

    }

    public void ApplyEquipment()
    {
        if (button.isButtonSelected)
        {
            equipment.ApplyEquipment();

        }

    }


}
