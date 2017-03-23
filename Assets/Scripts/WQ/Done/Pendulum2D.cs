using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum2D : MonoBehaviour {
	public Transform center;
	private float radius;
	[Range(-90,90)]
	public float angle;
	//-----------------------------------------
	private float expectantAngle; //预期的角度,初始为initialAngle,但是会随着摆动一圈就会减少一点,必须大于0
	[Header("每圈减少的度数,大于0减少,小于0增加")]
	public float reducedAngle = 10; //每圈减少的度数,大于0减少,小于0增加
	private float currentAngle; //当前角度,用于摆放
	private List<float> recordPoints; //在上的部分,用插值,在下的时候就按点返回
	[Range(0,1),Header("速度,值在0-1,用于插值算法,越小变化越慢")]
	public float speed = 0.1f; //速度,值在0-1,用于插值算法,越小变化越慢
	[Header("点击变化的角度")]
	public float strength = 20;

	void Start()
	{
		recordPoints = new List<float>();
		expectantAngle = -90;

		center=GameObject.Find("SceneParent/SceneLevel_4(Clone)/Center").transform;
		radius = Vector3.Distance(transform.position, center.position);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//左上点右,右上点左;左下点左,右下点右;都是增加expectantAngle
			//左上点左,右上点右,左下点右,右下点左;都是减少expectantAngle
			bool isLeft = IsLeft();
			bool inLeft = InLeft(temp);
			if (isLeft && forward && !inLeft || !isLeft && forward && inLeft || isLeft && !forward && inLeft || !isLeft && !forward && !inLeft)
			{
				//此时增加expectantAngle
				float initialAngle = Mathf.Clamp(Mathf.Abs(currentAngle) + strength, 0, 90);
				expectantAngle = isLeft ? -initialAngle : initialAngle; //如果是左边为负,右边为正
				Caculate(expectantAngle, speed, recordPoints);
				//定位index
				for (int i = 1; i < recordPoints.Count; i++)
				{
					if (Mathf.Abs(currentAngle) < Mathf.Abs(recordPoints[i]) && Mathf.Abs(currentAngle) >= Mathf.Abs(recordPoints[i - 1]))
					{
						index = i + (forward ? 1 : -1);  // 如果是向上,向上一步;向下,向下一步
						break;
					}
				}
			}
			else
			{
				//此时减少expectantAngle
				float initialAngle = Mathf.Abs(expectantAngle) - strength;
				bool isMinus = expectantAngle * -1 <= 0; //算出之前的expectantAngle是否为负数
				if (initialAngle < 0)
				{
					forward = !forward;
					expectantAngle = !isMinus ? -(Mathf.Abs(initialAngle) + strength) : Mathf.Abs(initialAngle) + strength;
					Caculate(expectantAngle, speed, recordPoints);
					//定位index
					for (int i = 1; i < recordPoints.Count; i++)
					{
						if (Mathf.Abs(currentAngle) < Mathf.Abs(recordPoints[i]) && Mathf.Abs(currentAngle) >= Mathf.Abs(recordPoints[i - 1]))
						{
							index = i + (forward ? 1 : -1);  // 如果是向上,向上一步;向下,向下一步
							break;
						}
					}
				}
				else
				{
					expectantAngle = currentAngle;
					Caculate(expectantAngle, speed, recordPoints);  //从当前角度算起减缓趋势
					index = recordPoints.Count - 1;
				}
			}
		}
	}
	int index = 0; //用于记录recordPoints的点
	bool forward = true; //true的时候从0开始(上升),false,开始下落
	bool isCross = false; //用于做穿过检查----此时需要重新计算recordPoints
	void FixedUpdate()
	{
		if (index < 0 || index >= recordPoints.Count) return; //如果index异常了,不做处理
		if (isCross)  //此处注意:如果不执行最下边的逻辑,可能会被卡死在index = 0上
		{
			bool isMinus = expectantAngle * -1 >= 0;  //之前是否是负数
			expectantAngle = Mathf.Abs(expectantAngle) - reducedAngle;
			if (expectantAngle <= 0)
			{
				//说明之后不用再摆动了
				recordPoints.Clear();
				isCross = false;
				return;
			}
			else
			{
				expectantAngle = isMinus ? expectantAngle : -expectantAngle;
				Caculate(expectantAngle, speed, recordPoints);
				forward = true;
				index = 0;
				isCross = false;
			}
		}
		currentAngle = recordPoints[index]; //记录当前角度
		if (forward)
		{
			Place(recordPoints[index]);
			index++;
			if (index == recordPoints.Count)
			{
				forward = false;
				index--;
			}
		}
		else
		{
			Place(recordPoints[index]);
			index--;
			if (index == -1)
			{
				forward = true;
				index++;
				isCross = true; //打开,让上半部分逻辑能走通
			}
		}
	}

	/// <summary>
	/// 用于计算从0到expect的一系列插值,point来存储;序号0-对应角度从0-expect
	/// </summary>
	/// <param name="expect">预期角度</param>
	/// <param name="t">插值</param>
	void Caculate(float expect, float t, List<float>point)
	{
		point.Clear();
		float temp = 0;
		point.Add(temp);
		for (int i = 0; i < 50 && Mathf.Abs(temp - expect) > 0.1f; i++)  //限制次数不超过60,如果t过高,就会变快
		{
			temp = Mathf.Lerp(temp, expect, t);
			point.Add(temp);
		}
	}

	/// <summary>
	/// 给角度,正确的摆放,从-90到90
	/// </summary>
	/// <param name="_angle"></param>
	void Place(float _angle)
	{
		_angle = Mathf.Clamp(_angle, -90, 90);
		transform.rotation = Quaternion.Euler(0, 0, _angle);
		Vector3 offset = new Vector3(radius * Mathf.Sin(_angle * Mathf.Deg2Rad), -radius * Mathf.Cos(_angle * Mathf.Deg2Rad), 0);
		transform.position = center.position + offset;
	}
	/// <summary>
	/// 判断当前方向是左还是右
	/// </summary>
	/// <returns></returns>
	bool IsLeft()
	{
		return transform.rotation.z <= 0;
	}
	/// <summary>
	/// point是否在物体的左边
	/// </summary>
	/// <param name="point"></param>
	/// <returns></returns>
	bool InLeft(Vector3 point)
	{
		return Vector3.Dot(Vector3.Cross(point - transform.position, transform.up * -1), Vector3.forward) > 0;
	}
	void Test()
	{
		recordPoints = new List<float>();
		expectantAngle = -90;
		Caculate(expectantAngle, speed, recordPoints);
		string temp = "";
		foreach (var item in recordPoints)
		{
			temp += item + ":";
		}
		print(temp);
	}
}
