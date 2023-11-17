using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject[] door;
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private SpriteRenderer image2;

    public bool isActive = false;
    public bool isEnter = false;

    public void SetActiveLever(bool _isActive)
    {
        if (isEnter)
        {
            isActive = !isActive;
        }
    }

    private void Update()
    {
        if (isEnter)
        {
            if (isActive)
            {
                foreach (GameObject go in door)
                {
                    ActiveLever(go);
                }
            }
            else
            {
                foreach (GameObject go in door)
                {
                    DisableLever(go);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            image.enabled = true;
            isEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            image.enabled = false;
            isEnter = false;
        }
    }

    void ActiveLever(GameObject go)
    {
        go.GetComponent<BoxCollider2D>().enabled = true;
        image2.flipX = true;
        isActive = true;
    }

    void DisableLever(GameObject go)
    {

        go.GetComponent<BoxCollider2D>().enabled = false;
        image2.flipX = false;
        isActive = false;
    }
}
