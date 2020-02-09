using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    private int maxHearts = 3;
    private int currentHearts;
    public GameObject[] hearts = new GameObject[3];

	// Use this for initialization
	void Start () {
        currentHearts = maxHearts;
	}

    public int GetCurrentHearts()
    {
        return currentHearts;
    }
	
	public void TakeDamage()
    {
        if(currentHearts > 0)
        {
            currentHearts--; //decrement currentHearts first so that it represents the correct index on the next line
            hearts[currentHearts].GetComponent<HeartScript>().RemoveHeart();
        }
    }
}
