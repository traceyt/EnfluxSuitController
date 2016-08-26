﻿using UnityEngine;
using System.Collections;

public class ReferenceCoord : MonoBehaviour {

    private GameObject vrAdapter;
    private Transform hmdTrans;
    private Quaternion baseReference = new Quaternion();
    private bool adapterReady = false;

	// Use this for initialization
	void Start () {
        vrAdapter = GameObject.Find("SteamVRAdapter");

        if (vrAdapter != null)
        {
            StartCoroutine(findAdapterAndReady());
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (adapterReady)
        {
            gameObject.transform.localRotation = Quaternion.Inverse(baseReference) *
            hmdTrans.localRotation;
        }
	}

    private IEnumerator findAdapterAndReady()
    {
        yield return new WaitForSeconds(0.5f);
        hmdTrans = vrAdapter.GetComponent<SteamVRAdapter>().getHmd().transform;
        baseReference = hmdTrans.localRotation;
        adapterReady = true;
    }
}
