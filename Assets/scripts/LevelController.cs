using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {
	[SerializeField] Text rotText; 
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

        Quaternion rot = transform.localRotation;
        rot.x = rot.x + v * 0.002f;
        rot.z = rot.z - h * 0.002f;

        transform.localRotation = rot;

		rotText.text = "x = " + rot.x + "\n" +
			           "z = " + rot.z; 
    }
}
