using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class ArduinoManager : MonoBehaviour 
{
	void Start () 
	{
	}

	void Update ()
	{
		if (Arduino.Manager.detected) {
			Arduino.Manager.Update();
		}
		
		Shader.SetGlobalFloat("timeElapsed", Time.time);

		if (Arduino.Manager.Enable) 
		{
			// Upate shader
			for (int i = 1 ; i <= 3 ; ++i )
				renderer.material.SetFloat("slider" + i, Arduino.Manager.Slider(i));
			for (int i = 1 ; i <= 3 ; ++i )
				renderer.material.SetFloat("spiner" + i, Arduino.Manager.Spiner(i));
		}
	}
}