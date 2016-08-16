using UnityEngine;
using System.Collections;

public class ReferenceCoord : MonoBehaviour {

    private Transform hmdTrans;
    private Quaternion baseReference = new Quaternion();

	// Use this for initialization
	void Start () {
        hmdTrans = GameObject.Find("SteamVRAdapter").GetComponent<Transform>();
        baseReference = hmdTrans.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.localRotation = Quaternion.Inverse(baseReference) * 
            hmdTrans.localRotation;
	}
}
