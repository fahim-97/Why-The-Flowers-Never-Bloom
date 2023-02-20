using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void LoadNextScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevelwithName("MainMenu"));
    }

    public void LoadPianoScene()
    {
        StartCoroutine(LoadLevelwithName("SaveMe"));
        //FindObjectOfType<PlayerStatScript>().FixTransform();
    }


    public void LoadScene(string x)
    {
        StartCoroutine(LoadLevelwithName(x));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //play anim
        transition.SetTrigger("Start");

        //wait
        yield return new WaitForSeconds(transitionTime);

        //load scene
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadLevelwithName(string name)
    {
        //play anim
        transition.SetTrigger("Start");

        //wait
        yield return new WaitForSeconds(transitionTime);

        //load scene
        SceneManager.LoadScene(name);
    }

   
    public void Respawn()
    {
        StartCoroutine(LoadLevelwithName("Boulevard"));
        //FindObjectOfType<Door>().ShowDoor();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
