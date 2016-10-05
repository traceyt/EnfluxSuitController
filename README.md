# EnfluxSuitController
The Enflux Unity SDK with support for VR, animation recording and playback

**Documentation and tutorial updating, check back frequently**

## UnitySDK
* Check [releases tab](https://github.com/Enflux/EnfluxSuitController/releases)

## Instructions for setup
1. Download EnfluxVR Unity SDK
2. Start or open a Unity project
3. Under the tab Assets select Import Package > Custom Package
4. Navigate to folder containing EnfluxVR.unitypackage and select package
5. A window will open, select Import
6. Under Project tab, expand EnfluxVR > Resources > Prefabs
7. Drag and drop the following into Hierarchy
  * [EnfluxVR]
  * [EnfluxVRHumanoid]
8. If one is not already in the project, add a Canvas
  * From EnfluxVR > Resources > Prefabs, drag EnfluxPanel onto Canvas
  * **Dragging EnfluxPanel into Scene rather than Hierarchy may have incorrect result**
9. Move camera as needed
* **OPTIONAL PANTS WORKAROUND** 
10. Under [EnfluxVRHumanoid] > EVRLowerLimbMap check Use Core
    * This will control waist transform using data from core sensor

## Instructions for setup with HTC Vive headset
1. Obtain and import [SteamVR Unity SDK](https://www.assetstore.unity3d.com/en/#!/content/32647)
2. If not completed already, follow section **Instructions for setup**
3. On [EnfluxVRHumanoid] in Inspector, select Add Component > Scripts > SteamVRAdapter 
4. Add MainCamera, and CameraMount to SteamVRAdapter public objects
    * CameraMount found at [EnfluxVRHumanoid] > humanoid > Armature > core > abdomen > abdomen.001 > chest > neck > head
    * Make sure MainCamera position and rotation are both [0, 0, 0]
    * In Edit > Project Settings > Player make sure "Virtual Reality Supported" is on

## Instructions for setup with Oculus Rift
* Requirements: [Oculus Unity SDK](https://developer3.oculus.com/downloads/)
1. If not completed already, follow all **Instructions for setup**
2. In the player character, create a transform where the head is centered facing forward
3. Add script "Enflux Humanoid Attachment" to the [EnfluxVRHumanoid]. Set the HMD to be MainCamera and the eye location to be the head transformation.
4. Any character movement should be set to transform a parent of the main camera. This will move the entire avatar.


## Instructions for use without headset
1. Insert Bluegiga BLED112 dongle in a USB port on Windows PC
2. Obtain and install drivers for BLED112 [link](http://www.picaxe.com/downloads/bled112.zip)
3. Run Unity project
4. Under COM Ports observe Bluegiga Bluetooth Low Energy (COMX)
5. Select Attach, then turn on EnfluxVR modules
6. Under Devices observe Enflux modules and select Connect
7. If first time using suit or change in environment, select Run Calibration
  * [Calibration Tutorial](https://youtu.be/HKrl9DVYESI)
8. Select Start Animate Mode 
  * Timer will start counting down, **USER NEEDS TO BE STANDING STILL FOR MOST ACCURACY**
9. When the countdown has completed, the suit should be animating
10. It is possible to record animations by setting the filename and checking the record checkbox. Uncheck at the end of the animation.
11. To stop, select Stop Animate Mode 
  * When done using the suit, select Disconnect

## Instructions for use with HTC Vive headset
* For time being, these steps need to be followed from a place where computer is easily accessible
1. Point headset in desired starting direction **Easiest is to point headset direction at monitor**
2. Run application
3. Follow steps **Instructions for use w/o headset** but omit Calibration step

# Prebuilt Scenes
* These can be found under [EnfluxVR/Scenes](Assets/EnfluxVR/Scenes)

### EnfluxSuitSetup
* This is a scene setup following **Instructions for use without headset**

### RecordRaw 
* Same as EnfluxSuitSetup, but when run, will create a file named "rawvals.csv" in the project's main folder

### SetupVR
* Setup following **Instructions for setup w/ HTC Vive headset** minus the steps requiring SteamVR

### RecordDemo
* Setup so that data from animation is logged into a user specified .csv, see instructions. **NOT TESTED WITH HEADSET**

### PlaybackDemo
* Animate model based on recorded data in a .csv. See instructions.

## Instructions for scene PlaybackDemo
1. Find and open scene under [EnfluxVR/Scenes](Assets/EnfluxVR/Scenes)
2. If recordings were not recorded in a default directory set the property "File Directory" in replay_humanoid->Playback Animator
3. Run app and type the animation filename in the text field used in the recording step
5. Click Start Playback to run the playback animation

* 
# Known Issues
* Driver for the Bluegiga dongle can be found [here](http://www.picaxe.com/downloads/bled112.zip)
* Occasionally, a hang up occurs and the dongle needs to be removed and then plugged back in. If the application is abrupted this may be necessary.  If there is an issue with connection this is almost a universal fix.
* **HAVING SOME SIGNAL STRENGTH ISSUES WITH THE PANTS, SEE INSTRUCTIONS FOR WORKAROUND**
 
&nbsp;
* License subject to change
* All code provided **AS IS** with **NO WARRANTY**
* Enflux Inc. is not responsible for lost work from program crashes. 
* **MAKE SURE ALL WORK IS SAVED BEFORE RUNNING**