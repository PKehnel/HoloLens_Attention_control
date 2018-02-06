using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneManager : MonoBehaviour
{
    public void Continue()
    {
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.LoadNextLevel(); //Since levelManager doesn't get destroyed when another scene is loaded you can use it here
    }

    public void GoBack()
    {
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.LoadLastLevel(); //Since levelManager doesn't get destroyed when another scene is loaded you can use it here
    }

    public void EndExperiment()
    {
        Application.Quit();
    }
}

