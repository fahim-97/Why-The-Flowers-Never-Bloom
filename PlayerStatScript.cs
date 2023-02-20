using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStatScript : MonoBehaviour
{
    public float health = 100f;
    public Transform[] spawns;
    public int closestSpawn;
    public float criticalHealth = 30f;
    public bool isTalking;
    public GameObject choiceBoard;
    public GameObject shieldDialog;
    public GameObject existDialog;
    public GameObject daggerDialog;
    public GameObject daggerHolder;
    public GameObject bleedingObj;
    private bool weaponChosen = false;
    private bool foundKey = false;
    public GameObject keyDialog;
    public Animator camAnim;
    public GameObject deathImg;
    public Animator argonAnimator;
    public GameObject dialogueBox;
    public Rigidbody2D[] platforms;
    public bool metChlora = false;

    public GameObject platformRespawn;
    public GameObject chloraLetter;
    public GameObject platformPrefab;

    public Sprite benchArgon;
    public SpriteRenderer gfx;
    public DialogueTrigger bench;

    [Header("Health Bar")]
    public Slider slider;
    public GameObject endMessage;
    public GameObject credits;

    //private void Awake()
    //{
    //    //SINGLETON PATTERN
    //    int managerCount = FindObjectsOfType<PlayerStatScript>().Length;
    //    if (managerCount > 1)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else if (!String.Equals(SceneManager.GetActiveScene().name, "Finale"))
    //    {
    //        DontDestroyOnLoad(gameObject);
    //    }
    //}


    void Start()
    {
        closestSpawn = 0;
        Cursor.visible = false;
    }

    public void StartSaveMe()
    {
        //health full
        health = 200;
        //dagger in hand
        existDialog.SetActive(false);

    }

    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
        else if(health <= criticalHealth)
        {
            BleedingEffect();
        }

        if (health > criticalHealth)
        {
            bleedingObj.SetActive(false);
        }
    }

    void Die()
    {
        //death animation
        //Debug.Log("Died");
        argonAnimator.SetTrigger("die");
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);
        deathImg.SetActive(true);
        yield return new WaitForSeconds(6);
        FindObjectOfType<SceneLoader>().Respawn();
    }

    IEnumerator PlatformRespawn()
    {
        argonAnimator.SetTrigger("die");
        yield return new WaitForSeconds(2);
        deathImg.SetActive(true);
        yield return new WaitForSeconds(2);
        deathImg.SetActive(false);
        FindObjectOfType<SceneLoader>().LoadPianoScene();
        yield return new WaitForSeconds(2);
        this.transform.position = platformRespawn.transform.position;

    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void CloseLetter()
    {
        chloraLetter.SetActive(false);
        Cursor.visible = false;
    }

    //public void FixTransform()
    //{
    //    this.transform.position = GameObject.FindGameObjectWithTag("spawn").transform.position;
    //    health = 100;
    //    //FindObjectOfType<SceneLoader>().Respawn();
    //}

    void BleedingEffect()
    {
        //Debug.Log("induce bleeding");
        dialogueBox.SetActive(false);
        bleedingObj.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!weaponChosen && collision.tag == "Chlora")
        {
            //Debug.Log("Met Chlora");
            metChlora = true;
            collision.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
        }

        else if (collision.tag == "oldman" && health > criticalHealth)
        {
            metChlora = false;
            collision.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
        }

        else if (collision.tag == "existential")
        {
            existDialog.SetActive(false);
            daggerDialog.SetActive(false);
            shieldDialog.SetActive(false);
        }

        else if (collision.tag == "levelDoor")
        {
            if (foundKey)
            {
                FindObjectOfType<SceneLoader>().LoadPianoScene();
            }
            else
            {
                //display message
                keyDialog.SetActive(true);
            }

        }

        else if (collision.tag == "finaleDoor")
        {
            FindObjectOfType<SceneLoader>().LoadNextScene();
        }

        else if (collision.tag == "key")
        {
            Destroy(collision.gameObject);
            //animation?
            foundKey = true;
        }

        else if (collision.tag == "table")
        {
            Cursor.visible = true;
            chloraLetter.SetActive(true);
            daggerHolder.SetActive(true);
            health = 200;
            SetHealth((int)health);
        }

        else if (collision.tag == "wallDeath")
        {
            Die();
        }

        else if (collision.tag == "pianoWall")
        {
            FindObjectOfType<SaveMeScript>().dropPiano();
        }

        else if (collision.tag == "drop")
        {
            StartCoroutine(DropPlatforms());
        }

        else if (collision.tag == "platformDrop")
        {
            //respawn at spawn plat
            StartCoroutine(PlatformRespawn());
        }

        else if (collision.tag == "pianoRespawn")
        {
            //respawn at spawn plat
            StartCoroutine(PlatformRespawn());
        }

        //finale

        else if (collision.tag == "bench")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = benchArgon;
            gfx.enabled = false;
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            collision.GetComponent<DialogueTrigger>().TriggerDialogue();
            //bench.TriggerDialogue();
            StartCoroutine(RollCredits());
        }
    }

    IEnumerator RollCredits()
    {
        yield return new WaitForSeconds(15);
        endMessage.SetActive(true);
        yield return new WaitForSeconds(20);
        credits.SetActive(true);
        Cursor.visible = true;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("levelDoor"))
        {
            keyDialog.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "piano")
        {
            health = 20;
            SetHealth((int)health);
            Rigidbody2D pianoBody = collision.gameObject.GetComponent<Rigidbody2D>();
            StartCoroutine(FixPiano(pianoBody));
        }
    }

    IEnumerator FixPiano(Rigidbody2D piano)
    {
        yield return new WaitForSeconds(6);
        piano.bodyType = RigidbodyType2D.Static;
    }

    IEnumerator DropPlatforms()
    {
        yield return new WaitForSeconds(1f);
        //drop platform 1
        platforms[0].bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(0.8f);
        //drop platform 2
        platforms[1].bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(0.6f);
        //drop platform 3
        platforms[2].bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(0.4f);
        //drop platform 4
        platforms[3].bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(0.4f);
        //drop platform 5
        platforms[4].bodyType = RigidbodyType2D.Dynamic;

        //yield return new WaitForSeconds(0.4f);
        ////drop platform 6
        //platforms[5].bodyType = RigidbodyType2D.Dynamic;
    }

    public void ActivateChoice()
    {
        Cursor.visible = true;
        choiceBoard.SetActive(true);
    }

    public void ChooseDagger()
    {
        choiceBoard.SetActive(false);
        daggerDialog.SetActive(true);
        daggerHolder.SetActive(true);

        FindObjectOfType<PlayerMovement>().activateDagger();

        weaponChosen = true;
        Cursor.visible = false;
    }

    public void ChooseShield()
    {
        shieldDialog.SetActive(true);
        choiceBoard.SetActive(false);

        FindObjectOfType<ShieldScript>().activateShield();
        weaponChosen = true;
        Cursor.visible = false;
    }

    public void TakeDamage(float damage)
    {
       if (health - damage >= 0)
       {
            health -= damage;
            //Debug.Log("damaged");
            SetHealth((int)health);
        }
    }
}
