using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class controls the pookie movement, life and collider.
/// </summary>
public class PookieController : MonoBehaviour
{

    private const float LASER_COOLDOWN = 0.3f;
    private const float ATTACK_COOLDOWN = 0.0f;

    public static int cash = 0;
    public static int atkModifier = 0;

    public float moveSpeed;
    public GameObject lifeG, leftLaser, rightLaser;
    private PookieLifeScript life;
    private Transform leftFirePos, rightFirePos;

    private Text cashCount;

    private Animator anim;
    private Rigidbody2D myRigidBody;
    BoxCollider[] myColliders;

    private bool playerMoving;
    private bool playerAttacking;
    private bool playerAlive;
    private bool playerDamaged;
    private bool facingRight;
    private float lastMoveX;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        life = (PookieLifeScript)lifeG.GetComponent<PookieLifeScript>();
        playerAlive = true;
        playerDamaged = false;
        leftFirePos = transform.Find("LeftFirePos");
        rightFirePos = transform.Find("RightFirePos");
        facingRight = false;
        cashCount = GameObject.Find("Cash").GetComponent<Text>();
        SetText();
    }

    private float nextAttack;
    private float attackFrame;
    private float nextLaser;

    // Update is called once per frame
    void Update()
    {
        playerMoving = false;
        playerAttacking = false;
        if (playerAlive)
        {
            if (Input.GetAxisRaw("Horizontal") >= 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
            {
                if (Input.GetAxisRaw("Horizontal") >= 0.5f)
                {
                    facingRight = true;
                    lastMoveX = 1;
                }
                else
                {
                    facingRight = false;
                    lastMoveX = -1;
                }
                //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
                myRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidBody.velocity.y);
                playerMoving = true;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") >= -0.5f)
            {
                myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);
            }

            if (Input.GetAxisRaw("Vertical") >= 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
            {
                if (Input.GetAxisRaw("Horizontal") >= 0.5f) facingRight = true;
                if (Input.GetAxisRaw("Horizontal") < -0.5f) facingRight = false;
                //transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
                playerMoving = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") >= -0.5f)
            {
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0f);
            }

            if (Input.GetAxisRaw("Jump") >= 0.1f && Time.time > nextAttack)
            {
                nextAttack = Time.time + ATTACK_COOLDOWN;
                attackFrame = Time.time + 0.11f;
                playerAttacking = true;
            }

            if (Input.GetAxisRaw("Fire1") > 0.5f && Time.time > nextLaser)
            {
                nextLaser = Time.time + LASER_COOLDOWN;
                Fire();
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

        SetText();

    }

    /// <summary>
    /// Gets all the pookie weapons (tagged with Sword).
    /// </summary>
    /// <returns></returns>
    public GameObject[] getWeapons()
    {
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Sword");
        return weapons;
    }

    public void addAttack(int attackBonus)
    {
        PookieController.atkModifier += attackBonus;
    }

    public int getAtkModifier()
    {
        return PookieController.atkModifier;
    }

    public void setCash(int cash)
    {
        PookieController.cash = cash;
    }

    public void addCash(int coinValue)
    {
        PookieController.cash += coinValue;
    }

    public void removeCash(int value)
    {
        PookieController.cash -= value;
    }

    public int getCash()
    {
        return PookieController.cash;
    }

    public void SetText()
    {
        cashCount = GameObject.Find("Cash").GetComponent<Text>();
        cashCount.text = "Cash: " + PookieController.cash;
    }

    public void SetText(int newCash)
    {
        cashCount = GameObject.Find("Cash").GetComponent<Text>();
        cashCount.text = "Cash: " + newCash;
    }

    //fires a laser
    void Fire()
    {
        if (facingRight)
        {
            SoundManager.instance.playSingle(SoundManager.instance.laser);
            Instantiate(rightLaser, rightFirePos.position, Quaternion.identity);
        }
        if (!facingRight)
        {
            SoundManager.instance.playSingle(SoundManager.instance.laser);
            Instantiate(leftLaser, leftFirePos.position, Quaternion.identity);
        }
    }

    public BulletCtrl[] getLeftLasers()
    {
        BulletCtrl[] lasers = leftLaser.GetComponentsInChildren<BulletCtrl>();
        return lasers;
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
    /// <summary>
    /// Damages pookie with a certain amount of damage by a damager.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damager"></param>
    public void getDamaged(int damage, GameObject damager)
    {
        playerDamaged = true;
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        Collider2D collider = GetComponent<Collider2D>();
        Vector2 actualPosition = body.transform.position;
        Vector2 orientation = (actualPosition - (Vector2)damager.transform.position).normalized;
        body.AddForce(orientation * 100000);
        nextLife = Time.time + lifeCooldown;
        life.takeDamage(damage);
        SoundManager.instance.playSingle(SoundManager.instance.hit);
        if (life.getLifeLeft() <= 0)
        {
            playerAlive = false;
            body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            collider.enabled = false;
            SoundManager.instance.playSingle(SoundManager.instance.pookieDeath);
            ButtonScript button = (ButtonScript)GameObject.FindGameObjectWithTag("Button").GetComponent<ButtonScript>();
            button.showGameOver();
        }
    }
}
