using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject playerObject;
    Animator animator;
    PlayerController player;

    Vector2 origin;

    public float speed = 2.0f;
    public float range = 2.0f;
    public float attackRange = 1.5f;
    public float attackCooldown = 3.0f;

    float attackTimer;

    int direction = 1;

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
        if (Mathf.Abs(playerObject.transform.position.x - rb.position.x) < attackRange && Mathf.Abs(playerObject.transform.position.y - rb.position.y) < 1.0f)
        {
            if(direction < 0 && playerObject.transform.position.x < rb.position.x)
            {
                if(attackTimer <= 0) {
                    StartCoroutine(Attack());
                    attackTimer = attackCooldown;
                }
            }
            else if(direction > 0 && playerObject.transform.position.x > rb.position.x)
            {
                if (attackTimer <= 0)
                {
                    StartCoroutine(Attack());
                    attackTimer = attackCooldown;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;

        if(Mathf.Abs(origin.x - position.x) >= range)
        {
            direction = -direction;

            animator.SetFloat("Move X", direction);
        }

        position.x = position.x + Time.deltaTime * direction * speed;

        rb.MovePosition(position);

        attackTimer-= Time.deltaTime;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.Die();
        }
    }

    IEnumerator Attack()
    {
        speed = 0;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.75f);

        if(Mathf.Abs(playerObject.transform.position.x - rb.position.x) < attackRange && Mathf.Abs(playerObject.transform.position.y - rb.position.y) < 1.0f)
        {
            if (direction < 0 && playerObject.transform.position.x < rb.position.x)
            {
                player.Die();
            }
            else if (direction > 0 && playerObject.transform.position.x > rb.position.x)
            {
                player.Die();
            }
        }

        yield return new WaitForSeconds(1.25f);

        speed = 2.0f;
    }
}
