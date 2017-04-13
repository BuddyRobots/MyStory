using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleShow : MonoBehaviour
{

	public static SubtitleShow _instance;

	public bool pause;
	public Text textUI;
	public Util util;
	public float fadeinTime;
	public float fadeoutTime;
	public Image subtitleBg;
	Color subtitleOriginColor;

	void Awake ()
	{
		_instance = this;

	}

	void OnEnable ()
	{
		//      var data = util.Get(1);
		pause = false;
		var data = util.Get (LevelManager.currentLevelData.LevelID);
		StartCoroutine (ShowFromData (data));
	}

	IEnumerator ShowFromData (Util.PassData data)
	{
		

		for (int i = 0; i < data.textdatas.Count; i++) 
		{
			var item = data.textdatas [i];

			textUI.text = item.text; // 文字赋值
			Color color = textUI.color;
			color.a = 0;

			Color bgColor = subtitleBg.color;
			bgColor.a = 1;
			subtitleBg.color = bgColor;

			textUI.color = color;  // 颜色清空
			float colorPerA = 1 / fadeinTime * Time.deltaTime; // 淡入每帧Alpha的变化量
			//准备变色 -- 淡入
			while (color.a <= 1) {
				if (!pause) {
					color.a += colorPerA;
					textUI.color = color;
				}
				yield return null;
			}
			//颜色变为1了,卡住
			yield return new WaitForSeconds (item.time - fadeinTime - fadeoutTime);
			// 准备变色 -- 淡出
			float colorPerM = 1 / fadeoutTime * Time.deltaTime;
			while (color.a >= 0) {
				if (!pause) {
					color.a -= colorPerM;
					textUI.color = color;
					// 如果是最后一句了，且到了淡出的时间，背景也要随之淡出
					if (i == data.textdatas.Count - 1) {
						bgColor.a -= colorPerM;
						subtitleBg.color = bgColor;


					}
				}
				yield return null;
			}

		}
		if (Manager.storyStatus!=StoryStatus.Recording) {
			//字幕切换完毕，屏幕需要变暗再变亮
			FormalScene._instance.screenGrowingDarkAndLight = true;
		}


		Manager._instance.isSubtitleShowOver=true;
		Debug.Log("字幕结束");

	}




}