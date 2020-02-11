using UnityEngine;
using System.Collections;
using LetterboxCamera;

namespace LetterboxCamera
{
    /* MenuSpin.cs
     * 
     * Gently spins the gameoject it's attached to
     * 
     * Copyright 20 Credit Games 2014
     * Written by Milo Keeble, Chloe Goodchild */

    public enum rotAxis
    {
        xAxis,
        yAxis,
        zAxis
    }

    public class MenuSpin : MonoBehaviour
    {



        public float spinSpeed;
        public enum direc { clockwise, counterclockwise, random };
        public direc spinDirection;
        public rotAxis axisRotation = rotAxis.yAxis;

        private Vector3 rotationVector;

        // Use this for initialization
        void Start () {

            switch (axisRotation) {
                case rotAxis.xAxis:
                    rotationVector = Vector3.right;
                    break;
                case rotAxis.yAxis:
                    rotationVector = Vector3.up;
                    break;
                case rotAxis.zAxis:
                    rotationVector = Vector3.back;
                    break;
                default:
                    break;
            }

            if (spinDirection == direc.random) {
                int spinner = Random.Range(0, 99);

                //Debug.Log(spinner);

                if (spinner <= 49) {
                    spinDirection = direc.clockwise;
                } else
                    spinDirection = direc.counterclockwise;
            }

        }

        // Update is called once per frame
        void Update () {
            if (spinDirection == direc.clockwise)
                gameObject.transform.rotation *= Quaternion.AngleAxis(spinSpeed * Time.deltaTime, rotationVector);
            else
                gameObject.transform.rotation *= Quaternion.AngleAxis(spinSpeed * Time.deltaTime, -rotationVector);
        }
    }
}