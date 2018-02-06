using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;
using System;
using System.Collections.Generic;
using System.Linq;


public class LevelManager : MonoBehaviour
{
    public int testLevels = 6;
    private Random rnd;
    public int nextTestLevel = 0;
    public int nextLevel = 0;
    private List<int> testLevelList;
    private List<int> levelList;
    private List<int> experimentList;
    private int entry = 0;


    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        rnd = new Random();
        RandomizeLevelOrder();
    }
    private void RandomizeLevelOrder() //Randomize order using Fisher-Yates shuffle algorithm
    {
        Debug.Log("At the beginning");
        levelList = Enumerable.Range(1, 26).ToList();
        Debug.Log("levelList initialized");
        experimentList = new List<int>();
        Debug.Log("experimentList initialized");
        experimentList.Add(14);
        experimentList.Add(16);
        experimentList.Add(18);
        experimentList.Add(20);
        experimentList.Add(22);
        experimentList.Add(24);
        int n = experimentList.Count;
        Debug.Log("Length: " + n);
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            int value = experimentList[k];
            experimentList[k] = experimentList[n];
            experimentList[n] = value;
            Debug.Log(n + " ExperimentList[n]: " + experimentList[n]);
        }
        Debug.Log("ExperimentList randomized");
        levelList[13] = experimentList[0];
        levelList[15] = experimentList[1];
        levelList[17] = experimentList[2];
        levelList[19] = experimentList[3];
        levelList[21] = experimentList[4];
        levelList[23] = experimentList[5];
        levelList[14] = experimentList[0]+1;
        levelList[16] = experimentList[1]+1;
        levelList[18] = experimentList[2]+1;
        levelList[20] = experimentList[3]+1;
        levelList[22] = experimentList[4]+1;
        levelList[24] = experimentList[5]+1;
        for (int i = 0; i < levelList.Count; i++)
        {
            Debug.Log("LevelList at i: " + i + " = " + levelList[i]);
        }
    }

    public void LoadNextLevel()
    {
        Debug.Log("Scene: " + entry + " " + levelList[entry]);
        SceneManager.LoadScene(levelList[entry]);
        entry++;
    }  //Load 3 entry=4 ; entry=entry-2 Load 2 entry++; entry=3 Load 3 entry=4;

    public void LoadLastLevel()
    {
        entry = entry - 2; //-2 because instead of loading the next or the current scene we want the last one = -2
        Debug.Log("Scene: " + entry + " " + levelList[entry]);
        SceneManager.LoadScene(levelList[entry]);
        entry++;
    }
    /*
    private void RandomizeLevelOrder() //Randomize order using Fisher-Yates shuffle algorithm
    {
        levelList = Enumerable.Range(1, testLevels).ToList();
        int n = levelList.Count;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            int value = levelList[k];
            levelList[k] = levelList[n];
            levelList[n] = value;
        }
        for (int i = 0; i < levelList.Count; i++)
        {
            Debug.Log(levelList[i]);
        }
        testLevelList = new List<int>(levelList);
        for (int i = 0; i < levelList.Count; i++) 
        {
            levelList[i] = levelList[i] + testLevels; 
        }           
    }

    public void LoadNextLevel()
    {
        if (nextTestLevel == testLevelList.Count) //Case when all TestScenes are played through and Experimentmenu should pop up 
        {
            nextTestLevel++; //If we wouldn't increase it here it would just go in the case above again
            SceneManager.LoadScene(13);
        }
        else { 

        if (nextTestLevel < testLevelList.Count)
        {
            Debug.Log("Loading: " + testLevelList[nextTestLevel] + " Next Level:" + nextTestLevel);
            SceneManager.LoadScene(testLevelList[nextTestLevel]);
            nextTestLevel++;
        }  
        else
        {
            if (nextLevel < levelList.Count)
            {
                Debug.Log("nextTestLevel: " + nextLevel + "Length: " + levelList.Count);
                SceneManager.LoadScene(levelList[nextLevel]);
                nextLevel++;
            }
            else
            {
                SceneManager.LoadScene(14);
            }
        }
    }
    }
    */
}
