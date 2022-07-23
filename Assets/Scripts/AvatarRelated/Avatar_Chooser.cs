using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System.Text;
using System.Linq;
using RootMotion.FinalIK;
using RootMotion.Demos;

public class Avatar_Chooser : MonoBehaviour
{
    //[HideInInspector]
    public GameObject FemaleAsian, FemaleCaucasian, FemaleBlack, FemaleIndian, MaleAsian, MaleBlack, MaleCaucasian, MaleIndian;
    //[HideInInspector]
    public GameObject LeftFootTracker, RightFootTracker, Tracker1, Tracker2, PelvisTracker, LeftHandTracker, RightHandTracker, Tracker3, Tracker4, Tracker5;
    [Header("Select the Gender of the Self-Avatar")]
    public bool Male;
    public bool Female;
    [Header("Select the Race of the Self-Avatar")]
    public bool Asian;
    public bool Black;
    public bool Caucasian;
    public bool Indian;
    private GameObject Avatar;
    private GameObject Pelvis, Head;

    private GameObject leftHand, rightHand, leftLeg, rightLeg, pelvis, head, left_Hand_Target, right_Hand_Target, left_Foot_Target, right_Foot_Target, pelvis_Target, head_Target;
    private Quaternion leftHand_Default_Rotation, left_Foot_Default_Rotation, rightHand_Default_Rotation, right_Foot_Default_Rotation, pelvis_Default_Rotation, head_Default_Rotation;
    private Quaternion leftHand_Updated_Rotation, left_Foot_Updated_Rotation, rightHand_Updated_Rotation, right_Foot_Updated_Rotation, pelvis_Updated_Rotation, head_Updated_Rotation;
    private Quaternion calculated_difference_of_leftHand, calculated_difference_of_rightHand, calculated_difference_of_leftFoot, calculated_difference_of_rightFoot, calculated_difference_of_pelvis, calculated_difference_of_head;
    private int count;
    private StringBuilder result;
    private uint tracker;
    private VRIKCalibrationController vcc;
    private GameObject Left, Right;
    private int tracker_b_left, tracker_b_right, tracker_c_left, tracker_c_right, tracker_d_left, tracker_d_right;

    // Start is called before the first frame update
    void Start()
    {
        count = 1;
        tracker = 1;

        var error = ETrackedPropertyError.TrackedProp_Success;
        for (uint i = 0; i < 10; i++)
        {
            result = new StringBuilder((int)64);
            OpenVR.System.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);
            //Debug.Log("Result = " + result.ToString());
            if (result.ToString().Contains("tracker"))
            {
                if (tracker == 1)
                    Tracker1.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
                else if (tracker == 2)
                    Tracker2.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
                else if (tracker == 3)
                    Tracker3.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
                else if (tracker == 4)
                    Tracker4.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
                else if (tracker == 5)
                    Tracker5.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
                ++tracker;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (count < 3)
            count++;
        else if (count == 3)
        {
            //if (SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any)) // This if statement is only used while testing the avatars
            //{
            GameObject[] tracker_array = new GameObject[] { Tracker1, Tracker2, Tracker3, Tracker4, Tracker5 };
            tracker_array = tracker_array.OrderByDescending(v => Mathf.Round((v.transform.position.y) * 10f) / 10f).ToArray<GameObject>();

            //GameObject a = GameObject.Find("[CameraRig]/Camera"); // This is for SteamVR Interaction System
            GameObject a = GameObject.Find("XR Origin/Camera Offset/Main Camera"); // This is for Unity's XR Interaction System
            a.transform.rotation = Quaternion.Euler(0, a.transform.rotation.eulerAngles.y, 0);
            Quaternion main_rotation = a.transform.rotation;
            Vector3 main_position = new Vector3(a.transform.position.x, 0, a.transform.position.z);
            a.transform.position = main_position;
            a.transform.position += a.transform.forward;

            GameObject b = tracker_array[0];
            GameObject c = tracker_array[1];
            GameObject d = tracker_array[2];
            GameObject e = tracker_array[3];
            GameObject f = tracker_array[4];
            Vector3 b_pos = new Vector3(b.transform.position.x, 0, b.transform.position.z);
            Vector3 c_pos = new Vector3(c.transform.position.x, 0, c.transform.position.z);
            Vector3 d_pos = new Vector3(d.transform.position.x, 0, d.transform.position.z);
            Vector3 e_pos = new Vector3(e.transform.position.x, 0, e.transform.position.z);
            Vector3 f_pos = new Vector3(f.transform.position.x, 0, f.transform.position.z);
            b.transform.position = b_pos;
            c.transform.position = c_pos;
            d.transform.position = d_pos;
            e.transform.position = e_pos;
            f.transform.position = f_pos;
            b.transform.LookAt(a.transform);
            c.transform.LookAt(a.transform);
            d.transform.LookAt(a.transform);
            e.transform.LookAt(a.transform);
            f.transform.LookAt(a.transform);

            Tracker_Calibration(b, c, a);
            if (Left == b)
                tracker_b_left++;
            else
                tracker_b_right++;

            if (Right == c)
                tracker_c_right++;
            else
                tracker_c_left++;

            Tracker_Calibration(b, d, a);
            if (Left == b)
                tracker_b_left++;
            else
                tracker_b_right++;

            if (Right == d)
                tracker_d_right++;
            else
                tracker_d_left++;

            Tracker_Calibration(c, d, a);
            if (Left == c)
                tracker_c_left++;
            else
                tracker_c_right++;

            if (Right == d)
                tracker_d_right++;
            else
                tracker_d_left++;

            if (tracker_b_left >= 2 && tracker_b_right == 0)
            {
                uint index = (uint)b.GetComponent<SteamVR_TrackedObject>().index;
                LeftHandTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;
            }
            else if (tracker_b_left >= 1 && tracker_b_right <= 1)
            {
                uint index = (uint)b.GetComponent<SteamVR_TrackedObject>().index;
                PelvisTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;
            }
            else if (tracker_b_left == 0 && tracker_b_right >= 2)
            {
                uint index = (uint)b.GetComponent<SteamVR_TrackedObject>().index;
                RightHandTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;
            }

            if (tracker_c_left >= 2 && tracker_c_right == 0)
            {
                uint index = (uint)c.GetComponent<SteamVR_TrackedObject>().index;
                LeftHandTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;
            }
            else if (tracker_c_left >= 1 && tracker_c_right <= 1)
            {
                uint index = (uint)c.GetComponent<SteamVR_TrackedObject>().index;
                PelvisTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;
            }
            else if (tracker_c_left == 0 && tracker_c_right >= 2)
            {
                uint index = (uint)c.GetComponent<SteamVR_TrackedObject>().index;
                RightHandTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;
            }

            if (tracker_d_left >= 2 && tracker_d_right == 0)
            {
                uint index = (uint)d.GetComponent<SteamVR_TrackedObject>().index;
                LeftHandTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;
            }
            else if (tracker_d_left >= 1 && tracker_d_right <= 1)
            {
                uint index = (uint)d.GetComponent<SteamVR_TrackedObject>().index;
                PelvisTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;
            }
            else if (tracker_d_left == 0 && tracker_d_right >= 2)
            {
                uint index = (uint)d.GetComponent<SteamVR_TrackedObject>().index;
                RightHandTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;
            }

            Tracker_Calibration(e, f, a);
            if (Left == e)
            {
                uint index = (uint)e.GetComponent<SteamVR_TrackedObject>().index;
                LeftFootTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;
            }
            else
            {
                uint index = (uint)e.GetComponent<SteamVR_TrackedObject>().index;
                RightFootTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;
            }

            if (Left == f)
            {
                uint index = (uint)f.GetComponent<SteamVR_TrackedObject>().index;
                LeftFootTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;
            }
            else
            {
                uint index = (uint)f.GetComponent<SteamVR_TrackedObject>().index;
                RightFootTracker.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;
            }

            count++;
            //}
        }
        else if (count == 4)
        {
            //if (Female == true && Male == false)
            //{
            //    if(Asian == true)
            //    {
            //        Setup(FemaleAsian);
            //        count++;
            //    }
            //}
            if (Male == true && Female == false)
            {
                if (Asian == true && Black == true && Caucasian == true && Indian == true)
                {
                    Debug.LogError("All of the Races cannot be selected.\nPlease select any one Race of the avatar.");
                    Asian = false;
                    Black = false;
                    Caucasian = false;
                    Indian = false;
                }
                else if ((Asian == true && Black == true && Caucasian == true && Indian == false) || (Asian == true && Black == true && Caucasian == false && Indian == true) ||
                        (Asian == true && Black == true && Caucasian == false && Indian == false) || (Asian == true && Black == false && Caucasian == true && Indian == true) ||
                        (Asian == true && Black == false && Caucasian == true && Indian == false) || (Asian == true && Black == false && Caucasian == false && Indian == true) ||
                        (Asian == false && Black == true && Caucasian == true && Indian == true) || (Asian == false && Black == true && Caucasian == true && Indian == false) ||
                        (Asian == false && Black == true && Caucasian == false && Indian == true) || (Asian == false && Black == false && Caucasian == true && Indian == true))

                {
                    Debug.LogError("More than one Race cannot be selected.\nPlease select any one Race of the avatar.");
                    Asian = false;
                    Black = false;
                    Caucasian = false;
                    Indian = false;
                }
                else if (Asian == true && Black == false && Caucasian == false && Indian == false)
                {
                    Setup(MaleAsian);
                    count++;
                }
                else if (Asian == false && Black == true && Caucasian == false && Indian == false)
                {
                    Setup(MaleBlack);
                    count++;
                }
                else if (Asian == false && Black == false && Caucasian == true && Indian == false)
                {
                    Setup(MaleCaucasian);
                    count++;
                }
                else if (Asian == false && Black == false && Caucasian == false && Indian == true)
                {
                    Setup(MaleIndian);
                    count++;
                }
            }
            else if (Male == false && Female == true)
            {
                if (Asian == true && Black == true && Caucasian == true && Indian == true)
                {
                    Debug.LogError("All of the Races cannot be selected.\nPlease select any one Race of the avatar.");
                    Asian = false;
                    Black = false;
                    Caucasian = false;
                    Indian = false;
                }
                else if ((Asian == true && Black == true && Caucasian == true && Indian == false) || (Asian == true && Black == true && Caucasian == false && Indian == true) ||
                        (Asian == true && Black == true && Caucasian == false && Indian == false) || (Asian == true && Black == false && Caucasian == true && Indian == true) ||
                        (Asian == true && Black == false && Caucasian == true && Indian == false) || (Asian == true && Black == false && Caucasian == false && Indian == true) ||
                        (Asian == false && Black == true && Caucasian == true && Indian == true) || (Asian == false && Black == true && Caucasian == true && Indian == false) ||
                        (Asian == false && Black == true && Caucasian == false && Indian == true) || (Asian == false && Black == false && Caucasian == true && Indian == true))

                {
                    Debug.LogError("More than one Race cannot be selected.\nPlease select any one Race of the avatar.");
                    Asian = false;
                    Black = false;
                    Caucasian = false;
                    Indian = false;
                }
                else if (Asian == true && Black == false && Caucasian == false && Indian == false)
                {
                    Setup(FemaleAsian);
                    count++;
                }
                else if (Asian == false && Black == true && Caucasian == false && Indian == false)
                {
                    Setup(FemaleBlack);
                    count++;
                }
                else if (Asian == false && Black == false && Caucasian == true && Indian == false)
                {
                    Setup(FemaleCaucasian);
                    count++;
                }
                else if (Asian == false && Black == false && Caucasian == false && Indian == true)
                {
                    Setup(FemaleIndian);
                    count++;
                }
            }
            else if (Male == true && Female == true)
            {
                Debug.LogError("Both of the Genders cannot be selected.\nPlease select any one Gender of the avatar.");
                Male = false;
                Female = false;
            }
        }
        else if (count == 5)
        {
            if (head_Target == null)
                //head_Target = GameObject.Find("[CameraRig]/Camera/Head Target");
                head_Target = GameObject.Find("XR Origin/Camera Offset/Main Camera/Head Target");

            if (pelvis_Target == null)
                pelvis_Target = GameObject.Find("Trackers/Pelvis Tracker/Pelvis Target");

            if (left_Hand_Target == null)
                //left_Hand_Target = GameObject.Find("[CameraRig]/Controller (left)/Left Hand Target");
                left_Hand_Target = GameObject.Find("Trackers/Left Hand Tracker/Left Hand Target");

            if (right_Hand_Target == null)
                //right_Hand_Target = GameObject.Find("[CameraRig]/Controller (right)/Right Hand Target");
                right_Hand_Target = GameObject.Find("Trackers/Right Hand Tracker/Right Hand Target");

            if (left_Foot_Target == null)
                left_Foot_Target = GameObject.Find("Trackers/Left Foot Tracker/Left Foot Target");

            if (right_Foot_Target == null)
                right_Foot_Target = GameObject.Find("Trackers/Right Foot Tracker/Right Foot Target");

            if (vcc != null)
            {
                if (vcc.C_key_pressed == true)
                {
                    //Debug.Log("C key is pressed!!!");
                    //count++;
                    Calibrate();
                    count++;
                }
            }
        }
        //else if (count == 6)
        //{
        //    Calibrate();
        //    count++;
        //}
    }

    void Tracker_Calibration(GameObject tracker_A, GameObject tracker_B, GameObject Camera)
    {
        GameObject midPointGameObject = Camera;
        Vector3 midpoint = new Vector3((tracker_A.transform.position.x + tracker_B.transform.position.x) / 2, 0, (tracker_A.transform.position.z + tracker_B.transform.position.z) / 2);
        midPointGameObject.transform.position = midpoint;
        tracker_A.transform.LookAt(midPointGameObject.transform);
        tracker_B.transform.LookAt(midPointGameObject.transform);

        Quaternion a = tracker_A.transform.rotation;
        Quaternion b = tracker_B.transform.rotation;

        midPointGameObject.transform.position += midPointGameObject.transform.forward;
        tracker_A.transform.LookAt(midPointGameObject.transform);
        tracker_B.transform.LookAt(midPointGameObject.transform);

        Quaternion c = tracker_A.transform.rotation;
        Quaternion d = tracker_B.transform.rotation;
        Quaternion difference_of_tracker_A = a * Quaternion.Inverse(c);
        Quaternion difference_of_tracker_B = b * Quaternion.Inverse(d);

        Vector3 understandable_diff_of_tracker_A = new Vector3(0, 0, 0);
        Vector3 understandable_diff_of_tracker_B = new Vector3(0, 0, 0);

        if (difference_of_tracker_A.eulerAngles.y > 180)
            understandable_diff_of_tracker_A += new Vector3(difference_of_tracker_A.eulerAngles.x, difference_of_tracker_A.eulerAngles.y - 360, difference_of_tracker_A.eulerAngles.z);
        else
            understandable_diff_of_tracker_A += new Vector3(difference_of_tracker_A.eulerAngles.x, difference_of_tracker_A.eulerAngles.y, difference_of_tracker_A.eulerAngles.z);

        if (difference_of_tracker_B.eulerAngles.y > 180)
            understandable_diff_of_tracker_B += new Vector3(difference_of_tracker_B.eulerAngles.x, difference_of_tracker_B.eulerAngles.y - 360, difference_of_tracker_B.eulerAngles.z);
        else
            understandable_diff_of_tracker_B += new Vector3(difference_of_tracker_B.eulerAngles.x, difference_of_tracker_B.eulerAngles.y, difference_of_tracker_B.eulerAngles.z);

        if (understandable_diff_of_tracker_A.y < 0)
            Right = tracker_A;
        else
            Left = tracker_A;

        if (understandable_diff_of_tracker_B.y < 0)
            Right = tracker_B;
        else
            Left = tracker_B;
    }
    void Calibrate()
    {
        if (head_Target != null && pelvis_Target != null && left_Hand_Target != null && right_Hand_Target != null && left_Foot_Target != null && right_Foot_Target != null && vcc.C_key_pressed == true)
        {
            left_Foot_Updated_Rotation = leftLeg.transform.localRotation;
            calculated_difference_of_leftFoot = left_Foot_Default_Rotation * Quaternion.Inverse(left_Foot_Updated_Rotation);

            right_Foot_Updated_Rotation = rightLeg.transform.localRotation;
            calculated_difference_of_rightFoot = right_Foot_Default_Rotation * Quaternion.Inverse(right_Foot_Updated_Rotation);

            pelvis_Updated_Rotation = pelvis.transform.localRotation;
            calculated_difference_of_pelvis = pelvis_Default_Rotation * Quaternion.Inverse(pelvis_Updated_Rotation);

            leftHand_Updated_Rotation = leftHand.transform.localRotation;
            calculated_difference_of_leftHand = leftHand_Default_Rotation * Quaternion.Inverse(leftHand_Updated_Rotation);

            rightHand_Updated_Rotation = rightHand.transform.localRotation;
            calculated_difference_of_rightHand = rightHand_Default_Rotation * Quaternion.Inverse(rightHand_Updated_Rotation);

            head_Updated_Rotation = head.transform.localRotation;
            calculated_difference_of_head = head_Default_Rotation * Quaternion.Inverse(head_Updated_Rotation);

            Quaternion a = left_Hand_Target.transform.localRotation;
            a *= calculated_difference_of_leftHand;
            left_Hand_Target.transform.localRotation = a;

            Quaternion b = right_Hand_Target.transform.localRotation;
            b *= calculated_difference_of_rightHand;
            right_Hand_Target.transform.localRotation = b;

            Quaternion e = pelvis_Target.transform.localRotation;
            e *= calculated_difference_of_pelvis;
            pelvis_Target.transform.localRotation = e;

            Quaternion f = head_Target.transform.localRotation;
            f *= calculated_difference_of_head;
            head_Target.transform.localRotation = f;

            Quaternion c = left_Foot_Target.transform.localRotation;
            c *= calculated_difference_of_leftFoot;
            left_Foot_Target.transform.localRotation = c;

            Quaternion d = right_Foot_Target.transform.localRotation;
            d *= calculated_difference_of_rightFoot;
            right_Foot_Target.transform.localRotation = d;

            Vector3 head_target_position = head_Target.transform.localPosition;
            Vector3 pelvis_target_position = pelvis_Target.transform.localPosition;


            Vector3 head_Offset = Head.transform.localPosition;
            head_target_position -= head_Offset;
            //head_Target.transform.localPosition = head_target_position + new Vector3(-0.122f, 0.028f, -0.179f);
            //head_Target.transform.localRotation *= Quaternion.Euler(6.4f, 0.581f, 22.3f);




            Vector3 newAvatarScale = new Vector3(Avatar.transform.localScale.x - (head_Offset.y / 1.5f), Avatar.transform.localScale.y - (head_Offset.y / 1.5f), Avatar.transform.localScale.z - (head_Offset.y / 1.5f));
            Avatar.transform.localScale = newAvatarScale;

            //Debug.Log("Pelvis Target position = " + pelvis_target_position.ToString("F4"));

            float zPosVal = Mathf.Abs(pelvis_target_position.z);
            float zCalculatedVal = zPosVal / 1.5f;
            float xyCalculatedVal = zCalculatedVal / 1.5f;
            //pelvis_target_position -= new Vector3(0.1087f, 0.0438f, -0.048f);


            //Debug.Log("Pelvis Target position after x, y and z = " + pelvis_target_position.ToString("F4"));

            if (PelvisTracker.transform.rotation.eulerAngles.z > 180)
                pelvis_target_position += new Vector3(0.2187f, 0.1571f, 0.1f);
            else
                pelvis_target_position += new Vector3(-0.3f, -0.294f, 0.12f);

            //if (PelvisTracker.transform.rotation.eulerAngles.z > 180 && Avatar.name == MaleAsian.name)
            //    pelvis_target_position += new Vector3(10f, 0.0471f, 0.1f);
            //else
            //    pelvis_target_position += new Vector3(0, 0, 0.1f);

            ////Debug.Log("Pelvis Target position after x1, y1 and z1 = " + pelvis_target_position.ToString("F4"));
            pelvis_Target.transform.localPosition = pelvis_target_position;
            //Debug.Log("Pelvis Target new position = " + pelvis_target_position.ToString("F4"));

            if (Avatar.name == FemaleAsian.name)
                CalibrationHelper(-20, 50, -30, 0, -60, 10, 15, -7, 0, 3, 0, -4/*, -5*/);
                head_target_position += new Vector3(-0.06f, 0.12f, -0.046f);
                if (PelvisTracker.transform.rotation.eulerAngles.z > 180)
                    pelvis_target_position += new Vector3(0.2f, 0.0171f, 0.38f);
                else
                    pelvis_target_position += new Vector3(-0.3f, -0.26f, 0.1f);
            //head_Target.transform.localPosition = head_target_position + new Vector3(-0.122f, 0.028f, -0.179f);
            //head_Target.transform.localRotation *= Quaternion.Euler(6.4f, 0.581f, 22.3f);


            if (Avatar.name == FemaleBlack.name)
                CalibrationHelper(-20, 50, -30, 0, -60, 10, 0, 0, 0, 0, -10, -10);
                head_target_position += new Vector3(0.033f, -0.06f, -0.08f);
                if (PelvisTracker.transform.rotation.eulerAngles.z > 180)
                    pelvis_target_position += new Vector3(0.2f, 0.0171f, 0.38f);
                else
                    pelvis_target_position += new Vector3(-0.3f, -0.26f, 0.1f);

            if (Avatar.name == FemaleCaucasian.name)
                CalibrationHelper(-20, 50, -30, 0, -60, 10, 0, 0, 0, 0, 0, 0);
                head_target_position += new Vector3(-0.02f, 0.014f, 0f);


            if (Avatar.name == FemaleIndian.name)
                CalibrationHelper(-20, 50, -30, 0, -60, 10, 3, 0, 0, 0, -10, -10);
                head_target_position += new Vector3(-0.05f, 0.08f, -0.12f);
            //head_target_position += new Vector3(-0.05f, 1900f, -0.14f);
            ////head_Target.transform.localPosition = head_target_position;
            //if (PelvisTracker.transform.rotation.eulerAngles.z > 180)
            //    pelvis_target_position += new Vector3(0.2f, 0.0471f, -0.4f);
            //else
            //    pelvis_target_position += new Vector3(-0.3f, -0.26f, 0.1f);
            //pelvis_Target.transform.localPosition = pelvis_target_position;
            if (Avatar.name == MaleAsian.name)
                CalibrationHelper(10, 60, -20, -20, -80, 15, 0, 0, -10,-10,-5,-10);
                head_target_position += new Vector3(0f, -0.1f, 0.01f);
                if (PelvisTracker.transform.rotation.eulerAngles.z > 180)
                    pelvis_target_position += new Vector3(0.2f, 0.0471f, 0.45f);
                else
                    pelvis_target_position += new Vector3(-0.3f, -0.26f, 0.1f);

            if (Avatar.name == MaleBlack.name)
                CalibrationHelper(10, 60, -20, -20, -80, 20, 10, 0, 0, 0,-5 ,-8/*, -5*/);
                head_target_position += new Vector3(0.073f, -0.13f, 0f);
            //head_Target.transform.localRotation *= Quaternion.Euler(-15, 0, 0);
            //head_Target.transform.localRotation *= Quaternion.Euler(0, -25, 0);
            //head_Target.transform.localRotation *= Quaternion.Euler(0, 0, -25);

                if (PelvisTracker.transform.rotation.eulerAngles.z > 180)
                    pelvis_target_position += new Vector3(0.2f, 0.0471f, 0.38f);
                else
                    pelvis_target_position += new Vector3(-0.3f, -0.26f, 0.1f);
               




            if (Avatar.name == MaleCaucasian.name)
                CalibrationHelper(10, 60, -20, -20, -80, 15, 15, 0, 0, 0, -6, -5);
                head_target_position += new Vector3(-0.033f, 0.05f, 0.01f);
                if (PelvisTracker.transform.rotation.eulerAngles.z > 180)
                    pelvis_target_position += new Vector3(0.2f, 0.011f, 5.38f);
                else
                    pelvis_target_position += new Vector3(-0.3f, -0.26f, 0.1f);


            if (Avatar.name == MaleIndian.name)
                CalibrationHelper(10, 60, -20, -20, -80, 20, 8, 0, 0, 0, -3, -7/*, -5*/);
                head_target_position += new Vector3(0f, -0.03f, 0.09f);
                if (PelvisTracker.transform.rotation.eulerAngles.z > 180)
                    pelvis_target_position += new Vector3(0.2f, 0.0471f, 0.45f);
                else
                    pelvis_target_position += new Vector3(-0.3f, -0.26f, 0.1f);

            head_Target.transform.localPosition = head_target_position;

            //Avatar.GetComponent<VRIK>().solver.plantFeet = true;
        }
    }

    void CalibrationHelper(float leftHandX, float leftHandY, float leftHandZ, float rightHandX, float rightHandY, float rightHandZ, float leftFootX,float rightFootX,float leftFootY, float rightFootY, float leftFootZ, float rightFootZ/*, float rightFootX*/)
    {
        Quaternion leftHandOffset = left_Hand_Target.transform.localRotation;
        leftHandOffset *= Quaternion.Euler(new Vector3(0, leftHandY, 0));
        leftHandOffset *= Quaternion.Euler(new Vector3(0, 0, leftHandZ));
        leftHandOffset *= Quaternion.Euler(new Vector3(leftHandX, 0, 0));

        left_Hand_Target.transform.localRotation = leftHandOffset;


        Quaternion rightHandOffset = right_Hand_Target.transform.localRotation;
        rightHandOffset *= Quaternion.Euler(new Vector3(0, rightHandY, 0));
        rightHandOffset *= Quaternion.Euler(new Vector3(0, 0, rightHandZ));
        rightHandOffset *= Quaternion.Euler(new Vector3(rightHandX, 0, 0));
        right_Hand_Target.transform.localRotation = rightHandOffset;



        Quaternion leftFootOffset = left_Foot_Target.transform.localRotation;
        leftFootOffset *= Quaternion.Euler(0, leftFootY, 0);
        left_Foot_Target.transform.localRotation = leftFootOffset;

        Quaternion rightFootOffset = right_Foot_Target.transform.localRotation;
        rightFootOffset *= Quaternion.Euler(0, rightFootY, 0);
        right_Foot_Target.transform.localRotation = rightFootOffset;

        Quaternion rightFootNewOffset = right_Foot_Target.transform.localRotation;
        rightFootNewOffset *= Quaternion.Euler(0, 0, rightFootZ);
        right_Foot_Target.transform.localRotation = rightFootNewOffset;

        Quaternion XrightFootNewOffset = right_Foot_Target.transform.localRotation;
        XrightFootNewOffset *= Quaternion.Euler(rightFootX, 0, 0);
        right_Foot_Target.transform.localRotation = XrightFootNewOffset;


        Quaternion leftFootNewOffset = left_Foot_Target.transform.localRotation;
        leftFootNewOffset *= Quaternion.Euler(0, 0, leftFootZ);
        left_Foot_Target.transform.localRotation = leftFootNewOffset;

        

        //Quaternion rightFootNew1Offset = right_Foot_Target.transform.localRotation;
        //rightFootNew1Offset *= Quaternion.Euler(rightFootX, 0, 0);
        //right_Foot_Target.transform.localRotation = rightFootNew1Offset;
    }

    void Setup(GameObject avatar)
    {
        Avatar = avatar;
        avatar.SetActive(true);

        //leftHand = GameObject.Find(avatar.name + "/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand");
        //rightHand = GameObject.Find(avatar.name + "/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand");
        //leftLeg = GameObject.Find(avatar.name + "/mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot");
        //rightLeg = GameObject.Find(avatar.name + "/mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg/mixamorig:RightFoot");
        //pelvis = GameObject.Find(avatar.name + "/mixamorig:Hips");
        //head = GameObject.Find(avatar.name + "/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head");
        //Pelvis = GameObject.Find(pelvis.name + "/Pelvis");
        //Head = GameObject.Find(head.name + "/Head");


        leftHand = GameObject.Find(avatar.name + "/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");
        rightHand = GameObject.Find(avatar.name + "/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand");
        leftLeg = GameObject.Find(avatar.name + "/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot");
        rightLeg = GameObject.Find(avatar.name + "/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot");
        pelvis = GameObject.Find(avatar.name + "/Bip01/Bip01 Pelvis");
        head = GameObject.Find(avatar.name + "/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
        Pelvis = GameObject.Find(pelvis.name + "/Pelvis");
        //Head = GameObject.Find("/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Head");
        Head = GameObject.Find(head.name + "/Head");
        leftHand_Default_Rotation = leftHand.transform.localRotation;
        rightHand_Default_Rotation = rightHand.transform.localRotation;
        left_Foot_Default_Rotation = leftLeg.transform.localRotation;
        right_Foot_Default_Rotation = rightLeg.transform.localRotation;
        pelvis_Default_Rotation = pelvis.transform.localRotation;
        head_Default_Rotation = head.transform.localRotation;

        this.gameObject.GetComponent<VRIKCalibrationController>().ik = avatar.GetComponent<VRIK>();
        vcc = this.gameObject.GetComponent<VRIKCalibrationController>();
    }
}
