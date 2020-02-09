using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour {

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0) //if there is a touch, return to main menu
        {
            GameManager.Instance.LoadLevel(0); //load the main menu
        }
    }
}
