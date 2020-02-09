using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    //using modified script from https://stackoverflow.com/questions/32312895/stop-camera-from-moving-on-x-axis
    public GameObject player;
    private Vector3 offset;

    public bool moving; //should the camera be moving? Usually yes, but camera is still in a boss fight.

    // Use this for initialization
    void Start()
    {
        offset = transform.position;
        moving = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(moving)
            transform.position = new Vector3(player.transform.position.x + offset.x, offset.y, offset.z);
    }
}
