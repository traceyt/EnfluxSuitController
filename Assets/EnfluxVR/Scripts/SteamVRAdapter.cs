//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Demo adapter for use with SteamVR and HTC Vive
//
//======================================================================

using UnityEngine;

public class SteamVRAdapter : MonoBehaviour
{

    public GameObject hmd;
    public GameObject eyeLocation;


    // Use this for initialization
    void Start()
    {
        //body needs to align with headset from the start
        transform.rotation = Quaternion
                .AngleAxis(hmd.transform.rotation.eulerAngles.y, Vector3.up);
    }

    void Update()
    {
        
    }

    void LateUpdate()
    {
        Vector3 difference = hmd.transform.position - eyeLocation.transform.position;
        transform.Translate(difference);
    }



    public GameObject getHmd()
    {
        return hmd;
    }
}
