using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System;
using Newtonsoft.Json;
using System.Text;

public class Client : MonoBehaviour
{
	//Instance
	public static Client instance;

	//TcpClient için
	public TcpClient socket;
	public NetworkStream stream;
	public byte[] buffer;
	public int dataBufferSize = 4096;
	public int id;
	public string playerName;


	public InputField inputUserName;

	public void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(this.gameObject);
	}
	public void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void ServerConnect()
	{
		if (inputUserName.text != "")
		{
			//Connect
			if (socket == null)
			{

				socket = new TcpClient();
				socket.ReceiveBufferSize = dataBufferSize;
				socket.SendBufferSize = dataBufferSize;
				try
				{
					socket.BeginConnect(ServerSettings.Host, ServerSettings.Port, ConnectCallBack, null);

				}
				catch (System.Exception)
				{

					Debug.Log($"Baðlantýda hata var!...");
				}

			}
		}
	}

	public void ConnectCallBack(IAsyncResult ar)
	{
		socket.EndConnect(ar);
		if (!socket.Connected)
			return;

		Debug.Log("Baðlantý baþarýlý!...");

		stream = socket.GetStream();
		buffer = new byte[dataBufferSize];
		stream.BeginRead(buffer, 0, dataBufferSize, ReceiveCallBack, null);
	}

	public void ReceiveCallBack(IAsyncResult ar)
	{
		try
		{
			int receiveDataLength = stream.EndRead(ar);
			if (receiveDataLength <= 0)
			{
				ServerDisconnect();
				return;
			}

			byte[] _data = new byte[receiveDataLength];
			Array.Copy(buffer, _data, receiveDataLength);

			string json = Encoding.UTF8.GetString(_data);
			//gelen ver handle edilecek.
			Debug.Log($"Gelen veri : {json}");
			stream.BeginRead(buffer, 0, dataBufferSize, ReceiveCallBack, null);

		}
		catch (Exception)
		{
			//disconnect
			//disconnect
			ServerDisconnect();
			Debug.Log("Oyun patladý...");
			return;


		}

	}
	public void SendDataFromJson(string _json)
	{
		byte[] _data = Encoding.UTF8.GetBytes(_json);
		try
		{
			stream.BeginWrite(_data, 0, _data.Length, SendCallBack, _json);
		}
		catch (Exception)
		{
			ServerDisconnect();
		}
	}

	public void SendCallBack(IAsyncResult ar)
	{
		stream.EndWrite(ar);
		Debug.Log($"Giden veri: {(string)ar.AsyncState}");
	}

	public void ServerDisconnect()
	{

	}
}
