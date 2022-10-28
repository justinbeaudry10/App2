using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject Spawnpoint;
    public GameObject fireballPrefab;
    public GameObject pauseMenu;
    public LevelTimer timer;

    Rigidbody2D rb;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public float attackRange = 1.0f;
    public float attackTime = 1.5f;
    public float speed = 3.0f;
    public float jumpSpeed = 3.0f;
    public float distanceToCheck = 0.1f;

    float attackTimer = 0;
    bool attacking = false;
    bool isGrounded;
    float horizontal;
    bool isJumping = false;
    bool respawning = false;
    bool menuUp = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        Vector2 move = new Vector2(horizontal, 0);

        if(!Mathf.Approximately(move.x, 0.0f))
        {
            
            lookDirection.Set(move.x, 0);
            lookDirection.Normalize();
        } 
        

        animator.SetFloat("Look X", lookDirection.x);

        isGrounded = CheckGrounded();

        if (isJumping && rb.velocity.y < 0 && isGrounded)
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !respawning)
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            isJumping = true;
            animator.SetTrigger("Jump");
        }

        if (rb.position.y < -4 && !respawning)
        {
            isJumping = false;
            Die();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Strike();
            attacking = true;
        }

        if (attacking)
        {
            attackTimer += Time.deltaTime;

            if(attackTimer >= attackTime)
            {
                attacking = false;
                attackTimer = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(menuUp)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }

            timer.toggleTimer();
            menuUp = !menuUp;
        }

    }

    void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if(Mathf.Abs(horizontal) == 1)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanceToCheck);
        return hit;
    }

    void Launch()
    {
        animator.SetTrigger("Launch");

        GameObject fireballObject = Instantiate(fireballPrefab, rb.position + lookDirection * 0.5f + Vector2.down * 0.5f, Quaternion.identity);

        Fireball fireball = fireballObject.GetComponent<Fireball>();
        fireball.Launch(lookDirection, 300);
    }

    void Strike()
    {
        animator.SetTrigger("Strike");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDirection, attackRange);
        if (hit)
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
            
        }
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        respawning = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
