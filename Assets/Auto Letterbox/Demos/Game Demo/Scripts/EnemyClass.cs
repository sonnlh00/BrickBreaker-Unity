using UnityEngine;
using System.Collections;

namespace LetterboxCamera {

    public class EnemyClass : MonoBehaviour {
        public int nutrition;
        public Feed diet;

        AudioSource hitAudio;
        bool beingDragged = false;

        public Rigidbody2D rigid {
            get; private set;
        }

        void Start() {
            rigid = this.GetComponent<Rigidbody2D>();
            hitAudio = this.GetComponent<AudioSource>();
        }

        void OnMouseDown() {
            beingDragged = true;
            EnableGravity();
            PlayHitAudio(1.5f, 2.5f);
        }
        void OnMouseUp() {
            if (rigid != null) {
                rigid.velocity = Vector3.zero;
                rigid.angularVelocity = 0;
            }
            beingDragged = false;

        }
        void OnCollisionEnter2D(Collision2D coll) {

            EnableGravity();
            if (rigid != null && rigid.velocity.sqrMagnitude > 1) {
                PlayHitAudio(0.5f, 1.5f);
            }
            GameObject dragonObject = coll.gameObject;
            DragonClass dragonBump = dragonObject.GetComponent<DragonClass>();

            if (dragonBump != null) {// && beingDragged == true) {
                dragonBump.ApplyScore(nutrition, diet);
                GameObject.Destroy(this.gameObject);
            }

        }

        void PlayHitAudio(float _lowPitch, float _highPitch) {
            if (hitAudio != null) {
                hitAudio.pitch = Random.Range(_lowPitch, _highPitch);
                hitAudio.Play();
            }
        }

        void EnableGravity() {
            if (rigid != null) {
                rigid.gravityScale = 1;
            }
        }

        // Update is called once per frame
        void Update() {
            if (beingDragged) {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = -Camera.main.gameObject.transform.position.z;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

                this.gameObject.transform.position = mousePosition;
            }

        }
    }
}