using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShaderManager : MonoBehaviour {

	private Renderer renderer;
	private string[] shaderNames;
	private Shader[] shaders;
	private int shaderSelected;

	private string[] textureNames;
	private Texture[] textures;
	private int textureSelected;

	// Use this for initialization
	void Start () {
		shaderNames = new string[] { "Hihi", "PixelLod" };
		shaders = new Shader[shaderNames.Length];
		for (int s = 0; s < shaderNames.Length; ++s) {
			shaders[s] = Shader.Find(shaderNames[s]);
		}
		string texturePath = "Textures/";
		textureNames = new string[] { "Simono", "final", "boboris" };
		textures = new Texture[textureNames.Length];
		for (int s = 0; s < textureNames.Length; ++s) {
			textures[s] = Resources.Load(texturePath + textureNames[s]) as Texture;
		}
		renderer = GetComponent<Renderer>();
		shaderSelected = 0;
		textureSelected = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Arduino.Manager.detected) {
			Arduino.Manager.Update();
		}
		Shader.SetGlobalFloat("timeElapsed", Time.time);

		if (Arduino.Manager.Enable) 
		{
			// Prev texture
			if (Arduino.Manager.ButtonPressed(1)) {
				textureSelected = textureSelected == 0 ? textures.Length - 1 : textureSelected - 1;
				renderer.material.mainTexture = textures[textureSelected];
			} 
			// Next shader
			else if (Arduino.Manager.ButtonPressed(2)) {
				shaderSelected = (shaderSelected + 1) % shaders.Length;
				renderer.material.shader = shaders[shaderSelected];
			}
			// Next texture
			else if (Arduino.Manager.ButtonPressed(3)) {
				textureSelected = (textureSelected + 1) % textures.Length;;
				renderer.material.mainTexture = textures[textureSelected];
			}

			// Upate shader
			for (int i = 1 ; i <= 3 ; ++i )
				renderer.material.SetFloat("slider" + i, Arduino.Manager.Slider(i));
			for (int i = 1 ; i <= 3 ; ++i )
				renderer.material.SetFloat("spiner" + i, Arduino.Manager.Spiner(i));
		}
	}
}
