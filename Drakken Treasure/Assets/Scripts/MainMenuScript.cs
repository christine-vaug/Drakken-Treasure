using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void LoadScene(int level)
    {
        GameManager.Instance.LoadLevel(level);
    }
}