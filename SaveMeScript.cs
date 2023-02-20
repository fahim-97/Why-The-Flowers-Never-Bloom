using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMeScript : MonoBehaviour
{
    public Rigidbody2D piano;
    public AudioSource pianoBreak;

    void Start()
    {
        //dropPiano();
        FindObjectOfType<PlayerStatScript>().StartSaveMe();
    }

    public void dropPiano()
    {
        //drop the piano on him
        piano.bodyType = RigidbodyType2D.Dynamic;
        // StartCoroutine(DropPlatforms());
        pianoBreak.Play();
        Destroy(pianoBreak, 3f);
    }
}
