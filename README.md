# Known Issues
* If you don't have Program Files (x86)/Windows Kits/10, then in Unity you will likely get "DLLNotFoundException" in the console
  * A work around seems to be downloading the "Windows Software Development Kit (SDK) for Windows 10" found [here](https://developer.microsoft.com/en-us/windows/downloads/windows-10-sdk)
  * This installer will have several "features" to install. It seems like only the last option, "Windows SDK" is needed
  * MSDN blog has some more info on the SDK [here]( https://blogs.msdn.microsoft.com/vcblog/2015/03/03/introducing-the-universal-crt/)
* Driver for the Bluegiga dongle can be found [here](http://www.picaxe.com/downloads/bled112.zip)
* Occaisionally, a hang up occurs and the dongle needs to be removed and then plugged back in. 

# EnfluxSuitController
**Documentation and tutorial updating, check back frequently**

* License subject to change
* All code provided **AS IS** with **NO WARRANTY**
* Enflux Inc. is not responsible for lost work from program crashes. 
* **MAKE SURE ALL WORK IS SAVED BEFORE RUNNING**

## UnitySDK
* Check [releases tab](https://github.com/Enflux/EnfluxSuitController/releases)

## Instructions for setup w/o headset
1. Download EnfluxVR Unity SDK
2. Start or open a Unity project
3. Under the tab "Assets" select  "Import Package > Custom Package"
4. Navigate to folder containing "EnfluxVR.unitypackage" and select package
5. A window will open, select "Import"
6. Under "Project" tab, expand "EnfluxVR > Resources > Prefabs"
7. Drag and drop the following into "Hierarchy"
  * "[EnfluxVR]"
  * "[EnfluxVRHumanoid]"
8. If one is not already in the project, add a "Canvas"
  * From "EnfluxVR > Resources > Prefabs", drag "EnfluxPanel" onto "Canvas"
  * **Dragging "EnfluxPanel" into "Scene" rather than "Hierarchy" may have incorrect result**
9. Move camera as needed

## Instructions for setup w/ HTC Vive headset
1. Obtain and import SteamVR Unity SDK
2. Under "Project" tab, expand "SteamVR"
  * From "SteamVR > Prefabs" drag "[CameraRig]" into "Hierarchy"
  * **Dragging into "Scene" may produce incorrect result"**
3. If not completed already, follow all **Instructions for setup w/o headset**
4. From "EnfluxVR > Resources > Prefabs" drag "SteamVRAdapter" into "Hierarchy"
5. In "Hierarchy", expand "[EnfluxVRHumanoid]" and "[CameraRig]"
  * Select "[EnfluxVRHumanoid]", then drag "[CameraRig] > "Camera (head)" onto "Hmd" object of "EVRHumanoidLimbMap"
  * Select "[EnfluxVRHumanoid] > EVRUpperLimbMap" and drag "Camera (head)" onto "Hmd" object of "EVRUpperLimbMap"
  * Select "[EnfluxVRHumanoid] > EVRLowerLimbMap" and drag "Camera (head)" onto "Hmd" object of "EVRLowerLimbMap"
6. In "Hierarchy" expand "[EnfluxVRHumanoid] > humanoid > Armature > "
7. In "Hierarchy" select "SteamVRAdapter"
  * drag "[EnfluxVRHumanoid] > humanoid > Armature > waist " onto "waist" object of "SteamVRAdapter"
  * drag "[CameraRig] > Camera (head)" onto "Hmd" object of "SteamVRAdapter"

## Instructions for use
1. Insert Bluegiga BLED112 dongle in a USB port on Windows PC
2. Obtain and install drivers for BLED112 [link](http://www.picaxe.com/downloads/bled112.zip)
3. Run Unity project
4. Under "COM Ports" observe "Bluegiga Bluetooth Low Energy (COMX)"
5. Select "Attach" and turn on EnfluxVR modules
6. Under "Devices" observe Enflux modules and select "Connect"
7. If first time using suit or change in environment, select "Run Calibration"
  * For about 30 seconds, move around a lot then select "Finish Calibration"
  * Easiest to calibrate BEFORE putting on the suit. 
    * Just try to get in several rotations that are very different from each other. 
    * This is calibrating the magnetometers for the environment in which you are using the suit
8. Select "Start Animate Mode" 
  * Timer will start counting down, this is time to get prepared for suit to start animating
  * May pause at "3" while connecting to suit
9. When countdown has completed, suit should be animating
10. To stop, select "Stop Animate Mode" 
  * If done using suit, select "Disconnect" **CLICKING DISCONNECT BEFORE STOPPING THE GAME IS VERY IMPORTANT**
