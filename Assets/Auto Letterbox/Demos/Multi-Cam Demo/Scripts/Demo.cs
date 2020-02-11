using UnityEngine;
using System.Collections;
using LetterboxCamera;

namespace LetterboxCamera
{
    /* Demo.cs
     *
     * A small Demo to show off the capabilities and (hopefully) flexibilities of the Letter Box Ratio Camera System
     * 
     * Copyright Hexdragonal 2015
     * Written by Tom Elliott */

    public class Demo : MonoBehaviour
    {

        #region Variables
        // Public
        public ForceCameraRatio forceRatio;

        public Camera[] fourPlayerCameras;
        public Camera[] twoPlayerCameras;
        public Camera[] singlePlayerCameras;
        public Camera[] crazyCameras;

        public int startingDefault = 0;
        public Vector2[] defaultRatios;

        // Private
        private Color guiOnColor = new Color(1f, 1f, 1f, 1f);
        private Color guiOffColor = new Color(1f, 1f, 1f, 0.2f);
        private float guiColorProgress = 1f;
        private int currentDefault = 0;

        #endregion

        #region Unity Default Functions

        /// <summary>
        /// Validate any insecure variables
        /// </summary>
        private void Awake () {
            currentDefault = startingDefault;
            EnableCameras(singlePlayerCameras, true);
            EnableCameras(twoPlayerCameras, false);
            EnableCameras(fourPlayerCameras, false);
            EnableCameras(crazyCameras, false);
        }

        #endregion

        private void EnableCameras (Camera[] _cameras, bool _enable) {
            for (int i = 0; i < _cameras.Length; i++) {
                _cameras[i].enabled = _enable;
            }
        }

        private void OnGUI () {
            // Ensure the mouse is over the Demo Menu
            if (Input.mousePosition.x < 0 || Input.mousePosition.x > 120 ||
                Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height) {
                guiColorProgress = Util.Clamp(0.2f, 1f, guiColorProgress - Time.deltaTime * 2);
            } else {
                guiColorProgress = Util.Clamp(0.2f, 1f, guiColorProgress + Time.deltaTime * 2);
            }
            GUI.color = Color.Lerp(guiOffColor, guiOnColor, guiColorProgress);

            // Ratio
            GUI.Label(new Rect(10, 10, 100, 25), "Camera Ratio:");
            string newXstr, newYstr;
            newXstr = GUI.TextField(new Rect(10, 35, 45, 25), forceRatio.ratio.x.ToString());
            newYstr = GUI.TextField(new Rect(65, 35, 45, 25), forceRatio.ratio.y.ToString());

            float newX;
            float newY;

            bool usingDefault = false;
            // Use buttons to travers ratio defaults
            if (GUI.Button(new Rect(10, 70, 45, 25), "<")) {
                usingDefault = true;
                currentDefault--;
                if (currentDefault < 0) {
                    currentDefault = defaultRatios.Length - 1;
                }
                forceRatio.ratio = new Vector2(defaultRatios[currentDefault].x, defaultRatios[currentDefault].y);
            }

            if (GUI.Button(new Rect(65, 70, 45, 25), ">")) {
                usingDefault = true;
                currentDefault++;
                if (currentDefault >= defaultRatios.Length) {
                    currentDefault = 0;
                }
                forceRatio.ratio = new Vector2(defaultRatios[currentDefault].x, defaultRatios[currentDefault].y);
            }

            // Else parse the users custom ratio
            if (!usingDefault && float.TryParse(newXstr, out newX) && float.TryParse(newYstr, out newY)) {
                forceRatio.ratio = new Vector2(newX, newY);
            }

            // Camera Options
            if (GUI.Button(new Rect(10, 105, 100, 25), "Single Camera")) {
                EnableCameras(singlePlayerCameras, true);
                EnableCameras(twoPlayerCameras, false);
                EnableCameras(fourPlayerCameras, false);
                EnableCameras(crazyCameras, false);

            }
            if (GUI.Button(new Rect(10, 140, 100, 25), "Two Player")) {
                EnableCameras(singlePlayerCameras, false);
                EnableCameras(twoPlayerCameras, true);
                EnableCameras(fourPlayerCameras, false);
                EnableCameras(crazyCameras, false);

            } 
            if (GUI.Button(new Rect(10, 175, 100, 25), "Four Player")) {
                EnableCameras(singlePlayerCameras, false);
                EnableCameras(twoPlayerCameras, false);
                EnableCameras(fourPlayerCameras, true);
                EnableCameras(crazyCameras, false);

            } 
            if (GUI.Button(new Rect(10, 215, 100, 25), "Various Angles")) {
                EnableCameras(singlePlayerCameras, false);
                EnableCameras(twoPlayerCameras, false);
                EnableCameras(fourPlayerCameras, false);
                EnableCameras(crazyCameras, true);
            }

            // Letterbox Color
            GUI.Label(new Rect(10, 250, 100, 25), "Letterbox Color");
            string redStr, greenStr, blueStr, rOrigin, gOrigin, bOrigin;

            if (forceRatio.letterBoxCameraColor.r == 0) {
                rOrigin = "";
            } else {
                rOrigin = (forceRatio.letterBoxCameraColor.r * 255f).ToString();
            }
            if (forceRatio.letterBoxCameraColor.g == 0) {
                gOrigin = "";
            } else {
                gOrigin = (forceRatio.letterBoxCameraColor.g * 255f).ToString();
            }
            if (forceRatio.letterBoxCameraColor.b == 0) {
                bOrigin = "";
            } else {
                bOrigin = (forceRatio.letterBoxCameraColor.b * 255f).ToString();
            }
            redStr = GUI.TextField(new Rect(10, 275, 35, 25), rOrigin);
            greenStr = GUI.TextField(new Rect(45, 275, 35, 25), gOrigin);
            blueStr = GUI.TextField(new Rect(80, 275, 35, 25), bOrigin);

            if (redStr == "") {
                redStr = "0";
            }
            if (greenStr == "") {
                greenStr = "0";
            }
            if (blueStr == "") {
                blueStr = "0";
            }

            float r, g, b;        
            if (float.TryParse(redStr, out r) && float.TryParse(greenStr, out g) && float.TryParse(blueStr, out b)) {
                if (r > 0) {
                    r = r / 255f;
                } else {
                    r = 0;
                }
                if (g > 0) {
                    g = g / 255f;
                } else {
                    g = 0;
                }
                if (b > 0) {
                    b = b / 255f;
                } else {
                    b = 0;
                }
                forceRatio.letterBoxCameraColor = new Color(Util.Clamp(0, 1, r), Util.Clamp(0, 1, g), Util.Clamp(0, 1, b), 1);
            }
        }
    }
}