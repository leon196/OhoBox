using UnityEngine;
using System.Collections;

public class ResourceManager
{
	// Singleton
	private static ResourceManager _instance = null;
	private ResourceManager(){}
	public static ResourceManager Instance {
		get {
			if (_instance == null) _instance = new ResourceManager();
			return _instance;
		}
	}

	// Material
	private Material _material = null;
	public Material Material {
		get {
			if (_material == null) _material = Resources.Load("Material") as Material;
			return _material;
		}
	}

	// Leaf
	private GameObject _leaf = null;
	public GameObject Leaf {
		get {
			if (_leaf == null) _leaf = Resources.Load("Leaf") as GameObject;
			return _leaf;
		}
	}

	// Fragment plane
	private GameObject _plane = null;
	public GameObject Fragment {
		get {
			if (_plane == null) _plane = Resources.Load("Fragment") as GameObject;
			return _plane;
		}
	}

	// Worm Shit
	private GameObject _wormShit = null;
	public GameObject WormShit {
		get {
			if (_wormShit == null) _wormShit = Resources.Load("WormShit") as GameObject;
			return _wormShit;
		}
	}

	// Flower
	private GameObject _flower = null;
	public GameObject Flower {
		get {
			if (_flower == null) _flower = Resources.Load("Flower") as GameObject;
			return _flower;
		}
	}
}
