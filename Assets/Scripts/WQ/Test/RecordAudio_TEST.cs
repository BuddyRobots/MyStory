using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[RequireComponent (typeof(AudioSource))]
public class RecordAudio_TEST : MonoBehaviour {


	private static string[] micArray=null; 
	private AudioClip clip;  

	void Start () {
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
	}


	public void StartRecord()  
	{  
		GetComponent<AudioSource>().Stop();  
		if (micArray.Length == 0)  
		{  
			Debug.Log("No Record Device!");  
			return;  
		}  
		GetComponent<AudioSource>().loop = false;  
		GetComponent<AudioSource>().mute = true;  //   ？？？？？？？？-----这里为false的时候能检测到音量大小，但是录音不正常，能同时听到录的声音，如果为true，就不能检测音量大小，但是录音的时候正常
		GetComponent<AudioSource>().clip = Microphone.Start(null, false, 5, 44100);  
		clip = GetComponent<AudioSource>().clip;  
		while (!(Microphone.GetPosition(null)>0)) {  
		}  
		GetComponent<AudioSource>().Play ();  


		Debug.Log("---------StartRecord");  
  
	}  

	public void SaveAndPlay()
	{
		SaveMusic();
		PlayRecord();

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

		Debug.Log("-----PlayRecord");  

	}  

	public  void StopRecord()  
	{  

		if (micArray.Length == 0)  
		{  
			Debug.Log("No Record Device!");  
			return;  
		}  
			
		Microphone.End (null);  
		GetComponent<AudioSource>().Stop(); 



		Manager.recordingDone=true;

		Debug.Log("-------StopRecord");  

	}  
	public void SaveMusic()  
	{  
		Save ("123",clip);  
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
	const int HEADER_SIZE = 44;  
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

}
