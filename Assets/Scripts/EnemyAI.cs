using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allows the enemy to chase the main character.
/// </summary>
public class EnemyAI : MonoBehaviour {

    private GameObject self; //attached enemy object
    private Enemy selfEnemy; //attached enemy script
    private Animator anim;  //animator
    private PookieController pookie; //to interact with the player in game
    private Transform player;   //the player in the field  
    private Rigidbody2D myRigidBody;
    private bool moving;    // if the enemy is moving
    private float lastMove; //last face direction
    private bool isDamage;


    // Use this for initialization
    void Start () {
        self = this.gameObject;
        selfEnemy = (Enemy)self.GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pookie = (PookieController)GameObject.FindGameObjectWithTag("Player").GetComponent<PookieController>();
        myRigidBody = GetComponent<Rigidbody2D>();
        isDamage = false;
	}
	
	// Update is called once per frame
	void Update () {
        moving = false;
        if(player != null && transform != null)
        {
            Vector2 diff = player.position - transform.position;
            if (Vector2.Distance(transform.position, player.position) <= selfEnemy.getSightRange() && selfEnemy.isAlive())
            {
                chasePookie(diff);
            }
            anim.SetBool("isMoving", moving);
            anim.SetBool("isDead", !selfEnemy.isAlive());
            //anim.SetBool("IsDamage", isDamage);
            if (isDamage)
            {
                //isDamage = false;
            }
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
    }
    
    private void chasePookie(Vector2 diff)
    {      
        if (diff.x > -0.7 && diff.x < 0.7)
        {

        } else if(diff.x >= 0.7)
        {
            //transform.position += transform.right * selfEnemy.getMovementSpeed() * Time.deltaTime;
            myRigidBody.velocity = new Vector2(1* selfEnemy.getMovementSpeed(),myRigidBody.velocity.y);
            anim.SetFloat("MoveX", 1);
            lastMove = 1;
            moving = true;
        }
        else
        {
            //transform.position += -transform.right * selfEnemy.getMovementSpeed() * Time.deltaTime;
            myRigidBody.velocity = new Vector2(-1 * selfEnemy.getMovementSpeed(), myRigidBody.velocity.y);
            anim.SetFloat("MoveX", -1);
            lastMove = -1;
            moving = true;
        }
        anim.SetFloat("LastMoveX", lastMove);
        if (diff.y > -0.7 && diff.y < 0.7)
        {
            //TO-DO Verify Offset
        }
        else if (diff.y >= 0.7)
        {
            //transform.position += transform.up * selfEnemy.getMovementSpeed() * Time.deltaTime;
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 1 * selfEnemy.getMovementSpeed());
            moving = true;
        }
        else
        {
            //transform.position += -transform.up * selfEnemy.getMovementSpeed() * Time.deltaTime;
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, -1 * selfEnemy.getMovementSpeed());
            moving = true;
        }
    }  

    /// <summary>
    /// Deals damage to the player.
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            pookie = (PookieController)collision.gameObject.GetComponent(typeof(PookieController));   
            if (pookie.isAlive() && selfEnemy.isAlive())
            {
                pookie.getDamaged(selfEnemy.getDamage(), self);
            }
        }
    }
}

