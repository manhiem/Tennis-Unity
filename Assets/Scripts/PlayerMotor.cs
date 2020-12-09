using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float speed = 3f;
    public Transform aimTarget; 
    bool Hitting;
    Animator animator;
    Vector3 aimTargetInitialPosition;

    ShotManager shotManager;
    Shot currentShot;

    [SerializeField] Transform serveRight;
    [SerializeField] Transform serveLeft;

    bool servedRight = true;

    public Transform ball;
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        aimTargetInitialPosition = aimTarget.position;
        shotManager = GetComponent<ShotManager>();
        currentShot = shotManager.topspin;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.F))
        {
            Hitting = true;
            currentShot = shotManager.topspin;
        } 
        else if(Input.GetKeyUp(KeyCode.F))
        {
            Hitting = false;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Hitting = true;
            currentShot = shotManager.flatspin;
        }
        else if(Input.GetKeyUp(KeyCode.E))
        {
            Hitting = false;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            Hitting = true;
            currentShot = shotManager.flatserve;
            GetComponent<BoxCollider>().enabled = false;
            ball.transform.position = transform.position + new Vector3(0.2f, 1, 0);
            animator.Play("Serveprepare");
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            Hitting = true;
            currentShot = shotManager.kickserve;
            GetComponent<BoxCollider>().enabled = false;
            ball.transform.position = transform.position + new Vector3(0.2f, 1, 0);
            animator.Play("ServePrepare");
        }

        if(Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.T))
        {
            Hitting = false;
            GetComponent<BoxCollider>().enabled = true;
            Vector3 dir = aimTarget.position - transform.position;
            ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitforce + new Vector3(0, currentShot.upforce, 0); 
            animator.Play("Serve");

            ball.GetComponent<Ball_Motor>().hitter = "Player";
            ball.GetComponent<Ball_Motor>().playing = true;
        }


        if(Hitting)
        {
            aimTarget.Translate(new Vector3(h, 0, 0) * speed * 2 *Time.deltaTime);
        }

        if((h!=0 || v!=0) && !Hitting)
        {
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            Vector3 dir = aimTarget.position - transform.position;
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

            ball.GetComponent<Ball_Motor>().hitter = "Player";
            aimTarget.position = aimTargetInitialPosition;
        }
    }

    public void Reset()
    {
        if(servedRight)
        {
        transform.position = serveLeft.position;
        }
        else
        {
            transform.position = serveRight.position;
        }

        servedRight = !servedRight;

    }
}
