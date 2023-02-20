using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    //public float attackRange;
    public float attackRangeX;
    public float attackRangeY;
    public LayerMask whatIsEnemies;
    public int damage;

    public Animator weaponAnimator;
    public AudioSource src;
    public AudioClip weaponSFX;
    //public Animator camAnim;

    private void Start()
    {
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwAttack <= 0)
        {
            //you can attack
            if (Input.GetButtonDown("Fire1") && FindObjectOfType<PlayerMovement>().dagger)
            {
                weaponAnimator.SetTrigger("slash");
                //play Swoosh SFX
                src.PlayOneShot(weaponSFX);
                //camAnim.SetTrigger("shake");
                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2 (attackRangeX, attackRangeY), 0, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    //Debug.Log("hit enemy");
                    enemiesToDamage[i].GetComponent<Patrol>().TakeDamage(damage, this.transform.position);
                }
                timeBtwAttack = startTimeBtwAttack;
            } 
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));
    }
}
