using UnityEngine;
using System.Collections;

namespace LetterboxCamera {

    public class PlayerScrollScript : MonoBehaviour {

        public float speed = 1;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void FixedUpdate() {
            Vector3 right = new Vector3(1, 0, 0);
            right = speed * right;
            this.gameObject.transform.position = this.gameObject.transform.position + right;
        }
    }
}