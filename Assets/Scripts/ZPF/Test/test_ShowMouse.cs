using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyStory;

public class test_ShowMouse : MonoBehaviour {

	[HideInInspector]
	Mouse mouse;

	public GameObject headQuad;
	public GameObject leftEarQuad;
	public GameObject rightEarQuad;
	public GameObject bodyQuad;
	public GameObject leftArmQuad;
	public GameObject rightArmQuad;
	public GameObject leftLegQuad;
	public GameObject rightLegQuad;
	public GameObject tailQuad;

	// Use this for initialization
	void Start () {
		mouse = Manager._instance.mouse;

		headQuad.GetComponent<Renderer>().material.mainTexture = mouse.head.texture;
		leftEarQuad.GetComponent<Renderer>().material.mainTexture = mouse.leftEar.texture;
		rightEarQuad.GetComponent<Renderer>().material.mainTexture = mouse.rightEar.texture;
		bodyQuad.GetComponent<Renderer>().material.mainTexture = mouse.body.texture;
		leftArmQuad.GetComponent<Renderer>().material.mainTexture = mouse.leftArm.texture;
		rightArmQuad.GetComponent<Renderer>().material.mainTexture = mouse.rightArm.texture;
		leftLegQuad.GetComponent<Renderer>().material.mainTexture = mouse.leftLeg.texture;
		rightLegQuad.GetComponent<Renderer>().material.mainTexture = mouse.rightLeg.texture;
		tailQuad.GetComponent<Renderer>().material.mainTexture = mouse.tail.texture;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
