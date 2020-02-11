using UnityEngine;
using System.Collections;
using LetterboxCamera;

namespace LetterboxCamera {

    public class LetterboxGameDemo : MonoBehaviour {

        public ForceCameraRatio cameraManager;
        public float letterboxInRate = 2f;
        public float letterboxOutRate = 10f;
        float letterboxRate;
        Vector2 targetRatio;
        bool inRatio = false;

        public void Start() {
            targetRatio = new Vector2(5, 4);
        }

        public void Update() {
            cameraManager.ratio = Vector2.Lerp(cameraManager.ratio, targetRatio, letterboxRate * Time.deltaTime);
        }

        public void OnGUI() {
            Rect buttonRect = new Rect(20f, 10f, 200, 50f);

            if (!inRatio) {
                GUI.color = new Color(1, 0.1f, 0.1f);
                if (GUI.Button(buttonRect, "Letterbox off :(")) {
                    inRatio = true;
                    letterboxRate = letterboxInRate;
                    targetRatio = new Vector2(16, 9);
                }

            } else if (inRatio) {
                GUI.color = new Color(0.1f, 1.0f, 0.1f);
                if (GUI.Button(buttonRect, "LETTERBOX ON!")) {
                    inRatio = false;
                    letterboxRate = letterboxOutRate;
                    targetRatio = new Vector2(5, 4);
                }
            }
        }
    }
}