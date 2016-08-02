﻿//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Full body animation using EnfluxVR suit
//
//======================================================================

using UnityEngine;
using System;

public class EVRFullAnimator : EVRHumanoidLimbMap, ILimbAnimator {

    private ILimbAnimator upperAnim;
    private ILimbAnimator lowerAnim;
    public DataRecording recorder;
    private float[] upper = new float[20];
    private float[] lower = new float[20];

    //temporary
    private bool shouldInit = false;

    // Use this for initialization
    void Start () {
        upperAnim = GameObject.Find("EVRUpperLimbMap")
            .GetComponent<EVRUpperLimbMap>();

        lowerAnim = GameObject.Find("EVRLowerLimbMap")
            .GetComponent<EVRLowerLimbMap>();
	}

    public void setInit()
    {
        shouldInit = true;
        upperAnim.setInit();
        lowerAnim.setInit();
    }

    public void resetInit()
    {
        upperAnim.resetInit();
        lowerAnim.resetInit();
    }

    public void operate(float[] angles)
    {
        if(angles != null)
        {
            Buffer.BlockCopy(angles, 0, upper, 0, 20 * sizeof(float));
            upperAnim.operate(upper);

            Buffer.BlockCopy(angles, 20 * sizeof(float), lower, 0, 20 * sizeof(float));
            lowerAnim.operate(lower);
        }

        if (recorder !=null && recorder.shouldRecord() || shouldInit)
        {
            recorder.addUpper((float[])upper.Clone());
            recorder.addLower((float[])lower.Clone());

            shouldInit = false;
        }
    }
}
