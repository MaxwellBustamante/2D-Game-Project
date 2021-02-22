﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigateMenu : MonoBehaviour
{
   public void SwitchScene(string sceneName)
   {
       SceneManager.LoadScene(sceneName);
   }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Ya Quit, Quitch");
    }
    public void ReloadScene()
    {
        string CurrentLevel = SceneManager.GetActiveScene().name;
        SwitchScene(CurrentLevel);
    }
}
