using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;
using Valve.VR;

// This part is for XR Origin
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace RootMotion.Demos
{

    public class VRIKCalibrationController : MonoBehaviour
    {

        [Tooltip("Reference to the VRIK component on the avatar.")] public VRIK ik;
        [Tooltip("The settings for VRIK calibration.")] public VRIKCalibrator.Settings settings;
        [Tooltip("The HMD.")] public Transform headTracker;
        [Tooltip("(Optional) A tracker placed anywhere on the body of the player, preferrably close to the pelvis, on the belt area.")] public Transform bodyTracker;
        [Tooltip("(Optional) A tracker or hand controller device placed anywhere on or in the player's left hand.")] public Transform leftHandTracker;
        [Tooltip("(Optional) A tracker or hand controller device placed anywhere on or in the player's right hand.")] public Transform rightHandTracker;
        [Tooltip("(Optional) A tracker placed anywhere on the ankle or toes of the player's left leg.")] public Transform leftFootTracker;
        [Tooltip("(Optional) A tracker placed anywhere on the ankle or toes of the player's right leg.")] public Transform rightFootTracker;

        [Header("Data stored by Calibration")]
        public VRIKCalibrator.CalibrationData data = new VRIKCalibrator.CalibrationData();

        [HideInInspector]
        public bool C_key_pressed = false;
        private int count = 0;

        // This part is for XR Origin
        public InputHelpers.Button TriggerActivationButton;
        public float activationThresold = 0.1f;
        public XRController LeftController, RightController;
        private bool Controller_Activated;

        void LateUpdate()
        {
            // This part is for XR Origin
            bool x = CheckIfActivated(LeftController);
            bool y = CheckIfActivated(RightController);

            if (x == true || y == true)
                Controller_Activated = true;

            //if (Input.GetKeyDown(KeyCode.C) || SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any)) // This is for SteamVR Interaction system
            if (Input.GetKeyDown(KeyCode.C) || Controller_Activated == true) // This is for Unity's XR Interaction System
            {
                if (count == 0)
                {
                    // Calibrate the character, store data of the calibration
                    data = VRIKCalibrator.Calibrate(ik, settings, headTracker, bodyTracker, leftHandTracker, rightHandTracker, leftFootTracker, rightFootTracker);
                    C_key_pressed = true;
                    count++;
                }
            }

            /*
             * calling Calibrate with settings will return a VRIKCalibrator.CalibrationData, which can be used to calibrate that same character again exactly the same in another scene (just pass data instead of settings), 
             * without being dependent on the pose of the player at calibration time.
             * Calibration data still depends on bone orientations though, so the data is valid only for the character that it was calibrated to or characters with identical bone structures.
             * If you wish to use more than one character, it would be best to calibrate them all at once and store the CalibrationData for each one.
             * */
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (data.scale == 0f)
                {
                    Debug.LogError("No Calibration Data to calibrate to, please calibrate with settings first.");
                }
                else
                {
                    // Use data from a previous calibration to calibrate that same character again.
                    VRIKCalibrator.Calibrate(ik, data, headTracker, bodyTracker, leftHandTracker, rightHandTracker, leftFootTracker, rightFootTracker);
                }
            }

            // Recalibrates avatar scale only. Can be called only if the avatar has been calibrated already.
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (data.scale == 0f)
                {
                    Debug.LogError("Avatar needs to be calibrated before RecalibrateScale is called.");
                }
                VRIKCalibrator.RecalibrateScale(ik, data, settings);
            }
        }

        // This part is for XR ORigin
        public bool CheckIfActivated(XRController controller)
        {
            InputHelpers.IsPressed(controller.inputDevice, TriggerActivationButton, out bool isActivated, activationThresold);
            return isActivated;
        }

    }
}
