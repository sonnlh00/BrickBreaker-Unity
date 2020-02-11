using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    // Start is called before the first frame update
    private int maxHitTimes;
    public Sprite[] spriteSheet; // Sprites for switching when hit
    private BricksManager BricksManager;    // Keep track of number of brickable bricks remain, move to
                                            // next level if all brickable bricks are destroyed
    private int hitTimes;   
    private ParticleSystem particle;    // Effect when a brick is destroyed
    private AudioManager AudioManager;
    void Start()
    {
        maxHitTimes = spriteSheet.Length + 1;
        hitTimes = 0;
        BricksManager = GameObject.FindObjectOfType<BricksManager>();
        particle = GameObject.Find("Breaking Effect").GetComponent<ParticleSystem>();
        AudioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.tag == "Breakable")
        {
            HandleBreakableBricks();
        }
        else
        {
            AudioManager.PlayUnbreakableHitAudio();
        }
    }
    private void HandleBreakableBricks()
    {
        AudioManager.PlayBreakableHitAudio();
        hitTimes++;
        if (hitTimes >= maxHitTimes)
        {
            // a variable for MainModule, use MainModule to change color of particles to the color of brick
            ParticleSystem.MainModule main = particle.main;
            particle.transform.position = transform.position;
            main.startColor = gameObject.GetComponent<SpriteRenderer>().color;
            particle.Play(); // Play effect when a brick is destroyed

            Destroy(gameObject);
            BricksManager.DestroyBrick(); // Decrease number of breakable bricks remaining by 1
        }
        else
        {
            // If brick hasn't been destroyed yet, change its spite base on number of hits it's got so far
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteSheet[hitTimes - 1];
        }
    }
}
