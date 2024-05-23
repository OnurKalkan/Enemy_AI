using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : MonoBehaviour
{
    public int speed = 10, jumpForce = 250, health = 100;
    public bool onGround, die, block, isMeleeActive, isStunned;
    public GameObject fireBallSample;
    public PlayerNo playerNo;
    public Image healthBar;
    Melee melee;

    private void Awake()
    {
        melee = transform.Find("MeleeRange").GetComponent<Melee>();
    }

    public enum PlayerNo
    {
        PlayerOne,
        PlayerTwo,
        Enemy
    }
    
    public void PlayAnimation(string animationName)
    {
        GetComponent<Animator>().SetBool("Idle", false);
        GetComponent<Animator>().SetBool("Run", false);
        GetComponent<Animator>().SetBool("Block", false);
        GetComponent<Animator>().SetBool(animationName, true);
    }

    public void HealthChange(int healthChange)
    {
        health += healthChange;
        healthBar.fillAmount = (float)health / 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!die)
        {
            //if (Input.GetKey(KeyCode.JoystickButton0))//A
            //    transform.Translate(Vector3.left * Time.deltaTime * speed);
            //if (Input.GetKey(KeyCode.JoystickButton1))//X
            //    transform.Translate(Vector3.right * Time.deltaTime * speed);
            if(playerNo == PlayerNo.PlayerOne)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {                    
                    PlayAnimation("Run");
                    if (Input.GetKey(KeyCode.A))
                        transform.Translate(Vector3.left * Time.deltaTime * speed);
                    if (Input.GetKey(KeyCode.D))
                        transform.Translate(Vector3.right * Time.deltaTime * speed);                    
                }
                if (Input.GetKeyDown(KeyCode.W) && onGround == true)
                {
                    GetComponent<Animator>().SetTrigger("Jump");
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Attack();
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    MeleeAttack();
                }
                if (Input.GetKey(KeyCode.F))
                {
                    Block();
                }
                else
                {
                    block = false;
                }
            }
            else if(playerNo == PlayerNo.PlayerTwo)
            {
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                {    
                    PlayAnimation("Run");
                    if (Input.GetKey(KeyCode.LeftArrow))
                        transform.Translate(Vector3.left * Time.deltaTime * speed);
                    if (Input.GetKey(KeyCode.RightArrow))
                        transform.Translate(Vector3.right * Time.deltaTime * speed);                    
                }
                if (Input.GetKeyDown(KeyCode.UpArrow) && onGround == true)
                {
                    GetComponent<Animator>().SetTrigger("Jump");
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
                }
                if (Input.GetKeyDown(KeyCode.RightShift))
                {
                    Attack();
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    MeleeAttack();
                }
                if (Input.GetKey(KeyCode.RightControl))
                {
                    Block();
                }
                else
                {
                    block = false;
                }
            }
            else if (playerNo == PlayerNo.Enemy)
            {
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                {
                    PlayAnimation("Run");
                    if (Input.GetKey(KeyCode.LeftArrow))
                        transform.Translate(Vector3.left * Time.deltaTime * speed);
                    if (Input.GetKey(KeyCode.RightArrow))
                        transform.Translate(Vector3.right * Time.deltaTime * speed);
                }
                if (Input.GetKeyDown(KeyCode.UpArrow) && onGround == true)
                {
                    GetComponent<Animator>().SetTrigger("Jump");
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
                }
                if (Input.GetKeyDown(KeyCode.RightShift))
                {
                    Attack();
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    MeleeAttack();
                }
                if (Input.GetKey(KeyCode.RightControl))
                {
                    Block();
                }
                else
                {
                    block = false;
                }
            }
            if (Input.anyKey == false && playerNo != PlayerNo.Enemy)
            {
                PlayAnimation("Idle");
            }
        }        
    }

    public void Attack()
    {
        GetComponent<Animator>().SetTrigger("Attack");
        CreateFireBall();
    }

    public void MeleeAttack()
    {
        GetComponent<Animator>().SetTrigger("Attack");
        if (isMeleeActive)
        {
            if (melee.enemy.GetComponent<Knight>().block == true)
            {
                melee.enemy.GetComponent<Knight>().HealthChange(-2);
                melee.enemy.GetComponent<Animator>().SetTrigger("BlockHurt");
                isStunned = true;
            }
            else
            {
                melee.enemy.GetComponent<Knight>().HealthChange(-20);
                if (melee.enemy.GetComponent<Knight>().health <= 0)
                {
                    melee.enemy.GetComponent<Knight>().PlayAnimation("Death");
                    melee.enemy.GetComponent<Knight>().die = true;
                    melee.enemy.GetComponent<Rigidbody2D>().simulated = false;
                    melee.enemy.GetComponent<BoxCollider2D>().enabled = false;
                }
                else
                {
                    //collision.gameObject.GetComponent<Player>().PlayAnimation("Hurt");
                    melee.enemy.GetComponent<Animator>().SetTrigger("Hurt");
                }
            }            
        }
    }

    public void Block()
    {
        block = true;
        PlayAnimation("Block");
    }

    void CreateFireBall()
    {
        GameObject newFireBall = Instantiate(fireBallSample, transform.position + new Vector3(0,2,0), Quaternion.identity);
        newFireBall.GetComponent<FireBall>().myPlayer = this.gameObject;
        newFireBall.transform.parent = null;
        newFireBall.SetActive(true);        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }
}
