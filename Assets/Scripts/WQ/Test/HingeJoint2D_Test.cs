using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeJoint2D_Test : MonoBehaviour {

	// Use this for initialization
    public GameObject t1;

    public float t;

    public float x1;

    public float y1;

    public float x2;

    public float t2;

    public bool leftbuff;

   // public float y1;
	void Start () {
        leftbuff = true;


       print( Vector3.Distance(t1.transform.position,this.transform.position));
       t = Vector3.Distance(t1.transform.position, this.transform.position);

       x1 = Mathf.Abs(t1.transform.position.x - this.transform.position.x);
       x2 = x1 * x1;
       t2 = t * t;
 
      // y1 = t1.transform.position.y - this.transform.position.y;



	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A)&&leftbuff) 
        {
           
                print("1");
               
                    Vector3 temp1 = new Vector3(this.transform.position.x + 0.01f, this.transform.position.y, this.transform.position.z);
                    this.transform.position = temp1;
               
                if (this.transform.position.x > 1f)
                {
                  
                    this.transform.position = new Vector3(1, 0, 0);
                    leftbuff = false;
                }
                
                // this.transform.position.x += 0.01f;
                y1 = Mathf.Abs((t2 * t2) - (x2 * x2));
                y1 = Mathf.Sqrt(y1);
             
                Vector3 temp = new Vector3(this.transform.position.x, -y1, this.transform.position.z);
                this.transform.position = temp;


                x1 = Mathf.Abs(t1.transform.position.x - this.transform.position.x);
                x2 = x1 * x1;
            

          
         // t1.transform.position.y = ;
        
        }


        if (Input.GetKey(KeyCode.S) && leftbuff)
        {

            print("1");

            Vector3 temp1 = new Vector3(this.transform.position.x - 0.01f, this.transform.position.y, this.transform.position.z);
            this.transform.position = temp1;

            if (this.transform.position.x < -1f)
            {

                this.transform.position = new Vector3(-1, 0, 0);
                leftbuff = false;
            }

            // this.transform.position.x += 0.01f;
            y1 = Mathf.Abs((t2 * t2) - (x2 * x2));
            y1 = Mathf.Sqrt(y1);

            Vector3 temp = new Vector3(this.transform.position.x, -y1, this.transform.position.z);
            this.transform.position = temp;


            x1 = Mathf.Abs(t1.transform.position.x - this.transform.position.x);
            x2 = x1 * x1;



            // t1.transform.position.y = ;

        }


        if (!leftbuff) {
            this.GetComponent<HingeJoint2D>().connectedBody = t1.gameObject.GetComponent<Rigidbody2D>();
        }
//     this.transform.LookAt(t1.transform.position);
//     Vector3 t2= this.transform.eulerAngles;
//     t2.x=0;
//     this.transform.eulerAngles = t2;

	}
}
