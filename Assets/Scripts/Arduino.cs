using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Globalization;
using UnityEngine;

public class Arduino 
{
	public bool detected = false;
	public bool enabled = true;
	private SerialPort stream;
	private StreamReader IN;
	private String[] input;

	// Singleton
	private static Arduino _singleton = null;
	private Arduino(){}
	public static Arduino Manager {
		get {
			if (_singleton == null) 
			{
				_singleton = new Arduino();
				_singleton.Setup();
			}
			return _singleton;
		}
	}

	public bool Enable {
		get { return detected && enabled; }
	}

	public void Setup ()
	{
		foreach (String portName in SerialPort.GetPortNames()) {
			stream = new SerialPort(portName, 9600);
			detected = true;
			Debug.Log("Detected Arduino at : " + portName);
			break;
		}
		if (detected) {
			stream.Open();
			IN = new StreamReader(stream.BaseStream);
			IN.ReadLine();
		}
	}

	public void Update()
	{
		// Read arduino serial
		input = IN.ReadLine().Split(',');
	}

	// Parse string input "3 buttons, 3 switches, 2 sliders, 3 spiners"

	// [1, 2, 3]
	public bool Button (int number)
	{
		return int.Parse(input[number-1]) == 1;
	}

	// [1, 2, 3]
	public bool Switch (int number)
	{
		return int.Parse(input[3 + number-1]) == 1;
	}

	// [1, 2]
	public float Slider (int number)
	{
		return int.Parse(input[6 + number-1]) / 100f;
	}

	// [1, 2, 3]
	public float Spiner (int number)
	{
		return int.Parse(input[8 + number-1]) / 100f;
	}
}