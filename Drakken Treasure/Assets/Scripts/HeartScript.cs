using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartScript : MonoBehaviour {
    public Sprite full;
    public Sprite empty;

	// Use this for initialization
	void Start () {
        GetComponent<Image>().sprite = full;
	}
	
	public void RemoveHeart()
    {
        GetComponent<Image>().sprite = empty;
    }

    public void RefillHeart()
    {
        GetComponent<Image>().sprite = full;
    }
}
