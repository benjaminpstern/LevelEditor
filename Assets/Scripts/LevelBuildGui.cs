/*Peter Aguiar
 * LevelBuildGui.cs creates the user gui for Level Builder.
 * 
 * Self Explanatory. Notice, however, that there is a bool "play" that tells drag.cs in the game whether or not they are allowed to move. 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuildGui : MonoBehaviour {
	public GameObject player, slow_enemy, pounce_enemy, fast_enemy, vision_tower, 
	ranged_enemy, exit, dead_zone, blowup_mine, push_mine, slow_mine, invisijuice, patrolPoint;

	public KeyCode Delete = KeyCode.H;
	public bool play = false;
	public bool export_Position = false;

	public int menu = 0;
	public string select = "none";

	public Level level;
	public string inputFile, outputFile;

	public GameObject wallTile;
	public GameObject floorTile;
	public Drag selectedObject;
	void Start(){
		level = new Level(inputFile);
		DisplayLevel();
		selectedObject = null;
		//level.write(outputFile);
	}
	void Update(){
		if (Input.GetMouseButtonDown (1)) {
			Object_Select();
		}
		if (Input.GetMouseButtonDown (2) || Input.GetKeyDown (Delete)) {
			select = "none";
			play = false;
			menu =0;
		}if (Input.GetKeyDown (KeyCode.LeftShift)){
			play = true;
			select = "playmode";
			menu = 10;
		}
	}
	void export(){
		level.tiles = new List<List<int>>();
		GameObject[] allTheTiles = GameObject.FindGameObjectsWithTag ("Tile");
		int farthestLeft = 0;
		int farthestRight = 0;
		int farthestDown = 0;
		int farthestUp = 0;
		if (allTheTiles.Length > 0) {
			farthestLeft = Mathf.RoundToInt(allTheTiles[0].transform.position.x);
			farthestRight = Mathf.RoundToInt(allTheTiles[0].transform.position.x);
			farthestUp = Mathf.RoundToInt(allTheTiles[0].transform.position.y);
			farthestDown = Mathf.RoundToInt(allTheTiles[0].transform.position.y);
				}
		for (int i=0; i<allTheTiles.Length; i++) {
			if(allTheTiles[i].transform.position.x < farthestLeft)
				farthestLeft = Mathf.RoundToInt(allTheTiles[i].transform.position.x);
			if(allTheTiles[i].transform.position.x > farthestRight)
				farthestRight = Mathf.RoundToInt(allTheTiles[i].transform.position.x);
			if(allTheTiles[i].transform.position.y < farthestDown)
				farthestDown = Mathf.RoundToInt(allTheTiles[i].transform.position.y);
			if(allTheTiles[i].transform.position.y > farthestUp)
				farthestUp = Mathf.RoundToInt(allTheTiles[i].transform.position.y);
		}
		int width = farthestRight - farthestLeft + 1;
		int height = farthestUp - farthestDown + 1;
		for (int i=0; i<height; i++) {
			level.tiles.Add (new List<int>());
			for(int j=0;j<width;j++){
				level.tiles[i].Add (0);
			}
		}
		for (int i=0; i<allTheTiles.Length; i++) {
			GameObject tile = allTheTiles[i];
			if (tile.name == "Wall"){
				int tilex = Mathf.RoundToInt(tile.transform.position.x-farthestLeft);
				int tiley = Mathf.RoundToInt(tile.transform.position.y-farthestDown);
				level.tiles[tiley][tilex] = 1;
			}
		}
		level.playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		level.exitPosition = GameObject.FindGameObjectWithTag ("Exit").transform.position;
		level.slowEnemyPositions = new List<Vector3> ();
		level.fastEnemyPositions = new List<Vector3> ();
		level.rangedEnemyPositions = new List<Vector3> ();
		level.pounceEnemyPositions = new List<Vector3> ();
		level.towerPositions = new List<Vector3> ();
		level.deadZonePositions = new List<Vector3> ();
		level.blowupMinePositions = new List<Vector3> ();
		level.slowMinePositions = new List<Vector3> ();
		level.pushMinePositions = new List<Vector3> ();
		level.invisijuicePositions = new List<Vector3> ();
		level.slowEnemyPatrol = new List<List<Vector3>>();
		level.fastEnemyPatrol = new List<List<Vector3>>();
		level.rangedEnemyPatrol = new List<List<Vector3>>();
		level.pounceEnemyPatrol = new List<List<Vector3>>();

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i=0; i<enemies.Length; i++) {
			Drag dragscript = enemies[i].GetComponentInChildren<Drag>();
			if(dragscript!=null){
				dragscript.export_Fucks();
			}
		}
		GameObject[] deadZones = GameObject.FindGameObjectsWithTag ("Dead Zone");
		for (int i=0; i<deadZones.Length; i++) {
			level.deadZonePositions.Add(deadZones[i].transform.position);
		}
		GameObject[] blowupMines = GameObject.FindGameObjectsWithTag ("Blowup Mine");
		for (int i=0; i<blowupMines.Length; i++) {
			level.blowupMinePositions.Add(blowupMines[i].transform.position);
		}
		GameObject[] pushMines = GameObject.FindGameObjectsWithTag ("Push Mine");
		for (int i=0; i<pushMines.Length; i++) {
			level.pushMinePositions.Add(pushMines[i].transform.position);
		}
		GameObject[] slowMines = GameObject.FindGameObjectsWithTag ("Slow Mine");
		for (int i=0; i<slowMines.Length; i++) {
			level.slowMinePositions.Add(slowMines[i].transform.position);
		}
		GameObject[] invisijuices = GameObject.FindGameObjectsWithTag ("Invisijuice");
		for (int i=0; i<invisijuices.Length; i++) {
			level.invisijuicePositions.Add(invisijuices[i].transform.position);
		}
		Vector3 transformVector = new Vector3 (farthestLeft, farthestDown, 0);
		level.playerPosition = level.playerPosition - transformVector;
		level.exitPosition = level.exitPosition - transformVector;
		for (int i=0; i<level.slowEnemyPositions.Count; i++) {
			level.slowEnemyPositions[i] = level.slowEnemyPositions[i] - transformVector;
				}
		for (int i=0; i<level.fastEnemyPositions.Count; i++) {
			level.fastEnemyPositions[i] = level.fastEnemyPositions[i] - transformVector;
		}
		for (int i=0; i<level.rangedEnemyPositions.Count; i++) {
			level.rangedEnemyPositions[i] = level.rangedEnemyPositions[i] - transformVector;
		}
		for (int i=0; i<level.pounceEnemyPositions.Count; i++) {
			level.pounceEnemyPositions[i] = level.pounceEnemyPositions[i] - transformVector;
		}
		for (int i=0; i<level.slowEnemyPatrol.Count; i++) {
			for(int j=0;j<level.slowEnemyPatrol[i].Count;j++){
				level.slowEnemyPatrol[i][j] = level.slowEnemyPatrol[i][j] - transformVector;
			}
		}
		for (int i=0; i<level.fastEnemyPatrol.Count; i++) {
			for(int j=0;j<level.fastEnemyPatrol[i].Count;j++){
				level.fastEnemyPatrol[i][j] = level.fastEnemyPatrol[i][j] - transformVector;
			}
		}
		for (int i=0; i<level.rangedEnemyPatrol.Count; i++) {
			for(int j=0;j<level.rangedEnemyPatrol[i].Count;j++){
				level.rangedEnemyPatrol[i][j] = level.rangedEnemyPatrol[i][j] - transformVector;
			}
		}
		for (int i=0; i<level.pounceEnemyPatrol.Count; i++) {
			for(int j=0;j<level.pounceEnemyPatrol[i].Count;j++){
				level.pounceEnemyPatrol[i][j] = level.pounceEnemyPatrol[i][j] - transformVector;
			}
		}
		for (int i=0; i<level.towerPositions.Count; i++) {
			level.towerPositions[i] = level.towerPositions[i] - transformVector;
		}
		for (int i=0; i<level.deadZonePositions.Count; i++) {
			level.deadZonePositions[i] = level.deadZonePositions[i] - transformVector;
		}
		for (int i=0; i<level.blowupMinePositions.Count; i++) {
			level.blowupMinePositions[i] = level.blowupMinePositions[i] - transformVector;
		}
		for (int i=0; i<level.pushMinePositions.Count; i++) {
			level.pushMinePositions[i] = level.pushMinePositions[i] - transformVector;
		}
		for (int i=0; i<level.slowMinePositions.Count; i++) {
			level.slowMinePositions[i] = level.slowMinePositions[i] - transformVector;
		}
		for (int i=0; i<level.invisijuicePositions.Count; i++) {
			level.invisijuicePositions[i] = level.invisijuicePositions[i] - transformVector;
		}
		
		level.write (outputFile);
	}
	public void slowPosition(Vector3 pos){
				level.slowEnemyPositions.Add (pos);
				level.slowEnemyPatrol.Add (new List<Vector3>());
		}
	public void slowPoints(List<GameObject> points){
		for(int i=0;i<points.Count;i++){
			level.slowEnemyPatrol[level.slowEnemyPositions.Count - 1].Add (points[i].transform.position);
		}
	}
	public void fastPosition (Vector3 pos){
				level.fastEnemyPositions.Add (pos);
				level.fastEnemyPatrol.Add (new List<Vector3>());
		}
	public void fastPoints(List<GameObject> points){
		for(int i=0;i<points.Count;i++){
			level.fastEnemyPatrol[level.fastEnemyPositions.Count - 1].Add (points[i].transform.position);
		}
	}
	public void rangedPosition(Vector3 pos){
		level.rangedEnemyPositions.Add (pos);
		level.rangedEnemyPatrol.Add (new List<Vector3>());
	}
	public void rangedPoints(List<GameObject> points){
		for(int i=0;i<points.Count;i++){
			level.rangedEnemyPatrol[level.rangedEnemyPositions.Count - 1].Add (points[i].transform.position);
		}
	}
	public void towerPosition(Vector3 pos){
				level.towerPositions.Add (pos);
		}
	public void pouncePosition(Vector3 pos){
		level.pounceEnemyPositions.Add (pos);
		level.pounceEnemyPatrol.Add (new List<Vector3>());
	}
	public void pouncePoints(List<GameObject> points){
		for(int i=0;i<points.Count;i++){
			level.pounceEnemyPatrol[level.pounceEnemyPositions.Count - 1].Add (points[i].transform.position);
		}
	}
	void DisplayLevel(){
				GameObject tile;
				if (level.tiles != null) {
						for (int y = 0; y < level.tiles.Count; y++) {
								for (int x = 0; x < level.tiles[y].Count; x++) {
										if (level.tiles [y] [x] == 0) {
												tile = Instantiate (floorTile, new Vector3 (x, y, 1), Quaternion.identity) as GameObject;
												tile.transform.name = "Tiles";
										} else if (level.tiles [y] [x] == 1) {
												tile = Instantiate (wallTile, new Vector3 (x, y, 0), Quaternion.identity) as GameObject;
												tile.transform.name = "Wall";
										}
										//more types of tiles coming up, e.g. kill zone
								}
						}
				} else {
						print ("There is no level yet.");		
		}
		GameObject playerObject;
		GameObject exitObject;
		GameObject o;
		GameObject p;
		if (!(level.playerPosition == Vector3.zero && level.exitPosition == Vector3.zero)) {
						playerObject = Instantiate (player, level.playerPosition, Quaternion.identity) as GameObject;
						playerObject.transform.name = "Player";
						exitObject = Instantiate (exit, level.exitPosition, Quaternion.identity)as GameObject;
						exitObject.transform.name = "Exit";
				}
		for(int i=0;i<level.slowEnemyPositions.Count;i++){
			o = Instantiate (slow_enemy, level.slowEnemyPositions[i], Quaternion.identity) as GameObject;
			o.transform.name = "Slow Enemy";
			for(int j=0;j<level.slowEnemyPatrol[i].Count;j++){
				p = Instantiate(patrolPoint,level.slowEnemyPatrol[i][j],Quaternion.identity) as GameObject;
				p.transform.name = "Patrol Point";
				o.GetComponent<Drag>().addPoint(p);
				p.SetActive (false);
			}
		}
		for(int i=0;i<level.fastEnemyPositions.Count;i++){
			o = Instantiate (fast_enemy, level.fastEnemyPositions[i], Quaternion.identity)as GameObject;
			o.transform.name = "Fast Enemy";
			for(int j=0;j<level.fastEnemyPatrol[i].Count;j++){
				p = Instantiate(patrolPoint,level.fastEnemyPatrol[i][j],Quaternion.identity) as GameObject;
				p.transform.name = "Patrol Point";
				o.GetComponent<Drag>().points.Add(p);
				p.SetActive (false);
			}
		}
		for(int i=0;i<level.pounceEnemyPositions.Count;i++){
				o = Instantiate (pounce_enemy, level.pounceEnemyPositions[i], Quaternion.identity)as GameObject;
			o.transform.name = "Pounce Enemy";
			for(int j=0;j<level.pounceEnemyPatrol[i].Count;j++){
				p = Instantiate(patrolPoint,level.pounceEnemyPatrol[i][j],Quaternion.identity) as GameObject;
				p.transform.name = "Patrol Point";
				o.GetComponent<Drag>().points.Add(p);
				p.SetActive (false);
			}
		}
		for(int i=0;i<level.rangedEnemyPositions.Count;i++){
				o = Instantiate (ranged_enemy, level.rangedEnemyPositions[i], Quaternion.identity)as GameObject;
			o.transform.name = "Ranged Enemy";
			for(int j=0;j<level.rangedEnemyPatrol[i].Count;j++){
				p = Instantiate(patrolPoint,level.rangedEnemyPatrol[i][j],Quaternion.identity) as GameObject;
				p.transform.name = "Patrol Point";
				o.GetComponent<Drag>().points.Add(p);
				p.SetActive (false);
			}
		}
		for(int i=0;i<level.towerPositions.Count;i++){
				o = Instantiate (vision_tower, level.towerPositions[i], Quaternion.identity)as GameObject;
			o.transform.name = "Vision Tower";
		}
		for(int i=0;i<level.deadZonePositions.Count;i++){
			o = Instantiate (dead_zone, level.deadZonePositions[i], Quaternion.identity)as GameObject;
			o.transform.name = "Dead Zone";
		}
		for(int i=0;i<level.blowupMinePositions.Count;i++){
			o = Instantiate (blowup_mine, level.blowupMinePositions[i], Quaternion.identity)as GameObject;
			o.transform.name = "Blowup Mine";
		}
		for(int i=0;i<level.pushMinePositions.Count;i++){
			o = Instantiate (push_mine, level.pushMinePositions[i], Quaternion.identity)as GameObject;
			o.transform.name = "Push Mine";
		}
		for(int i=0;i<level.slowMinePositions.Count;i++){
			o = Instantiate (slow_mine, level.slowMinePositions[i], Quaternion.identity)as GameObject;
			o.transform.name = "Slow Mine";
		}
		for(int i=0;i<level.invisijuicePositions.Count;i++){
			o = Instantiate (invisijuice, level.invisijuicePositions[i], Quaternion.identity)as GameObject;
			o.transform.name = "Invisijuice";
		}
	}
	void OnGUI(){
		GUI.Box (new Rect (Screen.width*0.70f, 0, Screen.width/4, Screen.height/25), "SELECTION: "+select);

		if (menu == 0) {
						GUI.Box (new Rect (0, 0, Screen.width / 4, Screen.height), "EDITOR");
						Main_Select ();
				} else if (menu == 1) {
						GUI.Box (new Rect (0, 0, Screen.width / 4, Screen.height), "PLAYER OBJECTS");
						Player_Select ();
				} else if (menu == 2) {
						GUI.Box (new Rect (0, 0, Screen.width / 4, Screen.height), "TERRAIN/OBJECTS");
						Terrain_Select ();
				} else if (menu == 3) {
						GUI.Box (new Rect (0, 0, Screen.width / 4, Screen.height), "ENEMY OBJECTS");
						Enemy_Select ();
				} else if (menu == 4) {
						GUI.Box (new Rect (0, 0, Screen.width / 4, Screen.height), "MINES");
						Mine_Select ();
				} else if (menu == 5) {
						GUI.Box (new Rect (0, 0, Screen.width / 4, Screen.height), "RESOURCES");
						Resource_Select ();
				} else if (select == "playmode") {
						playmode ();
				} else if (menu == 6) {
						export ();
				}
	}
	void Object_Select(){
		Vector3 locale = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		locale.z = 0;
		GameObject oh;
				if (select == "Player") {
			oh = Instantiate (player, (locale), Quaternion.identity)as GameObject;
						
				} else if (select == "Fast Enemy") {
			oh = Instantiate (fast_enemy, locale, Quaternion.identity) as GameObject;
				} else if (select == "Slow Enemy") {
			oh = Instantiate (slow_enemy, (locale), Quaternion.identity)as GameObject;
				} else if (select == "Ranged Enemy") {
			oh = Instantiate (ranged_enemy, (locale), Quaternion.identity)as GameObject;
				} else if (select == "Vision Tower") {
			oh = Instantiate (vision_tower, (locale), Quaternion.identity)as GameObject;
				} else if (select == "Pounce Enemy") {
			oh = Instantiate (pounce_enemy, (locale), Quaternion.identity)as GameObject;
				} else if (select == "Exit") {
			oh = Instantiate (exit, locale, Quaternion.identity)as GameObject;
				} else if (select == "Tiles") {
			oh = Instantiate (floorTile, locale, Quaternion.identity)as GameObject;
				} 
			else if (select == "Dead Zone") {
				oh = Instantiate (dead_zone, locale, Quaternion.identity)as GameObject;
			}
		else if (select == "Blowup Mine") {
			oh = Instantiate (blowup_mine, locale, Quaternion.identity)as GameObject;
		}
		else if (select == "Push Mine") {
			oh = Instantiate (push_mine, locale, Quaternion.identity)as GameObject;
		}
		else if (select == "Slow Mine") {
			oh = Instantiate (slow_mine, locale, Quaternion.identity)as GameObject;
		}
		else if (select == "Invisijuice") {
			oh = Instantiate (invisijuice, locale, Quaternion.identity)as GameObject;
		}else if (select == "Patrol Points") {
			if(selectedObject.tag == "Enemy"){
				oh = Instantiate (patrolPoint, locale, Quaternion.identity)as GameObject;
				oh.GetComponent<Point>().parent = selectedObject;
				selectedObject.GetComponent<Drag>().points.Add (oh);
			}
			else{
				return;
			}
		} else {
			oh = Instantiate (wallTile, locale, Quaternion.identity)as GameObject;
		}
		oh.transform.name = select;	
			
		}
		void playmode(){
		float offset = 0.0f;
		if (GUI.Button (new Rect (10,  10 + offset, Screen.width / 10, Screen.height * 0.05f), "X")) {
			select = "none";
			menu = 0;
			play = false;
			}	
		}
	void Main_Select(){
		float offset = 0.0f;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Player")) {
			menu = 1;
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Objects/Terrain")) {
			menu = 2;
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Enemies")) {
			menu = 3;
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Mines")) {
			menu = 4;
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Resource Preset")) {
			menu = 5;
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Export Level")) {
			menu = 6;
		}
	}
	void Player_Select(){
		if (GUI.Button (new Rect (10,  10 , 40, Screen.height * 0.05f), "X")) {
			menu = 0;
		}
		float offset = 0.0f;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Player")) {
			select = "Player";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Exit")) {
			select = "Exit";
		}
	}
	void Terrain_Select(){if (GUI.Button (new Rect (10,  10 , 40, Screen.height * 0.05f), "X")) {
			menu = 0;
		}
		float offset = 0.0f;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Tiles")) {
			select = "Tiles";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Wall")) {
			select = "Wall";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Dead Zone")) {
			select = "Dead Zone";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
	}
	void Enemy_Select(){
		if (GUI.Button (new Rect (10,  10 , 40, Screen.height * 0.05f), "X")) {
			menu = 0;
		}
		float offset = 0.0f;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Slow Enemy")) {
			select = "Slow Enemy";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Fast Enemy")) {
			select = "Fast Enemy";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Pounce Enemy")) {
			select = "Pounce Enemy";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Ranged Enemy")) {
			select = "Ranged Enemy";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Vision Tower")) {
			select = "Vision Tower";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Patrol Points")) {
			select = "Patrol Points";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Clear Patrol Points")) {
			if(selectedObject.tag == "Enemy"){
				Drag enemy = selectedObject.GetComponent<Drag>();
				for(int i=0;i<enemy.points.Count;i++){
					Destroy(enemy.points[i].gameObject);
				}
				enemy.points.Clear ();
			}
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
	}
	void Mine_Select(){
		if (GUI.Button (new Rect (10,  10 , 40, Screen.height * 0.05f), "X")) {
			menu = 0;
		}
		float offset = 0.0f;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Blowup Mine")) {
			select = "Blowup Mine";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Push Mine")) {
			select = "Push Mine";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Slow Mine")) {
			select = "Slow Mine";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "Invisijuice")) {
			select = "Invisijuice";
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
	}
	void Resource_Select(){
		if (GUI.Button (new Rect (10, 10, 40, Screen.height * 0.05f), "X")) {
			menu = 0;
		}
		float offset = 0.0f;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
		offset += Screen.height * 0.05f + 5;
		if (GUI.Button (new Rect (10, Screen.height / 10 + offset, Screen.width / 4 - 20, Screen.height * 0.05f), "")) {
			
		}
	}
}
