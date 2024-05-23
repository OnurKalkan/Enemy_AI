using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerOne, playerTwo;

    // Update is called once per frame
    void Update()
    {
        if(playerOne.transform.position.x > playerTwo.transform.position.x)
        {
            playerTwo.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            playerOne.transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
        }
        else
        {
            playerTwo.transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
            playerOne.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
    }
}
