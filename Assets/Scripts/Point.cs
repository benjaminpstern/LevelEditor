using UnityEngine;
using System.Collections;

public class Point : MonoBehaviour {
	public Drag parent;
	private GameObject spawn;
	private LevelBuildGui script;
	void Start(){
		spawn = GameObject.FindWithTag ("GameController");
		script = spawn.GetComponent<LevelBuildGui> ();
		parent = script.selectedObject;
	}
}
