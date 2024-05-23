using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject pointA, pointB;
    public bool rushA, rushB;
    public float speed = 0.5f;
    public float playerDistance;
    GameObject player;
    public EnemyDifficulty difficulty;
    float maxDistance, minDistance, runSpeed, rangeDistance;
    public BehaviourStates behaviourStates;

    public enum EnemyDifficulty
    {
        Easy,
        Medium,
        Hard
    }

    public enum BehaviourStates
    {
        MeleeAttack,
        Blocking,
        RangeAttack,
        Idle
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        behaviourStates = BehaviourStates.Idle;
        if(difficulty == EnemyDifficulty.Medium)
        {
            rangeDistance = 16;
            maxDistance = 12;
            minDistance = 2.5f;
            runSpeed = 1.5f;
        }
        else if (difficulty == EnemyDifficulty.Easy)
        {
            maxDistance = 4;
            minDistance = 2.5f;
            runSpeed = 1.25f;
        }
        else if (difficulty == EnemyDifficulty.Hard)
        {
            rangeDistance = 24;
            maxDistance = 20;
            minDistance = 2.5f;
            runSpeed = 2.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Knight>().die)
        {
            playerDistance = Vector3.Distance(player.transform.position, transform.position);
            if (playerDistance <= rangeDistance && playerDistance >= maxDistance && behaviourStates == BehaviourStates.Idle)
            {
                //range attack
                behaviourStates = BehaviourStates.RangeAttack;
                StartCoroutine(RangeAttack());
            }
            if (playerDistance <= maxDistance && playerDistance >= minDistance && behaviourStates == BehaviourStates.Idle)
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed * runSpeed);
                GetComponent<Knight>().PlayAnimation("Run");
                GetComponent<Animator>().speed = 2;
            }
            else if (playerDistance < minDistance && behaviourStates == BehaviourStates.Idle)
            {
                //melee attack
                behaviourStates = BehaviourStates.MeleeAttack;
                StartCoroutine(MeleeAttack());
            }
            else if (playerDistance > rangeDistance && behaviourStates == BehaviourStates.Idle)
            {
                GetComponent<Knight>().PlayAnimation("Run");
                GetComponent<Animator>().speed = 1;
                if (rushA)
                    transform.Translate(Vector3.left * Time.deltaTime * speed);
                else if (rushB)
                    transform.Translate(Vector3.right * Time.deltaTime * speed);
            }
        }              
    }

    IEnumerator MeleeAttack()
    {
        GetComponent<Knight>().MeleeAttack();
        yield return new WaitForSeconds(0.1f);
        if (GetComponent<Knight>().isStunned)
        {
            GetComponent<Knight>().PlayAnimation("Hurt");
            yield return new WaitForSeconds(2.0f);
            behaviourStates = BehaviourStates.Idle;
            GetComponent<Knight>().PlayAnimation("Idle");
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            GetComponent<Knight>().PlayAnimation("Idle");
            yield return new WaitForSeconds(1.5f);
            GetComponent<Knight>().PlayAnimation("Block");
            GetComponent<Knight>().block = true;
            yield return new WaitForSeconds(1.5f);
            behaviourStates = BehaviourStates.Idle;
            GetComponent<Knight>().PlayAnimation("Idle");
        }        
    }

    IEnumerator RangeAttack()
    {
        GetComponent<Knight>().Attack();
        yield return new WaitForSeconds(1);
        GetComponent<Knight>().PlayAnimation("Idle");
        yield return new WaitForSeconds(1);
        behaviourStates = BehaviourStates.Idle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == pointA.gameObject)
        {
            rushB = true;
            rushA = false;
        }
        else if (collision.gameObject == pointB.gameObject)
        {
            rushB = false;
            rushA = true;
        }
    }
}
