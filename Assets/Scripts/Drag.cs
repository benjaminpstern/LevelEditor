/*Peter Aguiar
 * Drag.cs moves objects in level editor.
 * 
 * OnMouseDrag is called when a collider is clicked on, the spawn point Vector3 is changed according to location of mouse. 
 * 
 * When OnMouseOver + middle mouse button is detected, object Destroys itself.
 * 
 * Start looks for Gamecontroller script with LevelBuildGui script, change GameController to whatever the LevelBuildGui script is on.
 * 
 * Update says that if not in play mode, keep object at spawn point. 
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drag : MonoBehaviour {
	public Vector3 spawn_Position;
	public List<GameObject> points;
	private GameObject spawn;
	private LevelBuildGui script;
	void OnMouseDrag(){
		if (!script.play) {
			Vector3 locale = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			locale.z = 0;
			transform.position = locale;
			spawn_Position = locale;
		}
	}
	void OnMouseDown(){
		Drag other = script.selectedObject;
		if(other !=null){
			if(other!=this && other.points!=null){
				for(int i=0;i<other.points.Count;i++){
					other.points[i].SetActive(false);
				}
			}
		}
		
		script.selectedObject = this;
		if(points!=null){
			for(int i=0;i<points.Count;i++){
				points[i].SetActive(true);
			}
		}
		
	}
	void OnMouseOver(){
		if (Input.GetMouseButtonDown (2) || Input.GetKeyDown(script.Delete)) {
			Destroy (gameObject);
		}
	}

	void Start(){
		spawn = GameObject.FindWithTag ("GameController");
		script = spawn.GetComponent<LevelBuildGui> ();
		spawn_Position = transform.position;
	}
	public void export_Fucks(){
		string myName = this.transform.name;
		if (myName == "Pounce Enemy") {
			script.pouncePosition (transform.position);
			script.pouncePoints(this.points);
		} else if (myName == "Slow Enemy") {
			script.slowPosition (transform.position);
			script.slowPoints(this.points);
		}else if (myName == "Fast Enemy") {
			script.fastPosition (transform.position);
			script.fastPoints(this.points);
		}else if (myName == "Ranged Enemy") {
			script.rangedPosition (transform.position);
			script.rangedPoints(this.points);
		}else if (myName == "Vision Tower") {
			script.towerPosition (transform.position);
		}
	}
	public void addPoint(GameObject p){
		points.Add (p);
	}
	void Update(){
		if (!script.play) {
			transform.position = spawn_Position;

		} 
	}
}
