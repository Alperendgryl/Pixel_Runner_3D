using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioManager audioManager;
    public GameManager gameManager;

    public Rigidbody rb;

    public float jumpForce;

    public Transform modelHolder;
    public LayerMask layerMask;
    public bool onGround;

    public Animator animator;

    [Header("ContinueTheGame")]
    private Vector3 startPos;
    private Quaternion startRotation;
    public float invincibleTime;
    private float invincibleTimer;

    private void Start()
    {
        startPos = transform.position; //keep the inital pos of player
        startRotation = transform.rotation; //keep the initial rotation of player
    }

    void Update()
    {
        Jump();

        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (gameManager.canMove)
        {
            onGround = Physics.OverlapSphere(modelHolder.position, 0.2f, layerMask).Length > 0f;

            if (onGround)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    rb.velocity = new Vector3(0f, jumpForce, 0f);
                    audioManager.jump.Play();
                }
            }
        }

        animator.SetBool("walking", gameManager.canMove);
        animator.SetBool("onGround", onGround);
    }

    public void ResetPos()
    {
        gameObject.GetComponent<Animator>().enabled = true; //re-enable the animations
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Cannot diseppear after death

        transform.position = startPos;
        transform.rotation = startRotation;

        invincibleTimer = invincibleTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacles")
        {
            if (invincibleTimer <= 0)
            {
                gameManager.Hit();
                rb.constraints = RigidbodyConstraints.None;
                rb.velocity = new Vector3(Random.Range(GameManager._worldSpeed / 2f, -GameManager._worldSpeed / 2f), 2.5f, -GameManager._worldSpeed / 2f);
                gameObject.GetComponent<Animator>().enabled = false;
                audioManager.hit.Play();
                ShakeController._isShake = true;
            }
        }

        if (other.tag == "Collactable")
        {
            other.gameObject.SetActive(false);
            gameManager.CoinCollected();

            audioManager.coin.Stop();
            audioManager.coin.Play();
        }
    }
}
