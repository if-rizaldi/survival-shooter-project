using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class PauseButton : MonoBehaviour
{

    private Button pauseButton;

    [Header("Event")]
    public GameEvent onPauseButtonClicked;

    private bool isPaused = true;


    private void Start()
    {
        if(Time.timeScale == 0)
            isPaused = true;
        else
            isPaused = false;

        pauseButton = this.gameObject.GetComponent<Button>();
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            onPauseButtonClicked.Raise(this, false);
            isPaused = false;
        }
        else
        {
            onPauseButtonClicked.Raise(this, true);
            isPaused = true;
        }

    }


}
