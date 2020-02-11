using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int numOfBrickableBricks;

    private LevelManager LevelManager;
    private AudioManager audioManager;
    private bool soundNotPlayed;
    public void DestroyBrick()
    {
        numOfBrickableBricks--;
    }
    public int RemainBricks()
    {
        return numOfBrickableBricks;
    }
    void Start()
    {
        LevelManager = GameObject.FindObjectOfType<LevelManager>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        soundNotPlayed = true;
        Cursor.visible = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (numOfBrickableBricks <= 0)
        {
            if (soundNotPlayed)
            {
                soundNotPlayed = false;
                audioManager.PlayClearingLevelAudio();
                
            }
            LevelManager.LoadNextLevel();
            Cursor.visible = true;
        }
    }
    //void CreateNewBrick()
}
