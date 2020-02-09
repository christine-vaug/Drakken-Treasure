////Player Movement code
//By: Christine Vaughan
//9/17/19

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private CharacterController2D controller;
    private Animation anim;
    private PlayerHealth ph;
    public EnvironmentManager em;

    private float maxSpeed;
    private float runSpeed;

    private float horizontalMove = 0f;
    private bool jump = false;
    //private bool usedClaw = false;
    private bool usedFireball = false;
    private Vector3 lockRotation = new Vector3(0, 90, 0); //keeps player from spinning

    //store fireball prefab
    public GameObject[] fireballs = new GameObject[5];
    public GameObject activeFireball;
    public FBColor fbcolor;

    private bool instancedfb = false;

    //touch controls
    private Vector2 startPos;
    private Vector2 endPos;

    //damage controls
    private bool isHurt = false;
    private int damageTimer = 4;

    private bool isDead = false;

    public bool isBossFight;

    void Awake()
    {
        anim = GetComponent<Animation>();
        controller = GetComponent<CharacterController2D>();
        ph = GameObject.FindGameObjectWithTag("Health").GetComponent<PlayerHealth>();
        isBossFight = GameManager.Instance.isBoss();
        if (isBossFight)
            maxSpeed = 0.0f;
        else
            maxSpeed = 30.0f;
    }

    void Start()
    {
        //transform.position = new Vector3(-4, 0, -3);
        if (isBossFight)
        {
            switch(GameManager.Instance.GetCurrentLevel())
            {
                case 2:
                    fbcolor = FBColor.blue;
                    activeFireball = fireballs[(int)FBColor.blue];
                    break;
                default:
                    break;
            }
        }
        else
        {
            fbcolor = FBColor.orange;
            activeFireball = fireballs[(int)FBColor.orange];
        }
        isHurt = isDead = false;
        runSpeed = maxSpeed;
    }

    void Update ()
    {
        //set the amount the dragon is moving by - right now it's just run speed, but might need to adjust later
        horizontalMove = runSpeed;

        //handle "running" animation
        if (Mathf.Abs(horizontalMove) > 1 || (isBossFight && !isHurt && !isDead))
            anim.CrossFade("SJ001_run");
        else if(!isHurt && !isDead)
            anim.CrossFade("SJ001_wait");

        //if the player has been damaged, make the character blink
        if(isHurt)
        {
            damageTimer -= 1;
            if(damageTimer == 0)
            {
                GetComponentInChildren<SkinnedMeshRenderer>().enabled = !GetComponentInChildren<SkinnedMeshRenderer>().enabled;
                damageTimer = 4;
            }
        }

        //once the damged effect is done, make sure the player is visible again
        if (!isHurt)
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;

        if (Input.touchCount > 0) //if there is a touch, register it
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on touch phase.
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    endPos = touch.position;
                    checkTouch(endPos - startPos);
                    break;
            }
        }

        //if (Input.GetButtonDown("Jump"))
        //{
        //    jump = true;
        //}

        //if (Input.GetButtonDown("Fire2"))
        //{
        //    usedClaw = true;
        //}
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    usedFireball = true;
        //    instancedfb = false;
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    GameManager.Instance.LoadLevel(2);
        //}
    }

    void checkTouch(Vector2 direction)
    {
        //if swiped to the right, use fireball
        if(direction.x > 20.0f)
        {
            usedFireball = true;
            instancedfb = false;
            //usedClaw = false;
            jump = false;
        }      
        //if swiped to the left, use claw
        //else if (direction.x < -20.0f)
        //{
        //    usedFireball = false;
        //    usedClaw = true;
        //    jump = false;
        //}
        //if a tap, jump
        else
        {
            usedFireball = false;
            //usedClaw = false;
            jump = true;
        }
    }

    void FixedUpdate()
    {
        transform.localEulerAngles = lockRotation; //we don't want the dragon spinning

        //if (usedClaw) //use a claw attack
        //{
        //    StartCoroutine("ClawAttack");
        //}
        //else 
        if(usedFireball) //use a fire ball
        {
            StartCoroutine("Fireball");
        }
        else //send the info to the character controller
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
            jump = false;
            //usedClaw = false;
            usedFireball = false;
        }
    }

    //coroutine for a claw attack
    //IEnumerator ClawAttack()
    //{
    //    anim.CrossFade("SJ001_skill1"); //play the animation
    //    yield return new WaitForSeconds(1.2f); //give the coroutine enough time to do the animation
    //    usedClaw = false; //claw attack has been done
    //}

    IEnumerator Fireball()
    {
        anim.CrossFade("SJ001_skill2"); //play the animation
        yield return new WaitForSeconds(0.5f);
        if (!instancedfb)
        {
            Vector3 attackPos = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
            Instantiate(activeFireball, attackPos, Quaternion.identity);
            instancedfb = true;
        }
        yield return new WaitForSeconds(0.9f); //give the coroutine enough time to do the animation
        usedFireball = false; //fireball attack done
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (ph.GetCurrentHearts() > 1) //if player has more than 1 heart left, take damage
            {
                if (!isHurt) //check if we've set the bool yet - don't want to trigger more than once
                {
                    runSpeed = 0f; //temporarily stop the dragon
                    GetHurt();
                }
            }
            else if(ph.GetCurrentHearts() == 1) //else we're about to lose our last heart and get a game over
            {
                if (!isDead) //check if we've set the bool yet - don't want to trigger more than once
                {
                    runSpeed = 0f; //temporarily stop the dragon
                    Death();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boss Attack")
        {
            if (ph.GetCurrentHearts() > 1) //if player has more than 1 heart left, take damage
            {
                if (!isHurt) //check if we've set the bool yet - don't want to trigger more than once
                {
                    runSpeed = 0f; //temporarily stop the dragon
                    GetHurt();
                }
            }
            else if (ph.GetCurrentHearts() == 1) //else we're about to lose our last heart and get a game over
            {
                if (!isDead) //check if we've set the bool yet - don't want to trigger more than once
                {
                    runSpeed = 0f; //temporarily stop the dragon
                    Death();
                }
            }
        }
    }

    private void GetHurt()
    {
        isHurt = true; //set the bool so that we don't run this multiple times while the coroutine is running
        ph.TakeDamage(); //a heart will disappear
        GameManager.Instance.SetLayerRecursively(gameObject, 9); //puts player on special layer so that enemies will go through - temporary invincibility
        StartCoroutine("Hurt");
    }

    IEnumerator Hurt()
    {
        anim.CrossFade("SJ001_hurt"); //play the animation
        yield return new WaitForSeconds(0.8f); //give the coroutine enough time to do the animation
        runSpeed = maxSpeed; //start moving again
        yield return new WaitForSeconds(1.5f); //give player time to move through enemy before "invincibilty" wears off
        GameManager.Instance.SetLayerRecursively(gameObject, 0); //go back to default layer that can interact with enemies again
        isHurt = false; //hurt animation has been done
    }

    private void Death()
    {
        isDead = true; //set the bool so that we don't run this multiple times while the coroutine is running
        if(isBossFight)
            em.StopBG();
        ph.TakeDamage(); //a heart will disappear
        GameManager.Instance.SetLayerRecursively(gameObject, 9); //puts player on special layer so that enemies will go through - temporary invincibility
        StartCoroutine("Die");
    }

    IEnumerator Die()
    {
        anim.CrossFade("SJ001_die"); //play the animation
        yield return new WaitForSeconds(3.0f); //give the coroutine enough time to do the animation
        GameManager.Instance.GameOver(); //show game over text
    }
}
