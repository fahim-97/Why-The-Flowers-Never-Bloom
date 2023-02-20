using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController controller;
    float horizontalMove = 0f;
    public float runSpeed = 40f;
    bool jump = false;
    bool crouch = false;

    private float timeBtwAttack;
    public float startTimeBtwAttack;

    //Animation
    public Animator animator;
    public Animator weaponAnimator;

    public bool dagger = false;

    // Update is called once per frame
    void Update()
    {
        //left = -1, right = 1
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        //move
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump); //no crouch, no jump
        jump = false;

        if (horizontalMove != 0)
        {
            animator.SetTrigger("walk_right");
        }
        else
        {
            animator.SetTrigger("idle");
        }
    }

    public void activateDagger()
    {
        dagger = true;
    }
}
