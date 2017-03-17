using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSwing : MonoBehaviour {


	bool isLeft;
	float anglePerClick=30;
	float angleMinusPerTurn=10;


	float Z_Rotate=30; //左右摆的最大度数

	float per_MinusRadius = 5; //每次摆动减少的度数

	Vector3 rotate_temp;

	float flag = 0; //判断的标志，为0时向左摆 rotate_temp.z--，为1时向右摆 rotate_temp.z++

	int counter = 0;

	// Use this for initialization
	void Start () {
		
		rotate_temp=transform.localEulerAngles;
		Debug.Log("MoveSwing----"+rotate_temp);
		Z_Rotate=60;
		//Init();
	}
	
	// Update is called once per frame
	void Update () 
	{

		Debug.Log("rotate_temp_z==" + rotate_temp.z + "  Z_Rotate==" + Z_Rotate + "  counter=="+ counter);

			if(flag==0){
				rotate_temp.z--;				
			}
			
		//(Z_Rotate -(per_MinusRadius * counter))
		if(rotate_temp.z == (-Z_Rotate)){
				flag = 1;
			}

			if(flag == 1){
				rotate_temp.z++;
			}
				
	       	if (rotate_temp.z == Z_Rotate) 
		 	{
				flag = 0;
				counter++;
				
			}

			transform.localEulerAngles=rotate_temp;

		
		
	}
}
