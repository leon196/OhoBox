using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class Root : MonoBehaviour {

	private List<Branch> branches;
	private List<GameObject> wormShits;
	private List<GameObject> flowers;
	private Camera camera;
	private GameObject planeFragment;

	// inputs
	private bool firedLeaves = false;
	private bool firedBranches = false;
	private bool firedReset = false;

	void Start () 
	{
		branches = new List<Branch>();
		wormShits = new List<GameObject>();
		flowers = new List<GameObject>();

		camera = Camera.main;

		planeFragment = Instantiate(ResourceManager.Instance.Fragment) as GameObject;
		//planeFragment.renderer.enabled = false;

		AddRoot();
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		if (Arduino.Manager.detected) {
			Arduino.Manager.Update();
		}
		Shader.SetGlobalFloat("timeElapsed", Time.time);

		for (int i = 0; i < branches.Count; ++i)
		{
			Branch branch = branches[i];
			if (branch.GetTotalLength() < 100f) {
				branch.Update();

				for (int w = 0; w < wormShits.Count; ++w) {
					GameObject wormShit = wormShits[w];
					float dist = Vector3.Distance(branch.GetLastPosition(), wormShit.transform.position);
					if (dist < 5f) {
						Destroy(wormShit);
						wormShits.RemoveAt(w);
						GameObject flower = Instantiate(ResourceManager.Instance.Flower, branch.GetLastPosition() + Vector3.up, Quaternion.Euler(60f, Random.Range(0f, 360f), 270f)) as GameObject;
						flower.renderer.material.shader = Shader.Find("Leaf");
						flower.renderer.material.SetFloat("timeOffset", Random.Range(0f, 1f));
						flower.renderer.material.SetFloat("timeStart", Time.time);
						flowers.Add(flower);
						break;
					}
				}
			}
		}

		if (Arduino.Manager.Enable)
		{
			// Branches
			if (Arduino.Manager.Button(1)) {
				if (!firedBranches) {
					AddBranches();
					firedBranches = true;
				}
			} else {
				firedBranches = false;
			}

			// Leaves
			if (Arduino.Manager.Button(2)) {
				if (!firedLeaves) {
					AddLeaves();
					firedLeaves = true;
				}
			} else {
				firedLeaves = false;
			}

			// Shader
			planeFragment.renderer.enabled = Arduino.Manager.Switch(2);
			for (int i = 1 ; i <= 3 ; ++i )
				planeFragment.renderer.material.SetFloat("slider" + i, Arduino.Manager.Slider(i));
			for (int i = 1 ; i <= 3 ; ++i )
				planeFragment.renderer.material.SetFloat("spiner" + i, Arduino.Manager.Spiner(i));

			/*if (Arduino.Manager.Switch(2) == false) 
			{
				// Zoom
				camera.orthographicSize = 20f + (1f - Arduino.Manager.Slider(1)) * 60f;

				// Pan
				if (Arduino.Manager.Switch(3)) {
					float x = (Arduino.Manager.Spiner(3) - 0.5f) * 100f;
					float z = (Arduino.Manager.Slider(2) - 0.5f) * 100f;

					float scale = 60f / camera.orthographicSize;
					x *= scale;
					z *= scale;

					camera.transform.position = new Vector3(x, camera.transform.position.y, z);
				}
			}*/

			// Reset
			if (Arduino.Manager.Button(3)) {
				if (!firedReset) {
					Reset();
					firedReset = true;
				}
			} else {
				firedReset = false;
			}
		} 
		else 
		{
			if (Input.GetMouseButtonDown(0)) AddBranches();
			else if (Input.GetKeyDown(KeyCode.R)) Reset();
			else if (Input.GetKeyDown(KeyCode.L)) AddLeaves();
		}
	}

	void AddRoot ()
	{
		branches.Add(new Branch(new Vector3()));
		branches.Add(new Branch(new Vector3()));

		int flowerCount = 3 + Random.Range(0, 3);
		for (int i = 0; i < flowerCount; ++i) {
			Vector3 randPos = new Vector3(Random.Range(-100f, 100f), 0f, Random.Range(-100f, 100f));
			GameObject wormShit = Instantiate(ResourceManager.Instance.WormShit, randPos, Quaternion.identity) as GameObject;
			wormShit.renderer.material.shader = Shader.Find("WormShit");
			wormShit.renderer.material.SetFloat("timeOffset", Random.Range(0f, 1f));
			wormShit.renderer.material.SetFloat("timeStart", Time.time);
			wormShits.Add(wormShit);
		}
	}

	void AddBranches ()
	{
		branches.Add(new Branch(new Vector3()));
		int count = branches.Count;
		if (count < 500)
		{
			for (int i = 0; i < count; ++i)
			{
				Branch branch = branches[i];
				branches.Add(new Branch(branch.GetLastPosition()));
			}
		}
	}

	void AddLeaves ()
	{
		int count = branches.Count;
		if (count < 500)
		{
			for (int i = 0; i < count; ++i)
			{
				Branch branch = branches[i];
				branch.AddLeaf();
			}
		}
	}

	void Reset ()
	{
		for (int i = 0; i < branches.Count; ++i)
		{
			Branch branch = branches[i];
			branch.Clean();
			Destroy(branch.gameObject);
		}
		branches = new List<Branch>();

		for (int i = 0; i < wormShits.Count; ++i)
		{
			Destroy(wormShits[i]);
		}
		wormShits = new List<GameObject>();

		for (int i = 0; i < flowers.Count; ++i)
		{
			Destroy(flowers[i]);
		}
		flowers = new List<GameObject>();

		AddRoot();

		//camera.transform.position = new Vector3(0f, camera.transform.position.y, 0f);
		//camera.orthographicSize = 80f;
	}
}
