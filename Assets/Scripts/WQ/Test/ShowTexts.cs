using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTexts : MonoBehaviour {
	public bool start = false;
	public bool pause = false;
	[Header("淡入淡出时间")]
	public float fadeinTime = 0.8f;
	public float fadeoutTime = 1f;
	[Header("要显示的字幕")]
	public List<string> texts;
	[Header("显示文字的UI")]
	public Text textUI;

	void Start()
	{
		StartCoroutine(Show());
	}

	IEnumerator Show()
	{
		foreach (var item in texts)
		{
			textUI.text = item; // 文字赋值
			Color color = textUI.color;
			color.a = 0;
			textUI.color = color;  // 颜色清空
			float colorPerA = 1 / fadeinTime * Time.deltaTime; // 淡入每帧Alpha的变化量
			//准备变色 -- 淡入
			while (color.a <= 1)
			{
				if (!pause)
				{
					color.a += colorPerA;
					textUI.color = color;
					print(color);
				}
				yield return null;
			}

			yield return new WaitForSeconds(3f);
			// 准备变色 -- 淡出
			float colorPerM = 1 / fadeoutTime * Time.deltaTime;
			while (color.a >= 0)
			{
				if (!pause)
				{
					color.a -= colorPerM;
					textUI.color = color;
				}
				yield return null;
			}
		}
	}
}
