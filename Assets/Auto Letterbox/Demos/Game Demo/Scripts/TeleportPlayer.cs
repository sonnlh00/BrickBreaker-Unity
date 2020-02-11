using UnityEngine;
using System.Collections;

namespace LetterboxCamera {

    public class TeleportPlayer : MonoBehaviour {

        public float newX = -12.98f;
        public Parralax[] parralaxObjects;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void OnTriggerEnter2D(Collider2D coll) {
            DragonClass dragonBump = coll.gameObject.GetComponent<DragonClass>();

            if (dragonBump != null) {

                float deltaX = dragonBump.transform.position.x - newX;

                dragonBump.transform.position = new Vector3(dragonBump.transform.position.x - deltaX, dragonBump.transform.position.y, dragonBump.transform.position.z);

                for (int i = 0; i < parralaxObjects.Length; i++) {
                    parralaxObjects[i].IgnoreNextFrame();
                    parralaxObjects[i].transform.position = new Vector3(parralaxObjects[i].transform.position.x - deltaX, parralaxObjects[i].transform.position.y, parralaxObjects[i].transform.position.z);
                }
            }
        }
    }
}