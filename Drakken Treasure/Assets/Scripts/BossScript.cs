using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {
    //launch fireball from jaw3 position
    //attach colliders to specific parts of rig, like chest, so the colliders will move with the rig
    private Animator anim;
    private Vector3 lockRotation = new Vector3(0, -90, 0); //keeps from spinning

    public int maxHealth;
    private int currentHealth;
    private bool isHurt, isDead;
    private int damageTimer = 4;

    public bool isAttacking;
    private int temp;
    private bool instancedfb = false;

    public GameObject jawPos;
    public GameObject activeFireball;
    public FBColor fbcolor;
    private Vector3 attackPos;

    public EnvironmentManager em;
    public DialogueManager dmgr;
    public DialogueTrigger dtgr;
    public GameObject gem;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start ()
    {
        isAttacking = false;
        isHurt = isDead = false;
        anim.SetBool("isLanding", false);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("landed", false);
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if the player has been damaged, make the character blink
        if (isHurt)
        {
            damageTimer -= 1;
            if (damageTimer == 0)
            {
                GetComponentInChildren<SkinnedMeshRenderer>().enabled = !GetComponentInChildren<SkinnedMeshRenderer>().enabled;
                damageTimer = 4;
            }
        }

        //once the damged effect is done, make sure the player is visible again
        if (!isHurt)
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;

        if (!isAttacking && !isDead)
        {
            temp = Random.Range(1, 101);
            if (temp <= 40) //if random number is <= 40, start an attack
            {
                isAttacking = true;
                instancedfb = false;
                StartCoroutine("FireballAttack");
            }
            else
            {
                isAttacking = false;
            }
        }
	}

    void FixedUpdate()
    {
        transform.localEulerAngles = lockRotation; //we don't want the dragon spinning

        if (isDead && !dmgr.dialogueRunning)
        {
            if (gem.transform.position.y > 0)
            {
                gem.transform.position = new Vector3(gem.transform.position.x, gem.transform.position.y - (Time.deltaTime * 5), gem.transform.position.z);
            }
            //coroutine to show getting sapphire
            if (gem.transform.position.y <= 0)
                StartCoroutine("GetGem");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //hurt boss
        if (other.gameObject.tag == "Fireball")
        {
            if (other.gameObject.GetComponent<FireballScript>().big) //if the fireball is big enough to damage the boss
            {
                if (!isHurt) //check if we've set the bool yet - don't want to trigger more than once
                {
                    GetHurt();
                }
            }
            if(currentHealth == 0)
            {
                if (!isDead) //check if we've set the bool yet - don't want to trigger more than once
                {
                    Death();
                }
            }
        }
    }

    private void GetHurt()
    {
        isHurt = true; //set the bool so that we don't run this multiple times while the coroutine is running
        currentHealth--;
        GameManager.Instance.SetLayerRecursively(gameObject, 9); //puts player on special layer so that enemies will go through - temporary invincibility
        StartCoroutine("Hurt");
    }

    IEnumerator Hurt()
    {
        yield return new WaitForSeconds(1.5f); //make boss invincible for a little while
        GameManager.Instance.SetLayerRecursively(gameObject, 0); //go back to default layer that can interact with player again
        isHurt = false; //hurt animation has been done
    }

    private void Death()
    {
        isDead = true; //set the bool so that we don't run this multiple times while the coroutine is running
        em.StopBG();
        isAttacking = false;
        anim.SetBool("isAttacking", false);
        GameManager.Instance.SetLayerRecursively(gameObject, 9); //puts player on special layer so that enemies will go through - temporary invincibility
        StartCoroutine("Die");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().enabled = false;
        dtgr.TriggerDialogue();
    }

    IEnumerator Die()
    {
        anim.SetBool("isLanding", true); //play landing animation
        yield return new WaitForSeconds(0.25f); //give the coroutine enough time to do the animation
        anim.SetBool("isLanding", false);
        anim.SetBool("landed", true);
    }

    IEnumerator FireballAttack()
    {
        anim.SetBool("isAttacking", isAttacking);
        yield return new WaitForSeconds(1.25f);
        if (!instancedfb)
        {
            for(int i = 0; i < 3; i++)
            {
                attackPos = new Vector3(jawPos.transform.position.x + 0.5f, jawPos.transform.position.y, -3);
                Instantiate(activeFireball, attackPos, Quaternion.identity);
                yield return new WaitForSeconds(0.25f);
            }
            instancedfb = true;
        }
        yield return new WaitForSeconds(1.25f); //give the coroutine enough time to do the animation
        isAttacking = false; //fireball attack done
        anim.SetBool("isAttacking", isAttacking);
    }

    IEnumerator GetGem()
    {
        yield return new WaitForSeconds(2.0f);
        GameManager.Instance.LoadLevel(0); //load the main menu
    }
}
