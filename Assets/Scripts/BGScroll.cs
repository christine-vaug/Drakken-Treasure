//Background Scroller
//By: Christine Vaughan
//9/23/19
//Code adapted for this game from:
//https://answers.unity.com/questions/648054/endless-2d-background.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour {
    private Vector3 backPos;
    public float width;
    private float X, Y, Z = 0f;
    private SpriteRenderer sr;
    public int speed;

    void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        speed = 5;
    }

    // Use this for initialization
    void Start () {
        width = sr.bounds.size.x;
    }
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x - (Time.deltaTime * speed), gameObject.transform.position.y, gameObject.transform.position.z);
	}

    //code adapted from https://answers.unity.com/questions/648054/endless-2d-background.html
    void OnBecameInvisible()
    {
        //calculate current position
        backPos = gameObject.transform.position;
        //calculate new position
        X = backPos.x + width * 2 - 0.5f;
        Y = backPos.y;
        Z = backPos.z;
        //move to new position when invisible
        gameObject.transform.position = new Vector3(X, Y, Z);
    }

    public void StopBGScroll()
    {
        for (int i = 0; i < 5; i++)
            speed--;
    }
}
