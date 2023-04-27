using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class finalScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int currentScore;
    [SerializeField] private bool isSurvivalMode;

    void Start()
    {
        if(isSurvivalMode)
        {
            currentScore = Resources.Load<PlayerData>("SurvivalModePlayerData").playerScore;
        }
        else
        {
            //currentScore = Resources.Load<PlayerData>("SurvivalModePlayerData").playerScore;
        }

        scoreText.text = "Your Score is " + currentScore; 

    }


     public void UpdateScore(Component sender, object data)
    {

        if (data is int)
        {
            int score = (int)data;
            currentScore += score;
             scoreText.text = "Your Score is " + currentScore;
        }

    }
  
}
