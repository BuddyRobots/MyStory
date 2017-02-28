using UnityEngine;
using System.Collections;

namespace MyStory
{
	public class Constant
	{
		// Segmentation.cs : Image threshold
		public const int THRES_H_MIN = 0;
		public const int THRES_H_MAX = 180;
		public const int THRES_S_MIN = 5;
		public const int THRES_S_MAX = 255;
		public const int THRES_V_MIN = 0;
		public const int THRES_V_MAX = 255;

		public const int MODEL_WIDTH   = 224;
		public const int MODEL_HEIGHT  = 224;
		public const int MODEL_CHANNEL = 3;
		public const int NUM_OF_CLASS  = 10;
		public const int NUM_OF_PARTS  = 9;
	}
}