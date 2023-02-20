using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
//    public Transform shieldThrowPoint;
//    public float speed = 20f;
//    public Rigidbody2D rb;

    public GameObject shieldObj;
    public bool shieldActivated = false;
    public bool thrown = false;
    public Animator animator;

    public void activateShield()
    {
        shieldActivated = true;
        shieldObj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldActivated && Input.GetButtonDown("Fire1"))
        {
            ThrowShield();
        }
    }

    void ThrowShield()
    {
        animator.SetTrigger("throw");
        Destroy(shieldObj, 3f);
    }
}
