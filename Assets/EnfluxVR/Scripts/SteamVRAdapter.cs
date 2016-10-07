//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Demo adapter for use with SteamVR and HTC Vive
//
//======================================================================

using UnityEngine;

[RequireComponent (typeof(EVRHumanoidLimbMap))]
public class SteamVRAdapter : MonoBehaviour
{
    public GameObject hmd;
    public GameObject eyeLocation;

    // Use this for initialization
    void Start()
    {
        transform.localRotation = Quaternion
            .AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);
    }

    void Update()
    {
    }

    void LateUpdate()
    {
        if (hmd != null)
        {
            Vector3 difference = hmd.transform.position - eyeLocation.transform.position;
            transform.Translate(difference, Space.World);
        }
    }
}
