using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot_Motor : MonoBehaviour
{
    float speed = 50;
    Animator animator;
    public Transform ball;
    public Transform aimTarget;
    public Transform[] targets;

    Vector3 targetPosition;
    float force = 13;
    Vector3 aimTargetInitialPosition;

    

    ShotManager shotManager;

    void Start() {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        aimTargetInitialPosition = aimTarget.position;
        shotManager = GetComponent<ShotManager>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        targetPosition.x = ball.position.x;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    Vector3 PickTarget()
    {
        int randomValue = Random.Range(0, targets.Length);
        return targets[randomValue].position;
    }

    Shot PickShot()
    {
        int randomValue = Random.Range(0, 2);
        if(randomValue == 0)
        {
            return shotManager.topspin;
        }
        else
        {
            return shotManager.flatspin;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            Shot currentShot = PickShot();
            Vector3 dir = PickTarget() - transform.position;
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitforce + new Vector3(0, currentShot.upforce, 0);
             
            Vector3 BallDir = ball.position - transform.position;
            if(BallDir.x >= 0)
            {
                animator.Play("Forehand"); 
            }
            else
            {
                animator.Play("Backhand"); 
            }

            ball.GetComponent<Ball_Motor>().hitter = "AI_Bot";
            aimTarget.position = aimTargetInitialPosition;
        }
    }
}
