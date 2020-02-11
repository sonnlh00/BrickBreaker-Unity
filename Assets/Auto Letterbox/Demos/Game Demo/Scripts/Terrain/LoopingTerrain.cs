using UnityEngine;
using System.Collections;

namespace LetterboxCamera {

    public class LoopingTerrain : MonoBehaviour {
        public GameObject upperTerrainDummy, lowerTerrainDummy;
        public Vector3 jumpPosition;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            Vector3 positionUpper, positionLower;

            positionUpper = upperTerrainDummy.transform.position;
            positionLower = lowerTerrainDummy.transform.position;

            if (positionLower.x <= positionUpper.x) {
                this.gameObject.transform.position = this.gameObject.transform.position + jumpPosition;
            }


        }
    }
}