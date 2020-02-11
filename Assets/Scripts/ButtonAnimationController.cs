using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private AudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
    }
    public void OnCursorEnter()
    {
        animator.SetTrigger("Cursor On Button");
        audioManager.PlayMouseHoverUIAudio();
    }
    public void OnCursorExit()
    {
        animator.SetTrigger("Cursor Off Button");
    }
    public void OnMouseClick()
    {
        audioManager.PlayMouseClickUIAudio();
        animator.SetTrigger("Clicked");
    }
}
