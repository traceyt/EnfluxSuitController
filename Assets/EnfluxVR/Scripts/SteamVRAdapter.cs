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
    public float offset = 1.9f;
    

    // Use this for initialization
    void Start()
    {
        gameObject.transform.rotation = Quaternion
                .AngleAxis(hmd.transform.rotation.eulerAngles.y, Vector3.up);
    }

    void Update()
    {

    }

    void LateUpdate()
    {

        gameObject.transform.position = hmd.transform.position - new Vector3(0,offset,0);
    }

    public GameObject getHmd()
    {
        return hmd;
    }
}
