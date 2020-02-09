using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour {
    public float speed = 8.0f;
    private Rigidbody2D rbd;
    public BossScript boss;
    public FBColor color;

    // Use this for initialization
    void Awake () {
        rbd = GetComponent<Rigidbody2D>();
        rbd.velocity = -(transform.right) * speed;
        boss = GameObject.Find("Blue").GetComponent<BossScript>();
    }

    void Start()
    {
        color = boss.fbcolor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            Destroy(gameObject);
    }

    //destroy the instance once it's offscreen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
