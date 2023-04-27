using UnityEngine;
using TMPro;

public class TextScore : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private int currentScore;
    [SerializeField] private bool isSurvivalMode;

    void Start()
    {
        scoreText = this.gameObject.GetComponent<TextMeshProUGUI>();
        if(isSurvivalMode)
        {
            currentScore = Resources.Load<PlayerData>("SurvivalModePlayerData").playerScore;
        }
        else
        {
            currentScore = 0;
        }

        scoreText.text = "Score : " + currentScore; 

    }



    public void UpdateScore(Component sender, object data)
    {

        if (data is int)
        {
            int score = (int)data;
            currentScore += score;
            scoreText.text = "Score : " + currentScore; 
        }

    }
}
