using UnityEngine;
using UnityEngine.SceneManagement;
using OpenCVForUnity;
using MyStory;

public class ButtonClicked : MonoBehaviour {

	public void OnDeveloperModeButtonClicked()
	{
		SceneManager.LoadSceneAsync("Util_HSVThreshold");
	}

	public void OnTakePhotoButtonClicked()
	{			
		Mat sourceMat = GameObject.Find("Binary Image").GetComponent<HSVThreshold>().GetRGBMat();
		Manager._instance.sourceMat = sourceMat;

		// Segmentation
		#if UNITY_IOS && !UNITY_EDITOR
		Mouse mouse = new Mouse(sourceMat);
		Manager._instance.mouse = mouse;
		#endif

		SceneManager.LoadSceneAsync("4_ModelAnimationShow");
	}
}