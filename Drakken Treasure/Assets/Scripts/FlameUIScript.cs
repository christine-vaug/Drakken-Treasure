using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FBColor
{
    blue,
    green,
    orange,
    red,
    yellow
}


public class FlameUIScript : MonoBehaviour {
    public Sprite[] flames = new Sprite[5];
    private Sprite currentFlame;
    private Image image;
    
    void Awake()
    {
        image = GetComponent<Image>();
    }

	// Use this for initialization
	void Start () {
        if(GameManager.Instance.GetCurrentLevel() == 2)
            currentFlame = flames[(int)FBColor.blue];
        else
            currentFlame = flames[(int)FBColor.orange];
        image.sprite = currentFlame;
    }
	
	public void ChangeColor(FBColor color)
    {
        currentFlame = flames[(int)color];
        image.sprite = currentFlame;
    }
}
