using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public int timer = 90;
    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        timerText.text = timer.ToString();
        StartCoroutine(TimerChange());
    }

    IEnumerator TimerChange()
    {
        yield return new WaitForSeconds(1);
        timer--;
        timerText.text = timer.ToString();
        if(timer > 0)
            StartCoroutine(TimerChange());
        else if(timer <= 0)
        {
            //oyunu bitir
            StartCoroutine(TimerChange());
            timerText.color = Color.red;
        }
    }
}
