using System;  
using System.Collections.Generic;  
using System.IO;  
using System.Linq;  
using System.Text;  
using UnityEngine;  
using System.Collections;  




/// <summary>
/// 比MicroPhoneInput多了保存音频为WAV的功能
/// </summary>
[RequireComponent (typeof(AudioSource))]  
public class MicroPhoneInputSaveWav : MonoBehaviour   
{  

	private static MicroPhoneInputSaveWav m_instance;  
	  
	public float sensitivity=10000;  
	public float loudness=0;  

	private static string[] micArray=null;  

	const int HEADER_SIZE = 44;  
//	const int RECORD_TIME = 5;  
	private int RECORD_TIME;  

	private AudioClip clip;  
	private string path_1; 




//	public bool isRecording=false;

	public static MicroPhoneInputSaveWav getInstance()  
	{  
		if (m_instance == null)   
		{  
			micArray = Microphone.devices;  
			if (micArray.Length == 0)  
			{  
				Debug.LogError ("Microphone.devices is null");  
			}  
			foreach (string deviceStr in Microphone.devices)  
			{  
				Debug.Log("device name = " + deviceStr);  
			}  
			if(micArray.Length==0)  
			{  
				Debug.LogError("no mic device");  
			}  

			GameObject MicObj=new GameObject("MicObj");  
			m_instance= MicObj.AddComponent<MicroPhoneInputSaveWav>();  
		}  
		return m_instance;  
	}  

	public void StartRecord()  
	{  
		

//		RECORD_TIME=Manager.recordTime;
		RECORD_TIME=LevelManager.currentLevelData.RecordTime;

//		Debug.Log("RECORD_TIME---"+LevelManager.currentLevelData.RecordTime);
		
		GetComponent<AudioSource>().Stop();  
		if (micArray.Length == 0)  
		{  
			Debug.Log("No Record Device!");  
			return;  
		}  
		GetComponent<AudioSource>().loop = false;  
		GetComponent<AudioSource>().mute = true;  //   ？？？？？？？？-----这里为false的时候能检测到音量大小，但是录音不正常，能同时听到录的声音，如果为true，就不能检测音量大小，但是录音的时候正常
		GetComponent<AudioSource>().clip = Microphone.Start(null, false, RECORD_TIME, 44100);  
		clip = GetComponent<AudioSource>().clip;  
		while (!(Microphone.GetPosition(null)>0)) {  
		}  
		GetComponent<AudioSource>().Play ();  
//		isRecording=true;

		Debug.Log("---------StartRecord");  
		//倒计时  
		StartCoroutine(TimeDown());  
	}  

	public  void StopRecord()  
	{  
		
		if (micArray.Length == 0)  
		{  
			Debug.Log("No Record Device!");  
			return;  
		}  

		//这里不注释掉的话，会进去return掉，后面的语句就不执行了
//		if (!Microphone.IsRecording(null))  
//		{  
//			return;  
//		}  

		Microphone.End (null);  


		GetComponent<AudioSource>().Stop(); 


		Manager.recordingDone=true;
		Debug.Log("-------StopRecord");  

	}  

	public Byte[] GetClipData()  
	{  
		if (GetComponent<AudioSource>().clip == null)  
		{  
			Debug.Log("GetClipData audio.clip is null");  
			return null;   
		}  

		float[] samples = new float[GetComponent<AudioSource>().clip.samples];  

		GetComponent<AudioSource>().clip.GetData(samples, 0);  


		Byte[] outData = new byte[samples.Length * 2];  
		//Int16[] intData = new Int16[samples.Length];  
		//converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]  

		int rescaleFactor = 32767; //to convert float to Int16  

		for (int i = 0; i < samples.Length; i++)  
		{  
			short temshort = (short)(samples[i] * rescaleFactor);  

			Byte[] temdata=System.BitConverter.GetBytes(temshort);  

			outData[i*2]=temdata[0];  
			outData[i*2+1]=temdata[1];  


		}  
		if (outData == null || outData.Length <= 0)  
		{  
			Debug.Log("GetClipData intData is null");  
			return null;   
		}  
		//return intData;  

		return outData;  
	}  


	public void PlayClipData(Int16[] intArr)  
	{  

		string aaastr = intArr.ToString();  
		long  aaalength=aaastr.Length;  
		Debug.LogError("aaalength=" + aaalength);  

		string aaastr1 = Convert.ToString (intArr);  
		aaalength = aaastr1.Length;  
		Debug.LogError("aaalength=" + aaalength);  

		if (intArr.Length == 0)  
		{  
			Debug.Log("get intarr clipdata is null");  
			return;  
		}  
		//从Int16[]到float[]  
		float[] samples = new float[intArr.Length];  
		int rescaleFactor = 32767;  
		for (int i = 0; i < intArr.Length; i++)  
		{  
			samples[i] = (float)intArr[i] / rescaleFactor;  
		}  

		//从float[]到Clip  
		AudioSource audioSource = this.GetComponent<AudioSource>();  
		if (audioSource.clip == null)  
		{  
			audioSource.clip = AudioClip.Create("playRecordClip", intArr.Length, 1, 44100, false, false);  
		}  
		audioSource.clip.SetData(samples, 0);  
		audioSource.mute = false;  
		audioSource.Play();  
	}  


	public void PlayRecord()  
	{  
		if (GetComponent<AudioSource>().clip == null)  
		{  
			Debug.Log("audio.clip=null");  
			return;  
		}  
		GetComponent<AudioSource>().mute = false;  
		GetComponent<AudioSource>().loop = false;  
		GetComponent<AudioSource>().Play ();  
		Debug.Log("PlayRecord");  

	}  

	//获取音量大小
	public float GetSoundVolume()
	{
		return  GetAveragedVolume () * sensitivity; 
	}


	public  float GetAveragedVolume()  
	{  
		float[] data=new float[256];  
		float a=0;  
		GetComponent<AudioSource>().GetOutputData(data,0);  
		foreach(float s in data)  
		{  
			a+=Mathf.Abs(s);  
		}  
		return a/256;  
	}  

	// Update is called once per frame  
	void Update ()  
	{  
		
//		loudness = GetAveragedVolume ()* sensitivity;  
//		if (loudness>0) {
//			Debug.Log("volume loudness-------"+loudness);
//
//		}
//		if (loudness > 1)   
//		{  
//			Debug.Log("loudness = "+loudness);  
//		}  
	}  

	private IEnumerator TimeDown()  
	{  
//		Debug.Log(" ------------IEnumerator TimeDown()");  

		int time = 0;  
		while (time < RECORD_TIME)  
		{  
			if (!Microphone.IsRecording (null))   
			{ //如果没有录制  
				Debug.Log ("IsRecording false");  
				yield break;  
			}  
//			Debug.Log("yield return new WaitForSeconds "+time);  
			yield return new WaitForSeconds(1);  
			time++;  
		}  
		if (time >= RECORD_TIME)  
		{  
//			Debug.Log("RECORD_TIME is out! stop record!");  
			StopRecord();  
			//停止录屏
//			VideoRecManager._instance.EndRec();

		}  
		yield return 0;  
	}  





	public void SaveMusic()  
	{  
		Save ("123",clip);  
	} 

   public void UnSaveMusic()  
   {  
       clip = null;  
   }


	//保存wav 模式  
	public static bool Save(string filename, AudioClip clip) 
	{  
		if (!filename.ToLower().EndsWith(".wav"))
		{  
			filename += ".wav";  
		}  
		                                  
		string filepath="";

		#if UNITY_EDITOR  
//		filepath = Path.Combine(Application.dataPath, filename);  
	
		filepath =Path.Combine(Path.Combine(Application.dataPath,"Audio"),filename);



		#elif UNITY_IOS  
		//      path_1 = Application.persistentDataPath;  
		filepath = Path.Combine(Application.persistentDataPath, filename);  
		#endif  

		/*
		#if UNITY_STANDALONE_WIN  
		Debug.Log("*****UNITY_STANDALONE_WIN****");
		//string filepath =  filename;  
		filepath = Path.Combine(Application.dataPath, filename);  
		#endif  


		#if UNITY_ANDROID  
		Debug.Log("*****UNITY_ANDROID****");
		filepath = Path.Combine(Application.persistentDataPath, filename);  
		#endif  
		*/

		Debug.Log("filePath-----"+filepath);  

		// Make sure directory exists if user is saving to sub dir.  
		Directory.CreateDirectory(Path.GetDirectoryName(filepath));  

		using (FileStream fileStream = CreateEmpty(filepath)) {  

			ConvertAndWrite(fileStream, clip);  

			WriteHeader(fileStream, clip);  
		}  

		return true; // TODO: return false if there's a failure saving the file  
	}  


	static FileStream CreateEmpty(string filepath) 
	{  
		FileStream fileStream = new FileStream(filepath, FileMode.Create);  
		byte emptyByte = new byte();  

		for(int i = 0; i < HEADER_SIZE; i++) //preparing the header  
		{  
			fileStream.WriteByte(emptyByte);  
		}  

		return fileStream;  
	}  

	static void ConvertAndWrite(FileStream fileStream, AudioClip clip) {  

		float[] samples = new float[clip.samples];  

		clip.GetData(samples, 0);  

		Int16[] intData = new Int16[samples.Length];  
		//converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]  

		Byte[] bytesData = new Byte[samples.Length * 2];  
		//bytesData array is twice the size of  
		//dataSource array because a float converted in Int16 is 2 bytes.  

		int rescaleFactor = 32767; //to convert float to Int16  

		for (int i = 0; i<samples.Length; i++) {  
			intData[i] = (short) (samples[i] * rescaleFactor);  
			Byte[] byteArr = new Byte[2];  
			byteArr = BitConverter.GetBytes(intData[i]);  
			byteArr.CopyTo(bytesData, i * 2);  
		}  

		fileStream.Write(bytesData, 0, bytesData.Length);  
	}  


	static void WriteHeader(FileStream fileStream, AudioClip clip) {  

		int hz = clip.frequency;  
		int channels = clip.channels;  
		int samples = clip.samples;  

		fileStream.Seek(0, SeekOrigin.Begin);  

		Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");  
		fileStream.Write(riff, 0, 4);  

		Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);  
		fileStream.Write(chunkSize, 0, 4);  

		Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");  
		fileStream.Write(wave, 0, 4);  

		Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");  
		fileStream.Write(fmt, 0, 4);  

		Byte[] subChunk1 = BitConverter.GetBytes(16);  
		fileStream.Write(subChunk1, 0, 4);  

		UInt16 two = 2;  
		UInt16 one = 1;  

		Byte[] audioFormat = BitConverter.GetBytes(one);  
		fileStream.Write(audioFormat, 0, 2);  

		Byte[] numChannels = BitConverter.GetBytes(channels);  
		fileStream.Write(numChannels, 0, 2);  

		Byte[] sampleRate = BitConverter.GetBytes(hz);  
		fileStream.Write(sampleRate, 0, 4);  

		Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2  
		fileStream.Write(byteRate, 0, 4);  

		UInt16 blockAlign = (ushort) (channels * 2);  
		fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);  

		UInt16 bps = 16;  
		Byte[] bitsPerSample = BitConverter.GetBytes(bps);  
		fileStream.Write(bitsPerSample, 0, 2);  

		Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");  
		fileStream.Write(datastring, 0, 4);  

		Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);  
		fileStream.Write(subChunk2, 0, 4);  

		//      fileStream.Close();  
	}  
}  
