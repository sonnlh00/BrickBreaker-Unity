using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    private SceneMovementAnimator Animator;
    public void Awake()
    {
        // Show mouse cursor on UI screen and hide it on game screen
        Cursor.visible = true;
    }
    public void Start()
    {
        Animator = GameObject.FindObjectOfType<SceneMovementAnimator>();
    }
    // Functions to delay new scene loading, it waits for the scene transition animation to finish first.
    IEnumerator SceneMovementAnm(string scene)
    {
        Animator.SetTrigger("ExitScene");
        yield return new WaitForSecondsRealtime(0.75f);
        SceneManager.LoadScene(scene);
    }
    IEnumerator SceneMovementAnm(int buildIndex)
    {
        Animator.SetTrigger("ExitScene");
        yield return new WaitForSecondsRealtime(0.75f);
        SceneManager.LoadScene(buildIndex);
    }
    public void LoadLevel(string name)
    {
        // Save current scene name before load requested scene
        PlayerPrefs.SetString("prevScene", SceneManager.GetActiveScene().name);
        // If scene has no animator then just loads scene normally. Otherwise, wait for the animation to end
        if (Animator == null)
        {
            if (name == "Quit Game")
            {
                Application.Quit();
            }
            else
                SceneManager.LoadScene(name);
        }
        else
        {
            StartCoroutine(SceneMovementAnm(name));
        }
       
        
        
    }
    public void RetryLevel()
    {
        if (Animator == null)
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("prevScene"));
        }
        else
            StartCoroutine(SceneMovementAnm(PlayerPrefs.GetString("prevScene")));
    }
    public void LoadNextLevel()
    {
        if (Animator == null)
        {
            // Load next scene in scene build order
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
            StartCoroutine(SceneMovementAnm(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void Update()
    {
        // Return to start menu when press Escape
        if (Input.GetKeyDown(KeyCode.Escape))
            LoadLevel("Start");
    }
}
