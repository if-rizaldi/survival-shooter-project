using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINavigation : MonoBehaviour
{
    [SerializeField] private Loader.Scene nextScene;
    [SerializeField] private Loader.Scene previousScene;
    [SerializeField] private GameObject nextMenu;
    [SerializeField] private GameObject previousMenu;

    [SerializeField] private GameObject menuToDestroy;
    public void GoToNextScene()
    {
        Loader.Load(Loader.Scene.Loading);
        Loader.Load(nextScene);
    }

    public void GoToGameplay()
    {
        Loader.Load(Loader.Scene.Loading);
        Loader.Load(Loader.Scene.Gameplay);
    }

    public void GoToPreviousScene()
    {
        Loader.Load(nextScene);
    }

    public void GoToNextMenu()
    {
        nextMenu.gameObject.GetComponent<Canvas>().enabled = true;
        this.gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void BackToPreviousMenu()
    {
        previousMenu.gameObject.GetComponent<Canvas>().enabled = true;
        this.gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void DestroyMenu()
    {
        Destroy(menuToDestroy);
    }

    public void HideThisMenu()
    {
        this.gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void UnhideThisMenu()
    {
        this.gameObject.GetComponent<Canvas>().enabled = true;
    }

}
