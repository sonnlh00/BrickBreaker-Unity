using UnityEngine;
using System.Collections;
//using UnityEditor;
//using System.Collections.Generic;
//using UnityEngine.UI;

namespace LetterboxCamera {

    /* PlayerMovement.cs
     *
     * Description
     *
     * Copyright Hexdragonal Games 2015
     * Written by Tom Elliott */

    public class PlayerMovement : MonoBehaviour {
        #region Variables
        // Public Variables
        public float runSpeed = 7.5f;
        public float jumpSpeed = 5f;
        public Transform feetMarker;

        // Private Variables
        private Rigidbody2D rigid;
        private bool grounded = false;

        #endregion

        #region Unity Default Functions

        /// <summary>
        /// Validate any insecure variables
        /// </summary>
        private void Awake() {
            rigid = this.GetComponent<Rigidbody2D>();
            if (rigid == null) {
                Debug.Log("Warning: There is no Rigidbody2D on the Player!");
            }
            if (feetMarker == null) {
                Debug.Log("Warning: Feet have not been set on the Player so we cannot jump!");
            }
        }

        /// <summary>
        /// Check for movement input
        /// Apply movement input to the Players velocity
        /// </summary>
        private void FixedUpdate() {
            Vector3 newVelocity = rigid.velocity;
            float horizontalInput = Input.GetAxis("Horizontal");

            // If grounded, the player can attempt to jump
            if (grounded && Input.GetButton("Jump")) {
                newVelocity.y = jumpSpeed;
            }

            // Apply Horizontal movement (running)
            newVelocity.x = runSpeed * horizontalInput;

            // Set the newly calculated velocity
            rigid.velocity = newVelocity;

            // We set grounded as false here because FixedUpdate() always runs before Collision checks
            grounded = false;
        }

        /// <summary>
        /// On collision, check if the Player's feet are touching the ground
        /// If the Player is grounded, we can jump
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionStay2D(Collision2D collision) {
            for (int i = 0; i < collision.contacts.Length; i++) {
                // If Collision point was low enough to hit the player's feet, we're grounded
                if (collision.contacts[i].point.y < feetMarker.position.y) {
                    grounded = true;
                }
            }
        }

        /// <summary>
        /// Draws the Feet Marker for ease of use
        /// </summary>
        public void OnDrawGizmos() {
            if (feetMarker != null) {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(feetMarker.position, 0.1f);
            }
        }

        #endregion
    }
}