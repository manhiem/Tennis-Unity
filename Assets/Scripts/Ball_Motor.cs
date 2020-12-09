using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball_Motor : MonoBehaviour
{
    Vector3 initialPos;

    public string hitter;

    int playerScore;
    int botScore;

    [SerializeField] Text playerScoreText;
    [SerializeField] Text botScoreText;

    public bool playing = true;
    private void Start()
    {
        initialPos = transform.position;
        playerScore = 0;
        botScore = 0;
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision other) 
    {
        if(other.transform.CompareTag("Wall"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //transform.position = initialPos;

            GameObject.Find("Player").GetComponent<PlayerMotor>().Reset();

            if(playing)
            {
            if(hitter == "Player")
            {
               botScore++;
            }
            else if(hitter == "AI_Bot")
            {
                playerScore++;
            }
            playing = false;
            UpdateScore();
            }
            
        }
        else if(other.transform.CompareTag("Out"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        
            GameObject.Find("Player").GetComponent<PlayerMotor>().Reset();
            if(playing)
            {
            if(hitter == "Player")
            {
               botScore++;
            }
            else if(hitter == "AI_Bot")
            {
                playerScore++;
            }
            playing = false;
            UpdateScore();
            }
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Out") && playing)
        {
            if(hitter == "Player")
            {
                botScore++;
            }
            else if(hitter == "AI_Bot")
            {
                playerScore++;
            }

            playing = false;
            UpdateScore();
        }
    }

    void UpdateScore()
    {
        playerScoreText.text = "Player:" + playerScore;
        botScoreText.text = "Bot:" + botScore;
    }
}
