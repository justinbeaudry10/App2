using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeController : MonoBehaviour
{
    GameObject playerObject;
    PlayerController player;
    Animator animator;
    Rigidbody2D rb;

    Vector2 origin;

    public float speed = 2.0f;
    public float attackSpeed = 3.0f;
    public float range = 4.0f;
    public float attackrange = 5.0f;
    public float attackCooldown = 5.0f;

    bool attacking = false;
    float attackTimer;

    float direction = 1;
    float vertDirection = -1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        origin = rb.position;

        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<PlayerController>();

        attackTimer = attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(rb.position.x - playerObject.transform.position.x) < 0.5f && !attacking && attackTimer <= 0)
        {
            attacking = true;
            animator.SetBool("Attacking", true);
        }

        attackTimer -= Time.deltaTime;

        if(attacking)
        {
            attackTimer = attackCooldown;
        }
        
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;

        if (Mathf.Abs(origin.x - position.x) >= range)
        {
            direction = -direction;
            animator.SetFloat("Look X", direction);

           //animator.SetFloat("Move X", direction);
        }

        if(attacking)
        {
            position.y = position.y + Time.deltaTime * attackSpeed * vertDirection;
            if(origin.y - position.y >= attackrange)
            {
                vertDirection = 1;
            }
            else if(origin.y - position.y <= 0)
            {
                vertDirection = -1;
                attacking = false;
                animator.SetBool("Attacking", false);
            }
        }
        else
        {
            position.x = position.x + Time.deltaTime * direction * speed;
        }
        

        rb.MovePosition(position);

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.Die();
        }
    }

}
