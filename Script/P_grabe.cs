using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_grabe : MonoBehaviour
{
    public enum boxPose 
    {
        Top,
        Left,
        Right,
        None,
    };
    public boxPose pose;
    // Start is called before the first frame update
    [SerializeField] private Transform grabePointRightTransform;
    [SerializeField] private Transform grabePointleftTransform;
    [SerializeField] private Transform grabePointTopTransform;
    [SerializeField] private Transform rayPointTransform;
    [SerializeField] private Transform rayPointRightTransform;
    [SerializeField] private Transform rayPointLeftTransform;
    [SerializeField] private Transform rayPointTopTransform;
    [SerializeField] private Animator animator;

    public GameObject grabbedObject;
    private int layerIndex;
    private int layerPlayer;
    private bool Cangrab;
    private bool callfunc;
    private bool cooldown = true;
    private Rigidbody2D rb;
    private RaycastHit2D hitRightInfo;

    public bool isGrabbing;
    public float rayDist;

    void Start()
    {
        layerIndex = LayerMask.NameToLayer("object");
        layerPlayer = LayerMask.NameToLayer("Player");
        grabbedObject = null;
        isGrabbing = false;
        pose = boxPose.None;
        rb = GetComponent<Rigidbody2D>();
    }
    public void setBool(bool inputHelp)
    {
        callfunc = inputHelp;
        isGrabbing = !isGrabbing;
    }

    // Update is called once per frame
    void Update()
    {
        FlipTrigger();

        Debug.DrawRay(rayPointLeftTransform.position, new Vector2(-1, 0) * rayDist);
        if (hitRightInfo.collider != null && hitRightInfo.collider.gameObject.layer == layerIndex | hitRightInfo.collider.gameObject.layer == layerPlayer)
            Cangrab = true;
        else if (grabbedObject != null)
            Cangrab = true;
        else
            Cangrab = false;

        if (isGrabbing && grabbedObject == null && Cangrab && cooldown)
        {
           
           grabbedObject = hitRightInfo.collider.gameObject;
            grabbedObject.transform.position = grabePointRightTransform.position;
            if (grabbedObject.layer == 6)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/CHARA/GRABER/grab_character");
                if (grabbedObject.tag == "Player-Puncher")
                    FMODUnity.RuntimeManager.PlayOneShot("event:/CHARA/PUNCHER/puncher_grab");
                else if (grabbedObject.tag == "Player-Jumper")
                    FMODUnity.RuntimeManager.PlayOneShot("event:/CHARA/JUMPER/jumper_grab");
            }
            else
                FMODUnity.RuntimeManager.PlayOneShot("event:/CHARA/GRABER/grab_box");
            StartCoroutine(Cooldown());

        }
        else if (!isGrabbing && grabbedObject != null)
        {
            if (pose == boxPose.Top && grabbedObject.layer == layerIndex)
                grabbedObject.transform.position = grabePointRightTransform.position;
            pose = boxPose.None;
            animator.SetFloat("Grab_state", 0);
            grabbedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if(grabbedObject.layer == layerIndex)
                grabbedObject.GetComponent<Rigidbody2D>().mass = 1000;
            else
                grabbedObject.GetComponent<Rigidbody2D>().mass = 1;
            animator.SetFloat("Grab_state", 0);
            grabbedObject.transform.SetParent(null);
            grabbedObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            grabbedObject = null;

        }
        if (grabbedObject == null)
            isGrabbing = false;
        if(isGrabbing)
            GrabePose();
    }

    private void GrabePose()
    {
        grabbedObject.GetComponent<Rigidbody2D>().velocity = rb.velocity;
        grabbedObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        grabbedObject.GetComponent<Rigidbody2D>().mass = 0;

        switch (pose)
        {
            case boxPose.Left:
                RaycastHit2D LeftInfo = Physics2D.Raycast(rayPointLeftTransform.position, new Vector2(-1, 0), 1.5f);
                if (LeftInfo.collider == null)
                {
                    grabbedObject.transform.position = grabePointleftTransform.position;
                }
                break;
            case boxPose.Right:
                RaycastHit2D RightInfo = Physics2D.Raycast(rayPointRightTransform.position, transform.right, 1.5f);
                if (RightInfo.collider == null)
                    grabbedObject.transform.position = grabePointRightTransform.position;
                break;
            case boxPose.Top:
                RaycastHit2D TopInfo = Physics2D.Raycast(rayPointTopTransform.position, new Vector2(0, 1), 1.5f);
                if (TopInfo.collider == null)
                    grabbedObject.transform.position = grabePointTopTransform.position;
                break;

            default: break;
        }
    }

    public void grabDir(Vector2 dir) 
    {
        if (isGrabbing)
        {
            if (dir.x >= 0.4 && dir.y <= 0.3)
            {
                pose = boxPose.Right;
                animator.SetFloat("Grab_state", 1);
            }
            else if (dir.x <= 0.3 && dir.y >= 0.4)
            {
                pose = boxPose.Top;
                animator.SetFloat("Grab_state", 2);
            }
            else if (dir.x <= -0.2 && dir.y <= 0.3)
            {
                pose = boxPose.Left;
                animator.SetFloat("Grab_state", 3);
            }
        }
    }

    void FlipTrigger()
    {
        if (gameObject.GetComponentInChildren<Animator>().gameObject.transform.localScale == new Vector3(1, 1, 1))
            hitRightInfo = Physics2D.Raycast(rayPointTransform.position, transform.right, rayDist);
        else if (gameObject.GetComponentInChildren<Animator>().gameObject.transform.localScale == new Vector3(-1, 1, 1))
        {
            hitRightInfo = Physics2D.Raycast(rayPointLeftTransform.position, new Vector2(-1, 0), rayDist);
            Debug.Log("flip");
        }
    }

    IEnumerator Cooldown()
    {
        cooldown = false;
        yield return new WaitForSeconds(1);
        cooldown = true;
    }
}
