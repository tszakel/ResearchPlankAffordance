using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // This namespace is necessary

public class controllerTest : MonoBehaviour
{
    public InputHelpers.Button teleportActivationButton; // Choose "Trigger" from the dropdown menu in inspector window in Unity
    public float activationThresold = 0.1f;
    public XRController LeftController, RightController; // Drag and Drop the LeftController and RightController GameObjects in these Objects in the inspector window in Unity
    private bool leftControllerActivated, rightControllerActivated, updateStopper;
    public GameObject Cube; // Just create a cube in the hierarchy window and drag it to this variable in the inspector window in Unity. This is just a test Cube!

    private void Start()
    {
        Cube.SetActive(false); // This cube is just for testing
    }

    // Update is called once per frame
    void Update()
    {
        leftControllerActivated = CheckIfActivated(LeftController);
        rightControllerActivated = CheckIfActivated(RightController);

        if (leftControllerActivated == true || rightControllerActivated == true) // This is the main part of the script where you will check whether the trigger button of either controller
                                                                                 // is pressed or not
        {
            Debug.Log("A controller is active");
            if (updateStopper == false) // This checking is for restricting the code from running multiple times as it is defined in the Update method
            {
                Cube.SetActive(true);
                updateStopper = true;
            }
        }
        else // Here you need to change the above boolean variable to false so that when you will press the trigger button once again later, the code will be executed once again!
        {
            updateStopper = false;
            Cube.SetActive(false);
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThresold);
        return isActivated;
    }
}
