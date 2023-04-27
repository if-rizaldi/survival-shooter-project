using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{

    public enum Scene{
        Gameplay,
        Loading,
        MainMenu,
        UpgradeMenu,
        PreparationMenu
    }

    private static Action onLoaderCallback;

    public static void Load(Scene scene)
    {
      

      onLoaderCallback = () =>{
        SceneManager.LoadScene(scene.ToString());
      };

      SceneManager.LoadScene(scene.ToString());

    }

    public static void LoaderCallback()
    {
      if(onLoaderCallback != null)
      {
        onLoaderCallback();
        onLoaderCallback = null;
      }

    }
    
}
