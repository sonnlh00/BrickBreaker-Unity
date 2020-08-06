using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int numOfBrickableBricks;

    private GameManager gameManager;
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
        gameManager = GameObject.FindObjectOfType<GameManager>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        soundNotPlayed = true;
       
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
            gameManager.OnWinning();
            
        }
    }
    //void CreateNewBrick()
}
