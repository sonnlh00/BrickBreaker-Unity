using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMovementAnimator : MonoBehaviour
{
    private bool animationEnded;
    int test;
    public void Start()
    {
    }
    public void SetTrigger(string trigger)
    {
        GetComponent<Animator>().SetTrigger(trigger);
    }
}
