using UnityEngine;
using System.Collections;

namespace LetterboxCamera {

    public enum Feed {
        Meat, Fruit
    }

    public class DragonClass : MonoBehaviour {

        AudioSource munchAudio;
        int meat = 0;
        int fruit = 0;
        int nutritionalValue;

        public int GetMeat() {

            return meat;

        }

        public int GetFruit() {

            return fruit;

        }

        public void ApplyScore(int IncomingScore, Feed Diet) {
            nutritionalValue += IncomingScore;
            if (Diet == Feed.Meat)
                meat += 1;
            if (Diet == Feed.Fruit)
                fruit += 1;

            if (munchAudio != null) {
                munchAudio.pitch = Random.Range(2.5f, 3.5f);
                munchAudio.Play();
            }

        }
        void Start() {
            munchAudio = this.GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}