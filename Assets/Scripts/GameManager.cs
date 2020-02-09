using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//GameManager code adapted from https://forum.unity.com/threads/help-how-do-you-set-up-a-gamemanager.131170/
public class GameManager
{
    private static GameManager instance;

    private GameManager()
    {
        // initialize your game manager here. Do not reference to GameObjects here (i.e. GameObject.Find etc.)
        // because the game manager will be created before the objects
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }

            return instance;
        }
    }

    // Add your game mananger members here
    private int currentLevel = 0; //0 is main menu, odd levels are regular levels, even levels are boss fights

    public void LoadLevel(int level)
    {
        if (currentLevel != level)
        {
            currentLevel = level;

            SceneManager.LoadScene(level, LoadSceneMode.Single);
        }
    }

    public void GameOver()
    {
        currentLevel = -1;
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    public bool isBoss()
    {
        if (currentLevel % 2 == 0)
            return true;
        else
            return false;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    //code from https://forum.unity.com/threads/change-gameobject-layer-at-run-time-wont-apply-to-child.10091/
    public void SetLayerRecursively(GameObject go, int layerNumber)
    {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}

