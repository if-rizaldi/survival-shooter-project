using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextHealth : MonoBehaviour
{
    [SerializeField] private GameObject slider;
    private RectTransform bar;
    private Image barImage;
    private float maxPlayerHealth = 100f;
    private float barMaxWidth;
    private float currentValue;
    private TextMeshProUGUI healthText;
    private Color maxhealthColor;

    void Start()
    {
        if (!slider)
            bar = this.gameObject.GetComponent<RectTransform>();
        else
            bar = slider.GetComponent<RectTransform>();

        barMaxWidth = bar.rect.width;

        if (GameObject.FindWithTag("Player").GetComponent<PlayerStats>())
            maxPlayerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerStats>().playerMaxHealth;

        barImage = slider.GetComponent<Image>();

        healthText = this.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        healthText.text = maxPlayerHealth.ToString();

        maxhealthColor = this.transform.GetChild(0).GetComponent<Image>().color;
    }

    public void SetHealth(float value)
    {
        currentValue = value;
        float deltaWidth = currentValue * (barMaxWidth / maxPlayerHealth);

        Vector2 sizeHealth = bar.sizeDelta;
        sizeHealth.x = deltaWidth;
        bar.sizeDelta = sizeHealth;

        if (bar.rect.width >= 0.35f * barMaxWidth)
        {
            barImage.color = maxhealthColor;
        }
        if (bar.rect.width <= 0.35f * barMaxWidth && bar.rect.width >= 0.15f * barMaxWidth)
        {
            barImage.color = Color.yellow;
        }
        if (bar.rect.width <= 0.15f * barMaxWidth)
        {
            barImage.color = Color.red;
        }



        healthText.text = value.ToString();

    }

    public void UpdateHealth(Component sender, object data)
    {

        if (data is float)
        {
            float amount = (float)data;
            SetHealth(amount);
        }

    }


}
