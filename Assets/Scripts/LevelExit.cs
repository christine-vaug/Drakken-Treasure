using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour {

    public DialogueTrigger dtgr;
    public DialogueTrigger bossdtgr;
    public DialogueManager dmgr;

    private bool reachedEnd;
    private bool loadBoss = false;

    void Awake()
    {
        reachedEnd = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") //if the player hits the exit, the level is complete
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            FBColor fbc = player.GetComponent<PlayerScript>().fbcolor;
            player.GetComponent<PlayerScript>().enabled = false;
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            player.GetComponent<Animation>().CrossFade("SJ001_wait");
            reachedEnd = true;
            if(GameManager.Instance.GetCurrentLevel() == 1 && fbc == FBColor.blue)
            {
                loadBoss = true;
                bossdtgr.TriggerDialogue();
            }
            else
                dtgr.TriggerDialogue();
        }
    }

    void FixedUpdate()
    {
        if(reachedEnd && !dmgr.dialogueRunning)
        {
            if (loadBoss)
                GameManager.Instance.LoadLevel(2); //load boss
            else
                ReturnToMainMenu();   //complete level
        }
    }

    void ReturnToMainMenu()
    {
        GameManager.Instance.LoadLevel(0); //load the main menu
    }
}
