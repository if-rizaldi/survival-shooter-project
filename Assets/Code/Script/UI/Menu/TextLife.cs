using TMPro;
using UnityEngine;

public class TextLife : MonoBehaviour
{
    private TextMeshProUGUI lifeText;
    private int currentLife;
    private int lifeBuff;
    [SerializeField] private bool isSurvivalMode;


    void Awake()
    {

        lifeText = this.gameObject.GetComponent<TextMeshProUGUI>();

        PlayerData player;


        if (isSurvivalMode)
        {
            player = Resources.Load<PlayerData>("SurvivalModePlayerData");
            currentLife = player.lifeAmount;
            lifeBuff = player.lifeBuff;
        }
        else
        {
            //currentLife = Resources.Load<PlayerData>("StoryModePlayerData").lifeAmount;
        }

        DisplayLife();

    }


    // Update is called once per frame
    public void UpdateLife(Component sender, object data)
    {
        if (data is int)
        {
            int life = (int)data;
            currentLife = life;
            DisplayLife();
            Debug.Log("UpdateLife sebesar " + data);

        }
        

    }

    public void LifeBuff(Component sender, object data)
    {
        if (data is int)
        {
            int amount= (int)data;
            lifeBuff = amount;
            DisplayLife();
            Debug.Log("LifeBuff sebesar" + data + "dengan currentLife" + currentLife);

        }
    }

    void DisplayLife()
    {
        if (lifeBuff > 0)
            lifeText.text = "Life :" + currentLife + "+" + lifeBuff;
        else
            lifeText.text = "Life :" + currentLife;

    }
}
