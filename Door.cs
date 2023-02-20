using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject enemy;
    public AudioSource src;
    public AudioClip hissSFX;
    public SpriteRenderer r;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Instantiate(enemy, transform);
            src.PlayOneShot(hissSFX);
            Destroy(gameObject, 2f);
        }
    }

    public void ShowDoor()
    {
        r.enabled = true;
    }
}
