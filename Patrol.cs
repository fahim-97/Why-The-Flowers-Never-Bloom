using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health;
    public float damage;
    public float speed;
    private bool movingRight = true;
    public Transform groundDetection;
    public float distance;
    private float dazedTime;
    public float startDazedTime;
    public AudioSource src;
    public AudioClip hmmSFX;
    public GameObject bloodEffect;

    public float knockbackForce;
    private float timeBtwDmg;
    public float startTimeBtwDmg;
    public Rigidbody2D rb;

    private void Start()
    {
        src = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dazedTime <= 0)
        {
            speed = 5;
        }
        else
        {
            speed = 0;
            dazedTime -= Time.deltaTime;
        }

        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        
        if (groundInfo.collider == false)
        {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (timeBtwDmg <= 0)
            {
                //Debug.Log("player found"); //works 
                collision.gameObject.GetComponent<PlayerStatScript>().TakeDamage(damage);
                //src.PlayOneShot(hmmSFX);
                timeBtwDmg = startTimeBtwDmg;
            }
            else
            {
                timeBtwDmg -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStatScript>().TakeDamage(damage);
            src.PlayOneShot(hmmSFX);
        }
    }

    public void TakeDamage(int damage, Vector3 other)
    {
        dazedTime = startDazedTime;

        if (health - damage >= 0)
        {
            health -= damage;
            Vector2 difference = (transform.position - other).normalized;
            Vector2 force = difference * knockbackForce;
            rb.AddForce(force, ForceMode2D.Impulse);
            //Instantiate(bloodEffect, transform.position, Quaternion.identity);
            //Debug.Log("damage taken");
        }
        else if (health - damage <= 0)
        {
            Die();
        }
       
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
