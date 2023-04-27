using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PickToolButton : MonoBehaviour
{

    //public bool isButtonPressed;
    [HideInInspector] public bool isButtonSelected;
    [HideInInspector] public bool isTheLastButtonSelected;
    public PickToolMenu buttonManager;

    [HideInInspector] public GameObject buttonHighlighted;
    [HideInInspector] public GameObject buttonColor;



    Button button;

    void Awake()
    {
        //define some of variables
        button = this.gameObject.GetComponent<Button>();
        buttonHighlighted = transform.GetChild(0).gameObject;
        buttonColor = transform.GetChild(1).gameObject;


        //make sure the button is not selected at the start
        buttonHighlighted.SetActive(false);
        isButtonSelected = false;
        isTheLastButtonSelected = false;

        //register this event
        button.onClick.AddListener(OnButtonClick);


    }


    public void OnButtonClick()
    {
        buttonManager.ActivateButton(this.gameObject);

    }



}
