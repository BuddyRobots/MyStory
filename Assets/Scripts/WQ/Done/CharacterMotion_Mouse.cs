using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CharacterMotion_Mouse : MonoBehaviour
{
	Animator animator;
	bool move;
	Vector3 prePos;
	float ramX;
	float speed=2f;


	void Start()
	{
		animator = GetComponent<Animator>();
	}


	void Update ()
	{

		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); 
			if (hit.collider!=null) 
			{
				if (hit.collider.tag=="Player") //如果点击了老鼠
				{
					move=true;
					//播放跑步动画   to  do...
//					animator.SetBool("isRunning",true);

					Debug.Log("播放跑步动画");

					prePos=transform.position;//保存下原来的位置
					ramX=Random.Range(-2f,-5f);
					Debug.Log("ramX--"+ramX);
				
				}
			}
		}
		if (move) 
		{
			


			transform.Translate(Vector3.left*Time.deltaTime*speed);

			if (transform.position.x<=(prePos.x+ramX)) 
			{
				//停止跑步动画   to do...

			
				move=false;
//				animator.enabled=false;
				animator.CrossFade("kicking",0.1f);
//				animator.SetBool("isRunning",false);
				Debug.Log("停止跑步动画");

				prePos=transform.position;
			}

		}


	}







}
