using UnityEngine;
using System.Collections;
using System.IO;


public class test_PlayMovie : MonoBehaviour
{
	void Start ()
	{
		string moviePath = Path.Combine(Application.streamingAssetsPath, "Movies/IMG_1120.mov");

		if (File.Exists(moviePath))
			Debug.Log("movie exists.");
		else
			Debug.Log("movie does not exist.");

		Handheld.PlayFullScreenMovie("file://" + moviePath, Color.black, FullScreenMovieControlMode.CancelOnInput);
	}
}