using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PookieController : MonoBehaviour {

    public float moveSpeed;
    public GameObject lifeG;
    private PookieLifeScript life;
    

    private Animator anim;
    private Rigidbody2D myRigidBody;
    BoxCollider[] myColliders;

    private bool playerMoving;
    private bool playerAttacking;
    private bool playerAlive;
    private bool playerDamaged;
    private float lastMoveX;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        life = (PookieLifeScript)lifeG.GetComponent<PookieLifeScript>();
        playerAlive = true;
        playerDamaged = false;
    }


    private float cooldown = 0.5f;
    private float nextAttack;
    private float attackFrame;

	// Update is called once per frame
	void Update () {
        playerMoving = false;
        playerAttacking = false;
        if (playerAlive)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
            {
                //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
                myRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidBody.velocity.y);
                playerMoving = true;
                lastMoveX = Input.GetAxisRaw("Horizontal");
            }
            else if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f)
            {
                myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);
            }

            if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
            {
                //transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
                playerMoving = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)
            {
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0f);
            }

            if (Input.GetAxisRaw("Jump") > 0.5f && Time.time > nextAttack)
            {
                nextAttack = Time.time + cooldown;
                attackFrame = Time.time + 0.11f;
                playerAttacking = true;
            }
            
        }
        else
        {
            myRigidBody.velocity = new Vector2(0f, 0f); //stop player
        }
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetBool("PlayerAttacking", playerAttacking);
        anim.SetFloat("LastMoveX", lastMoveX);
        anim.SetBool("PlayerAlive", playerAlive);
        anim.SetBool("PlayerDamaged", playerDamaged);
        if (playerDamaged)
        {
            playerDamaged = false;
        }
    }

    public bool isAttacking()
    {
        return playerAttacking || anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack") || Time.time < attackFrame;
    }

    public bool isAlive()
    {
        return playerAlive;
    }


    private float lifeCooldown = 3.0f; //test
    private float nextLife;

    public void getDamaged(int damage, GameObject damager)
    {
        playerDamaged = true;
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        Vector2 actualPosition = body.transform.position;
        Vector2 orientation = (actualPosition - (Vector2)damager.transform.position).normalized;
        body.AddForce(orientation * 100000);
        nextLife = Time.time + lifeCooldown;
        life.takeDamage(damage);          
        if (life.getLifeLeft() <= 0)
        {
            playerAlive = false;
            body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
    }
}
