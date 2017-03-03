using UnityEngine;
using System.Collections;
using OpenCVForUnity;


namespace MyUtils
{
	public static class ReadPicture
	{
		public static Texture2D ReadAsTexture2D(string path)
		{
			return Resources.Load(path) as Texture2D;
		}			

		public static Texture ReadAsTexture(string path)
		{
			return Resources.Load(path) as Texture;
		}	
	}
}