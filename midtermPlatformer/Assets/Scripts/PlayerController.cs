using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int fruit = 0;
    public float collect = 0f;
    public float gravityMultiplier;
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 10f;
    private bool isFacingRight = true;

    private bool canDash = true;
    public bool isDashing;
    private float dashingPower = 30f;
    private float dashingTime = .2f;
    private float dashingCooldown = 1f;
    

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
  

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown("space") && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
        }

        Flip();

        /*         
         if ("Level 2")
        {
            //Wall Jump

        }*/
        Scene scene = SceneManager.GetActiveScene();
        string sceneName = scene.name;

        if (sceneName == "Level 1")
        {
            //Dash
           if(Input.GetKeyDown(KeyCode.W) && canDash)
            {
                StartCoroutine(Dash());        
            }
        }

        gravityMultiplier = collect;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    /*void OnCollisionEnter2D(Collider2D collider)
        {

            if (collider.gameObject.tag == "Enemy")
        {
            if(collect > 3)
             {
            collect -= 3;
            Debug.Log(collect);
             }
            else if(collect == 3)
            {
                Debug.Log("You Died");
                //restartlayout
            }

        }

            if (C && collider.gameObject.tag == "Wall")
        {
            //Break Wall
            Input.GetKeyDown("Q")
                Destroy(collider.gameObject)
        }
         }*/

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.gameObject.name == "Choice"))// && (Input.GetKeyDown(KeyCode.Q)))
        {
            SceneManager.LoadScene("Level 1");
        }

        if ((collider.gameObject.name == "Choice (1)"))// && (Input.GetKeyDown(KeyCode.Q)))
        {
            SceneManager.LoadScene("Level 2");
        }

        if ((collider.gameObject.name == "Choice (2)")) // && (Input.GetKeyDown(KeyCode.Q)))
        {
            SceneManager.LoadScene("Level 3");
        }

        if (collider.gameObject.tag == "NextLevel")
        {
            SceneManager.LoadScene("the creature");
        }

        if ((collider.gameObject.name == "Collectable"))
        {
            collect += 1f;
            Debug.Log(collect);
            Destroy(collider.gameObject);
        }
        if ((collider.gameObject.name == "Fruit"))
        {
            fruit += 1;
            Destroy(collider.gameObject);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}