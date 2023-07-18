using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Handlers : MonoBehaviour
{
	public enum Server
	{
		HelloServer = 1
	}
	public enum ClientEnum
	{
		HelloClient = 1
	}

	public static void Handle(string _json)
	{
		Packet mainPacket =JsonUtility.FromJson<Packet>(_json);
		switch (mainPacket.type)
		{
			case (int)Server.HelloServer:
				GetHello(JsonUtility.FromJson<Hello>(_json));
				break;
			default:
				break;

		}
	}
	public static void GetHello(Hello _packet)//sunucu kabul mesajý, ve bizim gerekli birlgilerimiz gönderdiðimiz yer
	{
		
		Client.instance.id= _packet.id;
		Client.instance.SendDataFromJson(JsonUtility.ToJson(CreataHello(Client.instance.id,(int)ClientEnum.HelloClient,Client.instance.playerName)));

	}

	public class Packet
	{
		public int id;
		public int type;
	}

	public class Hello : Packet
	{
		public string name;
		public string message;
	}

	public static Hello CreataHello(int _id, int _type, string _name)
	{
		Hello packet = new Hello();
		packet.id = _id;
		packet.type = _type;
		packet.name = _name;
		return packet;


	}
}
