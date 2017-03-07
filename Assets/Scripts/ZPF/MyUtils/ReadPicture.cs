using UnityEngine;
using System.Collections;
using OpenCVForUnity;


namespace MyUtils
{
	// Usage: path = "Pictures/Mouses/1487573118"
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

		public static Mat ReadAsMat(string path)
		{
			Texture2D tex = ReadAsTexture2D(path);
			Mat mat = new Mat(tex.height, tex.width, CvType.CV_8UC3);
			Utils.texture2DToMat(tex, mat);
			return mat;
		}
	}
}