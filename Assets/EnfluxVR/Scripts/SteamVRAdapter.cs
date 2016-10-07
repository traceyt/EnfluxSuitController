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
    public GameObject humanoid;    

    // Use this for initialization
    void Start()
    {
        humanoid.transform.rotation = Quaternion
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
}
