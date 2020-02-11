using UnityEngine;
using System.Collections;
using LetterboxCamera;

namespace LetterboxCamera {

    public class Parralax : MonoBehaviour {

        public Transform camToParralaxAgainst;
        public Vector2 movePercentRange;
        public float baseZ = 50;
        protected float maxDistanceToCam = 14f;

        bool ignoreThisFrame = false;
        float parralaxMagnitude;
        Vector3 camLastPos;

        // Use this for initialization
        void Start() {
            camLastPos = camToParralaxAgainst.transform.position;
            parralaxMagnitude = Random.Range(movePercentRange.x, movePercentRange.y);
            float newZ = baseZ - parralaxMagnitude;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, newZ);
        }

        // Update is called once per frame
        void Update() {
            if (ignoreThisFrame) {
                ignoreThisFrame = false;
                camLastPos = camToParralaxAgainst.transform.position;
                return;
            }

            Vector3 travel = camToParralaxAgainst.transform.position - camLastPos;
            travel *= parralaxMagnitude;
            this.transform.position = this.transform.position + new Vector3(travel.x, travel.y, 0);
            camLastPos = camToParralaxAgainst.transform.position;

            if (this.transform.position.x < camToParralaxAgainst.transform.position.x - maxDistanceToCam) {
                ResetParralaxObject();
            }
        }

        void ResetParralaxObject() {
            parralaxMagnitude = Random.Range(movePercentRange.x, movePercentRange.y);
            float newZ = baseZ - parralaxMagnitude;
            this.transform.position = new Vector3(camToParralaxAgainst.transform.position.x + maxDistanceToCam, this.transform.position.y, newZ);
        }

        public void IgnoreNextFrame() {
            ignoreThisFrame = true;
        }
    }
}