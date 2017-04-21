using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenCVForUnity;


namespace MyStory
{
	public class Segmentation
	{
		[DllImport("__Internal")]
		private static extern int dll_Segment(
			[In]  IntPtr inputImageArray, int channel, int width, int height, int klass,
			[In, Out] IntPtr segmentatonResultArray
		);
			
		public static Mat Segment(Mat sourceImage, out List<Texture2D> partList, out List<OpenCVForUnity.Rect> bbList)
		{
			partList = new List<Texture2D>();
			bbList = new List<OpenCVForUnity.Rect>();

			Mat modelSizeImage = CropMatToModelSize(sourceImage);

			// Float or Byte?
			float[] inputImageArray = MatToTensorArrayByte(modelSizeImage);
			float[] segmentationResultArray;

			Call_dll_Segment(inputImageArray, out segmentationResultArray);

			Mat modelMaskImage = GenerateMask(segmentationResultArray);
			Mat originMaskImage = new Mat(sourceImage.size(), CvType.CV_8UC1);
			Imgproc.resize(modelMaskImage, originMaskImage, originMaskImage.size(), 0, 0, Imgproc.INTER_NEAREST);

			FillHoles(originMaskImage);

			GetLists(sourceImage, originMaskImage, out partList, out bbList);

			return modelSizeImage;
		}

		// TODO Test returning Mat, change to return void when public
		public static Mat Segment(Texture2D sourceTex, Texture2D resultMaskTex, out List<Texture2D> partList, out List<OpenCVForUnity.Rect> bbList)
		{
			Mat sourceImage = new Mat(sourceTex.height, sourceTex.width, CvType.CV_8UC3);
			Utils.texture2DToMat(sourceTex, sourceImage);
			Mat resultMaskImage = new Mat(resultMaskTex.height, resultMaskTex.width, CvType.CV_8UC1);
			Utils.texture2DToMat(resultMaskTex, resultMaskImage);

			partList = new List<Texture2D>();
			bbList = new List<OpenCVForUnity.Rect>();

			Mat modelSizeImage = CropMatToModelSize(sourceImage);

			Mat originMaskImage = new Mat(sourceImage.size(), CvType.CV_8UC1);
			Imgproc.resize(resultMaskImage, originMaskImage, originMaskImage.size(), 0, 0, Imgproc.INTER_NEAREST);

			FillHoles(originMaskImage);

			GetLists(sourceImage, originMaskImage, out partList, out bbList);

			return originMaskImage;
		}

		private static Mat CropMatToModelSize(Mat sourceImage)
		{			
			Mat grayImage = MatBGR2Gray(sourceImage);

			Mat points = Mat.zeros(grayImage.size(), grayImage.type());
			Core.findNonZero(grayImage, points);
			OpenCVForUnity.Rect roi = Imgproc.boundingRect(new MatOfPoint(points));
			OpenCVForUnity.Rect bb = new OpenCVForUnity.Rect(
				new Point(Math.Max(roi.tl().x - 50.0, 0),
					      Math.Max(roi.tl().y - 50.0, 0)),
				new Point(Math.Min(roi.br().x + 50.0, sourceImage.cols()),
					      Math.Min(roi.br().y + 50.0, sourceImage.rows())));
			Mat croppedImage = new Mat(sourceImage, bb);

			// Zoom to 224*224
			Mat zoomedImage = ZoomCropped(croppedImage);		

			return zoomedImage;
		}

		private static Mat MatBGR2Gray(Mat sourceImage)
		{
			int THRES_H_MIN = PlayerPrefs.GetInt("THRES_H_MIN");
			int THRES_H_MAX = PlayerPrefs.GetInt("THRES_H_MAX");
			int THRES_S_MIN = PlayerPrefs.GetInt("THRES_S_MIN");
			int THRES_S_MAX = PlayerPrefs.GetInt("THRES_S_MAX");
			int THRES_V_MIN = PlayerPrefs.GetInt("THRES_V_MIN");
			int THRES_V_MAX = PlayerPrefs.GetInt("THRES_V_MAX");

			// BGR to HSV
			Mat hsvImage = new Mat(sourceImage.rows(), sourceImage.cols(), CvType.CV_8UC3);
			Imgproc.cvtColor(sourceImage, hsvImage, Imgproc.COLOR_BGR2HSV);
			// InRange
			Mat grayImage = new Mat(sourceImage.rows(), sourceImage.cols(), CvType.CV_8UC1);
			Core.inRange(hsvImage,
				new Scalar(THRES_H_MIN, THRES_S_MIN, THRES_V_MIN),
				new Scalar(THRES_H_MAX, THRES_S_MAX, THRES_V_MAX),
				grayImage);
			Imgproc.morphologyEx(grayImage, grayImage, Imgproc.MORPH_OPEN,
				Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(7, 7)));
			Imgproc.morphologyEx(grayImage, grayImage, Imgproc.MORPH_CLOSE,
				Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(7, 7)));

			return grayImage;
		}

		private static Mat ZoomCropped(Mat croppedImage)
		{
			int croppedWidth = croppedImage.cols();
			int croppedHeight = croppedImage.rows();

			if (croppedWidth > croppedHeight)
			{
				int topMargin = (croppedWidth - croppedHeight)/2;
				int bottomMargin = topMargin;

				// Needed due to percision loss when /2
				if ((croppedHeight + topMargin*2) != croppedWidth)
					bottomMargin = croppedWidth - croppedHeight - topMargin;

				Core.copyMakeBorder(croppedImage, croppedImage, topMargin, bottomMargin, 0, 0, Core.BORDER_REPLICATE);
			}
			else if (croppedWidth < croppedHeight)
			{
				int leftMargin = (croppedHeight - croppedWidth)/2;
				int rightMargin = leftMargin;

				// Needed due to percision loss when /2
				if ((croppedWidth + leftMargin*2) != croppedHeight)
					rightMargin = croppedHeight - croppedWidth - leftMargin;

				Core.copyMakeBorder(croppedImage, croppedImage, 0, 0, leftMargin, rightMargin, Core.BORDER_REPLICATE);
			}

			Mat scaleImage = new Mat();
			Imgproc.resize(croppedImage, scaleImage, new Size(Constant.MODEL_HEIGHT, Constant.MODEL_WIDTH));

			// Return croppedImage[224*224*3]
			return scaleImage;
		}
	
		// TODO need to simplify this
		// float 0~1
		private static float[] MatToTensorArrayFloat(Mat image)
		{
			byte [] byteArray  = new byte [image.rows()*image.cols()*image.channels()];
			float[] floatArray = new float[image.rows()*image.cols()*image.channels()];

			image.get(0, 0, byteArray);

			for (var i = 0; i < byteArray.Length; i++)
				floatArray[i] = (float)byteArray[i]/255.0f;

			return floatArray;
		}

		// float 0~255
		private static float[] MatToTensorArrayByte(Mat image)
		{
			byte [] byteArray  = new byte [image.rows()*image.cols()*image.channels()];
			float[] floatArray = new float[image.rows()*image.cols()*image.channels()];

			image.get(0, 0, byteArray);

			for (var i = 0; i < byteArray.Length; i++)
				floatArray[i] = (float)byteArray[i];

			return floatArray;
		}

		private static void Call_dll_Segment(float[] inputImageArray, out float[] segmentationResultArray)
		{
			
			segmentationResultArray = new float[Constant.MODEL_WIDTH*Constant.MODEL_HEIGHT*Constant.NUM_OF_CLASS];

			GCHandle inputHandle = GCHandle.Alloc(inputImageArray, GCHandleType.Pinned);
			GCHandle outputHandle = GCHandle.Alloc(segmentationResultArray, GCHandleType.Pinned);

			dll_Segment(inputHandle.AddrOfPinnedObject(), Constant.MODEL_CHANNEL, Constant.MODEL_WIDTH, Constant.MODEL_HEIGHT, Constant.NUM_OF_CLASS,
				        outputHandle.AddrOfPinnedObject());

			inputHandle.Free();
			outputHandle.Free();
		}
	
		private static Mat GenerateMask(float[] segmentationResultArray)
		{
			byte[] maskImageData = new byte[Constant.MODEL_HEIGHT*Constant.MODEL_WIDTH];

			for (var i = 0; i < maskImageData.Length; i++)
			{
				float[] pixel = new float[Constant.NUM_OF_CLASS];
				for (var j = 0; j < pixel.Length; j++)
					pixel[j] = segmentationResultArray[i*Constant.NUM_OF_CLASS + j];
				// klass 0(bg), 1 ~ 9(parts)
				maskImageData[i] = (byte)Softmax(pixel);
			}

			Mat maskImage = new Mat(Constant.MODEL_HEIGHT, Constant.MODEL_WIDTH, CvType.CV_8UC1);
			maskImage.put(0, 0, maskImageData);

			return maskImage;
		}

		private static int Softmax(float[] data)
		{
			int maxIndex = 0;
			float maxValue = 0;

			for (int i = 0; i < data.Length; i++)
				if (data[i] > maxValue)
				{
					maxValue = data[i];
					maxIndex = i;
				}
			return maxIndex;
		}
	
		public static void GetLists(Mat originImage, Mat originMaskImage, out List<Texture2D> partTextureList, out List<OpenCVForUnity.Rect> partBBList)
		{
			int originHeight = originMaskImage.rows();
			int originWidth  = originMaskImage.cols();

			// klass 0(bg), 1 ~ 9(parts)
			byte[] maskImageData = new byte[originHeight*originWidth];
			originMaskImage.get(0, 0, maskImageData);

			List<Mat> partMaskList = new List<Mat>();
			for (var i = 0; i < Constant.NUM_OF_PARTS; i++)
				partMaskList.Add(new Mat(originHeight, originWidth, CvType.CV_8UC1, new Scalar(0)));

			for (var i = 0; i < originHeight; i++)
				for (var j = 0; j < originWidth; j++)
				{
					int part = maskImageData[i*originWidth + j] - 1;
					try{

						if (part == -1) continue;

						part = (int)(part*9/255);
						partMaskList[part].put(i, j, (byte)254);
					}
					catch(Exception ex)
					{
						Debug.Log("Segmentation.cs getLists() Exception: partImageList.count = " + partMaskList.Count);
						Debug.Log("Segmentation.cs getLists() Exception: part = " + part + " " + ex.Message);
						break;
					}
				}

			partBBList = GetROIList(partMaskList);

			Mat originImageAlpha = new Mat();
			Imgproc.cvtColor(originImage, originImageAlpha, Imgproc.COLOR_BGR2BGRA);

			partTextureList = new List<Texture2D>();
			for (var i = 0; i < partMaskList.Count; i++)
			{
				Mat resultImage = new Mat(originHeight, originWidth, CvType.CV_8UC4, new Scalar(0, 0, 0, 0));
				originImageAlpha.copyTo(resultImage, partMaskList[i]);
				Mat croppedImage = new Mat(resultImage, partBBList[i]);
				RemoveBorder(croppedImage);

				Texture2D tmpTex = new Texture2D(croppedImage.width(), croppedImage.height(), TextureFormat.RGBA32, false);
				Utils.matToTexture2D(croppedImage, tmpTex);
				partTextureList.Add(tmpTex);
			}
		}

		// TODO Will crash if did not segment parts right (wrong thresholds)
		// Need to add some bug free features.
		private static List<OpenCVForUnity.Rect> GetROIList(List<Mat> partMaskList)
		{
			List<OpenCVForUnity.Rect> roiList = new List<OpenCVForUnity.Rect>();
			for (var i = 0; i < partMaskList.Count; i++)
			{
				// Find Contours
				List<MatOfPoint> contours = new List<MatOfPoint>();
				Mat hierarchy = new Mat();
				Mat mask = partMaskList[i].clone();
				Imgproc.findContours(mask, contours, hierarchy, Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE, new Point(0, 0));

				// Find max contour id
				double maxArea = 0.0;
				int maxIdx = 0;
				for (var j = 0; j < contours.Count; j++)
				{
					double area = Imgproc.contourArea(contours[j]);
					if (area > maxArea)
					{
						maxArea = area;
						maxIdx = j;
					}
				}
				OpenCVForUnity.Rect roi = Imgproc.boundingRect(contours[maxIdx]);
				roiList.Add(roi);
			}
			return roiList;
		}

		// To deal with random black lines on cropped image border.
		private static void RemoveBorder(Mat image)
		{
			if (image.type() != CvType.CV_8UC4)
			{
				Debug.LogError("Segmentation.cs removeBorder() Err: Expect input CvType.CV_8UC4, got " + image.type());
				return;
			}
			byte[] zero = new byte[4] {0, 0, 0, 0};
			for (var i = 0; i < image.rows(); i++)
			{
				image.put(i, 0, zero);
				image.put(i, image.cols() - 1, zero);
			}
			for (var j = 0; j < image.cols(); j++)
			{
				image.put(0, j, zero);
				image.put(image.rows() - 1, j, zero);
			}
		}
	
		private static void FillHoles(Mat originMaskImage)
		{
			Imgproc.morphologyEx(originMaskImage, originMaskImage, Imgproc.MORPH_CLOSE,
				Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(2, 2)));
		}
	}
}