/*Peter Aguiar
 * DragTiles.cs moves objects in level editor.
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

public class DragTiles : MonoBehaviour {
	public Vector3 spawn_Position;
	
	private GameObject spawn;
	private LevelBuildGui script;
	private bool export_shit = true;
	void OnMouseDrag(){
		if (!script.play) {
			Vector3 locale = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			locale.x = Mathf.Round(locale.x);
			locale.y = Mathf.Round (locale.y);
			locale.z = this.transform.position.z;
			transform.position = locale;
			spawn_Position = locale;
		}
	}
	void OnMouseOver(){
		if (Input.GetMouseButtonDown (2)) {
			Destroy (gameObject);
		}
		if (Input.GetKeyDown(script.Delete)){
			Destroy (gameObject);
		}
	}
	
	void Start(){
		spawn = GameObject.FindWithTag ("GameController");
		script = spawn.GetComponent<LevelBuildGui> ();
		spawn_Position = transform.position;
		transform.position = new Vector3( Mathf.Round (transform.position.x),Mathf.Round(transform.position.y),transform.position.z);


	}
	void export_Fucks(){
		string myName = this.transform.name;
		if (myName == "Wall Tile") {
				
		}
						
	}
	void Update(){
		if(export_shit && script.export_Position){
			export_Fucks();
			export_shit = false;
		}
		if(!script.export_Position){
			export_shit = true;
		}



}
}
