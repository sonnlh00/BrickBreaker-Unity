using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LetterboxCamera;

namespace LetterboxCamera {

    public class FoodTracker : MonoBehaviour {

        public DragonClass dragon;
        public Text[] textPieces;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            for (int i = 0; i < textPieces.Length; i++) {
                textPieces[i].text = (dragon.GetFruit() + dragon.GetMeat()).ToString();
            }
        }
    }
}