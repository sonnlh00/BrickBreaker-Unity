using UnityEngine;
using System.Collections;
//using UnityEditor;
//using System.Collections.Generic;
//using UnityEngine.UI;

namespace LetterboxCamera {

    /* FollowCam.cs
     *
     * Gently follows a given Transform
     * Repositions the Camera left or right depending on the last direction travelled
     *
     * Copyright Hexdragonal Games 2015
     * Written by Tom Elliott */

    public class FollowCam : MonoBehaviour {
        #region Variables
        // Public Variables
        public Transform objectToFollow;
        public float localDistanceAheadOfObject = 6f;
        public float followWeight = 0.1f;

        // Private Variables
        private Vector3 targetLocalPosition;
        private Vector3 originLocalPosition;

        #endregion

        #region Unity Default Functions

        /// <summary>
        /// Validate any insecure variables
        /// </summary>
        private void Awake() {
            originLocalPosition = this.transform.localPosition;

            if (objectToFollow == null) {
                Debug.Log("Warning: There is no Object to follow on the Following Camera!");
            } else {
                targetLocalPosition = this.transform.localPosition;
            }
        }

        /// <summary>
        /// Lerp to a distance ahead of the follow target
        /// </summary>
        private void Update() {
            float horizontalInput = Input.GetAxis("Horizontal");

            // Look right
            if (horizontalInput > 0.05f) {
                targetLocalPosition = new Vector3(localDistanceAheadOfObject, originLocalPosition.y, originLocalPosition.z);
            } else if (horizontalInput < -0.05f) { // Look left
                targetLocalPosition = new Vector3(-localDistanceAheadOfObject, originLocalPosition.y, originLocalPosition.z);
            }

            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, targetLocalPosition, followWeight);
            this.transform.parent.position = Vector3.Lerp(this.transform.parent.position, objectToFollow.position, followWeight);
        }

        #endregion
    }
}