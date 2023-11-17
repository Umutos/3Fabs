using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Controller : MonoBehaviour
{
    [HideInInspector] public float punchVelocity = 0;

    [SerializeField] public  float speed = 3f;
    [SerializeField] private float speedAirControl = 3f;
    [SerializeField] private int   playerIndex = 0;
    [SerializeField] private SpriteRenderer spriteRenderer; 
    [SerializeField] private Animator animator;
    [SerializeField] private Spawnpoint sp;

    public ScriptCam scriptCam;

    [SerializeField] private FMODUnity.StudioEventEmitter sounds;
    private P_Grounded checkGround;
    private Rigidbody2D rb;
    private Vector2 inputVector = Vector2.zero;
    private Vector2 VELO = Vector2.zero;
    public bool IsDead = false;
    private bool InAir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkGround = GetComponent<P_Grounded>();
    }

    public int GetPlayerIndex()
    {
        return playerIndex - 1;
    }

    public void SetInputVector(Vector2 _inputVector)
    {
        inputVector = _inputVector;
    }

    private void FixedUpdate()
    {
        if (scriptCam.start)
        {
            PlayerMovement();
            Flip(rb.velocity.x);
            if (!checkGround.isGrounded) sounds.Stop();
            if (IsDead)
            {
                if (!sp.hasSwitchedRoom)
                {
                    IsDead = false;
                    transform.position = sp.spawnPoints[scriptCam.indexRoom - 1].spawn2.transform.position;

                }
            }
        }
    }

    void PlayerMovement()
    {
        if (rb.velocity.y < 0.5f && !checkGround.isGrounded)
        {
            rb.gravityScale = rb.gravityScale + 0.25f;
        }
        else if (checkGround.isGrounded)
        {
            rb.gravityScale = 1;
        }

        if (checkGround.isGrounded)
        {
            rb.velocity = new Vector2(inputVector.x * speed, rb.velocity.y);
        }
        else if (!checkGround.isGrounded)
        {
            rb.velocity = new Vector2(inputVector.x * speedAirControl, rb.velocity.y);
        }
        if (rb.velocity.y < -0.3 && !checkGround.isGrounded)
        {
            animator.SetTrigger("Fall");
            InAir = true;
        }
        if(checkGround.isGrounded && InAir)
        {
            animator.SetTrigger("Land");
            InAir = false;
        }

        if (rb.velocity.x != 0)
        {
            animator.SetFloat("Speed", 1);
            if (!sounds.IsPlaying()) sounds.Play();
        }
        else
        {
            animator.SetFloat("Speed", 0);
            if(sounds.IsPlaying())
                sounds.Stop();
        }

        rb.velocity += new Vector2(punchVelocity, 0);
        punchVelocity *= 0.8f;
    }

    void Flip(float _velocity)
    {
        if(_velocity > 0.1f)
        {
            if (tag == "Player-Grabber")
            {
                if(!GetComponent<P_grabe>().isGrabbing)
                    gameObject.GetComponentInChildren<Animator>().gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            else
                spriteRenderer.flipX = false;
        }
        else if(_velocity < -0.1f)
        {
            if (tag == "Player-Grabber")
            {
                if (GetComponent<P_grabe>().isGrabbing)
                    gameObject.GetComponentInChildren<Animator>().gameObject.transform.localScale = new Vector3(1, 1, 1);
                else
                    gameObject.GetComponentInChildren<Animator>().gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
                spriteRenderer.flipX = true;
        }
    }

    IEnumerator deathRoutine()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(.2f);
    }
}

