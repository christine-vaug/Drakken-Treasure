using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

    public BGScroll[] bgScrolls;
    public void Start()
    {
        bgScrolls = GetComponentsInChildren<BGScroll>(true);
    }
	public void StopBG()
    {
        foreach (BGScroll bgs in bgScrolls)
            bgs.StopBGScroll();
    }

}
