using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChangeScript : MonoBehaviour {

    public PlayerScript playerScript;
    public FBColor color; //FBColor enum found in PlayerScript
    public FlameUIScript fs;

    void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        fs = GameObject.Find("CurrentFlame").GetComponent<FlameUIScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") //if the player hits a fireball, the player gains that color
        {
            playerScript.activeFireball = playerScript.fireballs[(int)color];
            playerScript.fbcolor = color;
            fs.ChangeColor(color);
            Destroy(gameObject);
        }
    }
}
