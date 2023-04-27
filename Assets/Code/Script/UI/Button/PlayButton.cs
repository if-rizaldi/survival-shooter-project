 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
 public void OnPlayButtonClick()
 {
    Loader.Load(Loader.Scene.Loading);
    Loader.Load(Loader.Scene.Gameplay);

 }
}
