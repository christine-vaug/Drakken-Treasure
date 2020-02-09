using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalonScript : MonoBehaviour {
    public enum EnemyColor
    {
        blue,
        green,
        orange,
        red,
        yellow,
        purple
    }

    public CharacterController2D controller;
    public Animator anim;

    public float runSpeed = 15f;
    private float horizontalMove = 0f;
    private Vector3 lockRotation = new Vector3(0, -90, 0); //keeps metalon from spinning
    public bool dead = false;

    public EnemyColor color;

    void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
    }

    // Use this for initialization
    void Start () {
        controller.m_FacingRight = false; //enemies move left
        anim.SetBool("Walk Forward", true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        horizontalMove = -runSpeed; //it will be moving left
    }

    void FixedUpdate()
    {
        //send the info to the character controller
        //use Time.fixedDeltaTime to account for the fact that Fixed
        controller.Move(horizontalMove * Time.fixedDeltaTime, false);
        transform.localEulerAngles = lockRotation;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {

    //    }
    //}

    public void Death()
    {
        runSpeed = 0; //stop moving
        GameManager.Instance.SetLayerRecursively(gameObject, 9); //put on invincible layer so that player won't get hurt running into corpse
        anim.SetBool("Walk Forward", false); //metalon stops walking
        StartCoroutine("DeathAnim");
    }

    IEnumerator DeathAnim()
    {
        anim.SetBool("Die", true); //metalon stops walking
        yield return new WaitForSeconds(1.2f); //give the coroutine enough time to do the animation
        dead = true;
        Destroy(gameObject);
    }
}
