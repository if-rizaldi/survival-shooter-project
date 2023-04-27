using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickToolMenu : MonoBehaviour
{
    [SerializeField] private int numberOfButtonAllowed;
    public GameObject continueButton;

    [SerializeField] private bool isControlContinueButton;
    private int numberOfButtonSelected;
    private PickToolButton[] buttons;


    void Start()
    {
        buttons = GetComponentsInChildren<PickToolButton>();
        if (continueButton && isControlContinueButton)
        {
            continueButton.SetActive(false);
        }

    }






    public void ActivateButton(GameObject buttonGameobject)
    {
        //define the selected button to activate    
        PickToolButton buttonSelected;
        buttonSelected = buttonGameobject.GetComponent<PickToolButton>();

        //check the number of button already selected
        numberOfButtonSelected = 0;
        foreach (PickToolButton button in buttons)
        {
            if (button.isButtonSelected)
                numberOfButtonSelected++;
        }

        if (continueButton && isControlContinueButton)
        {
            ControlContinueButton();
        }

        //if still allowed: activate the selected button while deactive the other button except the already selected button
        if (numberOfButtonSelected < numberOfButtonAllowed)
        {
            if (buttonSelected.isButtonSelected)
                buttonSelected.isButtonSelected = false;
            else
                buttonSelected.isButtonSelected = true;


            foreach (PickToolButton button in buttons)
            {
                //reset all the button attribute inside the menu
                //button.isButtonPressed = false;
                button.isTheLastButtonSelected = false;

                //except the for the button that already selec
                if (button.isButtonSelected)
                {
                    button.buttonColor.SetActive(true);
                    button.buttonHighlighted.SetActive(true);
                    button.isButtonSelected = true;
                }
                else
                {
                    button.buttonColor.SetActive(false);
                    button.buttonHighlighted.SetActive(false);
                    button.isButtonSelected = false;

                }

                //buttonSelected.isButtonPressed = true;
                buttonSelected.isTheLastButtonSelected = true;

            }
        }
        else if (numberOfButtonSelected == numberOfButtonAllowed)
        {
            if (buttonSelected.isButtonSelected)
            {
                buttonSelected.isButtonSelected = false;

                foreach (PickToolButton button in buttons)
                {
                    //button.isButtonPressed = false;

                    if (!button.isButtonSelected)
                    {
                        button.buttonColor.SetActive(false);
                        button.buttonHighlighted.SetActive(false);
                        button.isButtonSelected = false;
                    }
                    else
                    {

                        button.buttonColor.SetActive(true);
                        button.buttonHighlighted.SetActive(true);
                        button.isButtonSelected = true;

                    }
                }
                buttonSelected.isTheLastButtonSelected = true;


            }
            else
            {
                buttonSelected.isButtonSelected = true;

                foreach (PickToolButton button in buttons)
                {
                    //button.isButtonPressed = false;

                    if (!button.isButtonSelected)
                    {
                        button.buttonColor.SetActive(false);
                        button.buttonHighlighted.SetActive(false);
                        button.isButtonSelected = false;
                    }
                    else
                    {
                        if (button.isTheLastButtonSelected)
                        {
                            button.buttonColor.SetActive(false);
                            button.buttonHighlighted.SetActive(false);
                            button.isButtonSelected = false;
                            button.isTheLastButtonSelected = false;
                        }
                        else
                        {
                            button.buttonColor.SetActive(true);
                            button.buttonHighlighted.SetActive(true);
                            button.isButtonSelected = true;
                        }

                    }
                }
                //buttonSelected.isButtonPressed = true;
                buttonSelected.isTheLastButtonSelected = true;
            }
        }

        numberOfButtonSelected = 0;
        foreach (PickToolButton button in buttons)
        {
            if (button.isButtonSelected)
                numberOfButtonSelected++;

            if (continueButton && isControlContinueButton)
            {
                ControlContinueButton();
            }
        }


        void ControlContinueButton()
        {
            if (numberOfButtonSelected >= numberOfButtonAllowed)
            {
                continueButton.SetActive(true);
            }
            else if (numberOfButtonSelected < numberOfButtonAllowed)
                continueButton.SetActive(false);

        }





    }

}





