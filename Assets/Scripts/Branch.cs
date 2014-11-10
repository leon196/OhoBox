using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Branch
{
	public GameObject gameObject;
	private LineRenderer line;
	private List<Vector3> positions;
	private List<GameObject> leaves;

	private float segmentLength = 1f;
	private float totalLength = 0f;
	
	public Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
	private float speed = 4f;
	private float torsadeSpeed = Random.Range(0.5f, 2f);
	private float torsadeRadius = Random.Range(0.2f, 1.0f);

	private GameObject helper;
	private GameObject finalLeaf;

	public Branch (Vector3 position)
	{
		gameObject = new GameObject("Branch");

		line = gameObject.AddComponent<LineRenderer>();
		line.SetWidth(1f, 0.2f);
		line.material = ResourceManager.Instance.Material;

		positions = new List<Vector3>();
		positions.Add(position);
		positions.Add(position);

		leaves = new List<GameObject>();

		line.SetVertexCount(positions.Count);
		UpdateLine();

		finalLeaf = AddLeaf();
	}
	
	public void Update () 
	{

		GrowLine();

		float dist = Vector3.Distance( positions[positions.Count - 2], positions[positions.Count - 1] );
		if (dist > segmentLength) AddSegment();

		UpdateLine();

		finalLeaf.transform.position = GetLastPosition() + Vector3.up;
	}

	void GrowLine ()
	{
		if (Arduino.Manager.Enable) 
		{
			// MANUAL
			float angle = (1f - Arduino.Manager.Spiner(1)) * Mathf.PI * 2f;
			Vector3 dir = direction + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

			if (Arduino.Manager.Switch(1)) {
				float oscillo = Mathf.Cos(Time.time * torsadeSpeed) * torsadeRadius;
				dir += new Vector3(oscillo, 0f, oscillo);
			}

			dir.Normalize();
			// Move last position
			positions[positions.Count-1] += dir * speed * Time.deltaTime;

			//
			speed = 0.1f + Arduino.Manager.Spiner(2) * 20f;
		} 
		else 
		{
			// AUTO
			float oscillo = Mathf.Cos(Time.time * torsadeSpeed) * torsadeRadius;
			Vector3 torsade = new Vector3(oscillo, 0f, oscillo);
			// Move last position
			positions[positions.Count-1] += (direction + torsade) * speed * Time.deltaTime;
		}

	}

	void AddSegment ()
	{
		Vector3 lastPosition = positions[positions.Count - 1];
		positions.Add(lastPosition);
		line.SetVertexCount(positions.Count);

		totalLength += segmentLength;
	}

	void UpdateLine ()
	{
		for (int i = 0; i < positions.Count; ++i)
		{
			line.SetPosition(i, positions[i]);
		}
	}

	public GameObject AddLeaf ()
	{
		Vector3 lastPosition = positions[positions.Count - 1];
		GameObject leaf = GameObject.Instantiate(
			ResourceManager.Instance.Leaf, 
			lastPosition + Vector3.up, 
			Quaternion.Euler(0f, Random.Range(0f, 360f), 0f)) as GameObject;
		Renderer renderer = leaf.GetComponentInChildren<Renderer>();
		renderer.material.shader = Shader.Find("Leaf");
		renderer.material.SetFloat("timeOffset", Random.Range(0f, 1f));
		renderer.material.SetFloat("timeStart", Time.time);
		leaves.Add(leaf);
		return leaf;
	}

	public void Clean ()
	{
		foreach (GameObject leaf in leaves)
		{
			GameObject.Destroy(leaf);
		}
	}

	public Vector3 GetLastPosition ()
	{
		return positions[positions.Count-1];
	}

	public float GetTotalLength()
	{
		return totalLength;
	}
}
