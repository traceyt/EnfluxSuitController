﻿//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Upperbody mapping and operation of EnfluxVR suit
//
//======================================================================

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EVRUpperLimbMap : EVRHumanoidLimbMap, ILimbAnimator {
    JointRotations jointRotations = new JointRotations();
    private Quaternion chain;

    private float[] initCore = new float[] { 0, 0, 0 };
    private float[] initLeftUpper = new float[] { 0, 0, 0 };
    private float[] initLeftFore = new float[] { 0, 0, 0 };
    private float[] initRightUpper = new float[] { 0, 0, 0 };
    private float[] initRightFore = new float[] { 0, 0, 0 };
    private Quaternion initHeadPose = new Quaternion();
    private Quaternion initCorePose = new Quaternion();
    private Queue<Quaternion> corePose = new Queue<Quaternion>();
    private Queue<Quaternion> rightUpperPose = new Queue<Quaternion>();
    private Queue<Quaternion> rightForePose = new Queue<Quaternion>();
    private Queue<Quaternion> leftUpperPose = new Queue<Quaternion>();
    private Queue<Quaternion> leftForePose = new Queue<Quaternion>();

    public void setInit()
    {
        initState = InitState.INIT;
        StartCoroutine(setPoses());        
    }

    public void resetInit()
    {
        initState = InitState.PREINIT;
        StopAllCoroutines();
    }

    private void setInitRot()
    {
        initCorePose = jointRotations.rotateCore(new float[] { 0, 0, initCore[2] }, 
            new float[] { 0, 0, 0 }, refCoord.localRotation);

        //set core rotation to get heading right
        //core.localRotation = initCorePose;

        initHeadPose = head.rotation;
    }

    private IEnumerator setPoses()
    {
        while (true)
        {

            //only animate the head if there is a hmd
            if (getLiveHMD())
            {
                head.rotation = hmd.rotation;
            }

            if (corePose.Count > 0)
            {
                core.localRotation = corePose.Dequeue();
            }

            if(rightUpperPose.Count > 0)
            {
                rightUpper.localRotation = rightUpperPose.Dequeue();
            }

            if (rightForePose.Count > 0)
            {
                rightFore.localRotation = rightForePose.Dequeue();
            }

            if(leftUpperPose.Count > 0)
            {
                leftUpper.localRotation = leftUpperPose.Dequeue();
            }

            if (leftForePose.Count > 0)
            {
                leftFore.localRotation = leftForePose.Dequeue();
            }

            yield return null;
        }
    }

    //interface method
    public void operate(float[] angles)
    {
        //parse angles
        //apply to upper        
        if (initState == InitState.PREINIT && angles != null)
        {
            //do initialization            
            Buffer.BlockCopy((float[])angles.Clone(), 1 * sizeof(float), initCore, 0, 3 * sizeof(float));
            Buffer.BlockCopy(angles, 5 * sizeof(float), initLeftUpper, 0, 3 * sizeof(float));
            Buffer.BlockCopy(angles, 9 * sizeof(float), initLeftFore, 0, 3 * sizeof(float));
            Buffer.BlockCopy(angles, 13 * sizeof(float), initRightUpper, 0, 3 * sizeof(float));
            Buffer.BlockCopy(angles, 17 * sizeof(float), initRightFore, 0, 3 * sizeof(float));

            setInitRot();

        } else if (initState == InitState.INIT && angles != null)
        {
            //core node 1
            float[] coreAngles = new float[] { angles[1], angles[2], angles[3] - initCore[2] };
            chain = jointRotations.rotateCore(coreAngles, initCore, refCoord.localRotation);            

            corePose.Enqueue(chain);

            //Left Upper user node 2
            //90 deg transform puts sensor in correct orientation
            float[] luAngles = new float[] { angles[5], angles[6], angles[7] - initCore[2] };
            chain = jointRotations.rotateLeftArm(luAngles, core.localRotation, refCoord.localRotation);

            leftUpperPose.Enqueue(chain);

            //Left Fore node 4
            float[] lfAngles = new float[] { angles[9], angles[10], angles[11] - initCore[2] };            
            chain = jointRotations.rotateLeftForearm(lfAngles, core.localRotation,
                leftUpper.localRotation, refCoord.localRotation);

            leftForePose.Enqueue(chain);

            //Right Upper node 3
            float[] ruAngles = new float[] { angles[13], angles[14], angles[15] - initCore[2] };
            chain = jointRotations.rotateRightArm(ruAngles, core.localRotation, refCoord.localRotation);

            rightUpperPose.Enqueue(chain);

            //Right Fore (Animation) Right Fore (User) node 5
            float[] rfAngles = new float[] { angles[17], angles[18], angles[19] - initCore[2] };
            chain = jointRotations.rotateRightForearm(rfAngles, core.localRotation, 
                rightUpper.localRotation, refCoord.localRotation);

            rightForePose.Enqueue(chain);
        }
    }
}