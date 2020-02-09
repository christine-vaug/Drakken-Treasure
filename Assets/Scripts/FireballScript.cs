using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour {
    public float speed = 8.0f;
    private Rigidbody2D rbd;
    public MetalonScript enemyScript;
    public PlayerScript playerScript;
    public BossAttack baScript;
    public FBColor color;
    public bool big; //is the fireball big enough to hurt boss?

    // Use this for initialization
    void Awake()
    {
        rbd = GetComponent<Rigidbody2D>();
        rbd.velocity = transform.right * speed;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    void Start()
    {
        color = playerScript.fbcolor;
        big = false;
    }

    private void OnTriggerEnter2D(Collider2D other) //interacting with other kinematic objects
    {
        if (other.gameObject.tag != "Player") //need to do this so the fireball is not effected by the player's colliders
        {
            if (other.gameObject.tag == "Boss Attack")
            {
                baScript = other.gameObject.GetComponent<BossAttack>();
                if ((int)color == (int)baScript.color) //check if the fireball color matches the enemy's fireball color
                {
                    Destroy(other.gameObject);
                    Vector2 enlarge = transform.localScale;
                    enlarge.x *= 2.5f;
                    enlarge.y *= 2.5f;
                    transform.localScale = enlarge;
                    big = true;
                }
                //else the attack does nothing
                else
                    Destroy(gameObject);
            }
            else if (other.gameObject.tag == "Boss" || big) //if a big flame hits the boss
            {
                rbd.velocity = new Vector2(0f, 0f);
                Destroy(GetComponent<CircleCollider2D>());
                gameObject.transform.position = other.gameObject.transform.position;
                Vector2 engulf = transform.localScale;
                engulf.x *= 2.5f;
                engulf.y *= 2.5f;
                transform.localScale = engulf;
                Destroy(gameObject, 1.3f);
            }
            else
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Player") //need to do this so the fireball is not effected by the player's colliders
        {
            //if the attack hits the opponent, do damage to them
            if (other.gameObject.tag == "Enemy")
            {
                rbd.velocity = new Vector2(0f, 0f);
                enemyScript = other.gameObject.GetComponent<MetalonScript>();
                if ((int)color == (int)enemyScript.color) //check if the fireball color matches the enemy
                {
                    Destroy(GetComponent<CircleCollider2D>());
                    gameObject.transform.position = other.gameObject.transform.position;
                    Vector2 engulf = transform.localScale;
                    engulf.x *= 2.5f;
                    engulf.y *= 2.5f;
                    transform.localScale = engulf;
                    enemyScript.Death(); //start the death sequence
                    Destroy(gameObject, 1.3f);
                }
                //else the attack does nothing
                else
                    Destroy(gameObject);
            }
            else
                Destroy(gameObject);
        }
    }

    //destroy the instance once it's offscreen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
