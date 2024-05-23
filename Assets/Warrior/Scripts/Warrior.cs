using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    public int moveSpeed = 1;
    public int jumpForce = 1;
    public GameObject projectileSample;

    // Update is called once per frame
    void Update()
    {
        if (!Input.anyKey)
        {
            GetComponent<Animator>().SetBool("Walk", false);
            GetComponent<Animator>().SetBool("Idle", true);
            GetComponent<Animator>().SetBool("Die", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Animator>().SetBool("Walk", true);
            GetComponent<Animator>().SetBool("Idle", false);
            GetComponent<Animator>().SetBool("Die", false);
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }        
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Animator>().SetBool("Walk", true);
            GetComponent<Animator>().SetBool("Idle", false);
            GetComponent<Animator>().SetBool("Die", false);
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<Animator>().SetTrigger("Jump");
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger("Hit");
            Invoke(nameof(Hit), 0.5f);
        }        
    }

    void Hit()
    {
        GameObject projectile = Instantiate(projectileSample, transform.position, Quaternion.identity);
        projectile.transform.parent = null;
        projectile.SetActive(true);
    }
}
