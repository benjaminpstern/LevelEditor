using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public class Level{
	
	public List<List<int>> tiles;
	public int width = 50;
	public int height = 50;
	public Vector3 playerPosition;
	public Vector3 exitPosition;
	public List<Vector3> slowEnemyPositions;
	public List<Vector3> fastEnemyPositions;
	public List<Vector3> pounceEnemyPositions;
	public List<Vector3> rangedEnemyPositions;
	public List<Vector3> towerPositions;
	public List<Vector3> deadZonePositions;
	public List<Vector3> pushMinePositions;
	public List<Vector3> blowupMinePositions;
	public List<Vector3> slowMinePositions;
	public List<Vector3> invisijuicePositions;
	public List<List<Vector3>> slowEnemyPatrol;
	public List<List<Vector3>> fastEnemyPatrol;
	public List<List<Vector3>> rangedEnemyPatrol;
	public List<List<Vector3>> pounceEnemyPatrol;
	string [] objectList;
	public Level(string fileName){
		tiles = new List<List<int>>(); // if we need to add new rows we need to initialize a List<int> element and add to tiles
		slowEnemyPositions = new List<Vector3>();
		fastEnemyPositions = new List<Vector3>();
		pounceEnemyPositions = new List<Vector3>();
		rangedEnemyPositions = new List<Vector3>();
		towerPositions = new List<Vector3>();
		deadZonePositions = new List<Vector3>();
		pushMinePositions = new List<Vector3>();
		blowupMinePositions = new List<Vector3>();
		slowMinePositions = new List<Vector3>();
		invisijuicePositions= new List<Vector3>();
		slowEnemyPatrol = new List<List<Vector3>>();
		fastEnemyPatrol = new List<List<Vector3>>();
		rangedEnemyPatrol = new List<List<Vector3>>();
		pounceEnemyPatrol = new List<List<Vector3>>();
		string [] objectList = {"SLOW ENEMY", "FAST ENEMY", "POUNCE ENEMY", "RANGED ENEMY", "TOWER", "DEAD ZONE",
			"PUSH MINE", "BLOWUP MINE", "SLOW MINE", "INVISIJUICE"};
		this.objectList = objectList;
		/*for(int i=0;i<width;i++){
			tiles.Add (new List<int>());
			for (int j = 0; j < height; j++){
				tiles[i].Add (0);
			}
		}*/
		
		if (fileName == ""){
			//setDimensions(100,100);
			return;
		}
		
		TextAsset txt = (TextAsset) Resources.Load ("LevelFiles/" + fileName, typeof (TextAsset));
		string content = txt.text;
		List<string> lines = new List<string> (Regex.Split(content, "\r\n"));
		
		int tileStart = -1;
		int tileEnd = -1;
		
		for (int i = 0; i < lines.Count; i++){
			if (string.Compare(lines[i], "TILES") == 0){
				tileStart = i;
			}
		}
		for (int i = tileStart+1; i < lines.Count; i++){
			if (lines[i].Length == 0 || (string.Compare(lines[i][0].ToString(), "1") != 0 && string.Compare(lines[i][0].ToString(),"0") != 0)){
				tileEnd = i;
				break;
			}
		}
		if (tileStart != -1){
			if (tileEnd == -1) tileEnd = lines.Count;
			int tilePosition = 0;
			int i;
			for (i = tileStart; i < tileEnd; i++){
				if (string.Compare(lines[i],"TILES" ) == 0) {
					continue;
				}
				string[] line = lines[i].Split(' ');
				tiles.Add(new List<int>());
				int j;
				for (j = 0; j < line.Length; j++){
					tiles[tilePosition].Add (int.Parse(line[j]));
				}
				width = j;
				tilePosition++;
			}
			height = i;
		}
		
		for (int i = 0; i < lines.Count; i++){
			if (string.Compare(lines[i], "PLAYER") == 0){
				string playerLine = lines[i+1];
				string[] playerLineSplit = playerLine.Split (',');
				playerPosition = new Vector3(float.Parse (playerLineSplit[0]), float.Parse (playerLineSplit[1]),0);
				break;
			}
		}
		
		for (int i = 0; i < lines.Count; i++){
			if (string.Compare(lines[i], "EXIT") == 0){
				string exitLine = lines[i+1];
				string[] exitLineSplit = exitLine.Split (',');
				exitPosition = new Vector3(float.Parse (exitLineSplit[0]), float.Parse (exitLineSplit[1]),0);
				break;
			}
		}
		
		for(int i=0;i<objectList.Length;i++){
			readPositions(objectList[i],lines);
		}
		
	}
	public void write(string fileName){
		if (fileName == "") return;
		
		string path = "Assets/Resources/LevelFiles/" + fileName + ".txt";
		
		StreamWriter sw;
		if (File.Exists(path)) {
			sw = new StreamWriter(path);
		}
		else {
			sw = File.CreateText(path);
		}
		
		sw.WriteLine("TILES");
		for (int i = 0; i < tiles.Count; i++){
			string line = "";
			for (int j = 0; j < tiles[i].Count; j++){
				line += tiles[i][j].ToString();
				if (j < tiles[i].Count - 1) line += " ";
			}
			sw.WriteLine(line);
		}
		
		sw.WriteLine("PLAYER");
		string playerLine = playerPosition[0].ToString() + "," + playerPosition[1].ToString();
		sw.WriteLine(playerLine);
		
		sw.WriteLine("EXIT");
		string exitLine = exitPosition[0].ToString() + "," + exitPosition[1].ToString();
		sw.WriteLine(exitLine);
		for(int i=0;i<objectList.Length;i++){
			writePositions(objectList[i],sw);
		}
		sw.Close();
	}
	public void readPositions(string objectName, List<string> lines){
		List<Vector3> positions;
		List<List<Vector3>> patrolPositions = null;
		if(objectName == "SLOW ENEMY"){
			positions = slowEnemyPositions;
			patrolPositions = slowEnemyPatrol;
		}
		else if(objectName == "FAST ENEMY"){
			positions = fastEnemyPositions;
			patrolPositions = fastEnemyPatrol;
		}
		else if(objectName == "POUNCE ENEMY"){
			positions = pounceEnemyPositions;
			patrolPositions = pounceEnemyPatrol;
		}
		else if(objectName == "RANGED ENEMY"){
			positions = rangedEnemyPositions;
			patrolPositions = rangedEnemyPatrol;
		}
		else if(objectName == "TOWER"){
			positions = towerPositions;
		}
		else if(objectName == "DEAD ZONE"){
			positions = deadZonePositions;
		}
		else if(objectName == "PUSH MINE"){
			positions = pushMinePositions;
		}
		else if(objectName == "SLOW MINE"){
			positions = slowMinePositions;
		}
		else if(objectName == "BLOWUP MINE"){
			positions = blowupMinePositions;
		}
		else if(objectName == "INVISIJUICE"){
			positions = invisijuicePositions;
		}
		else{
			positions = new List<Vector3>();
		}
		for (int i = 0; i < lines.Count; i++){
			if (string.Compare(lines[i], objectName) == 0){
				string BELine = lines[i+1];
				string[] BELineSplit = BELine.Split (' ');
				for (int j = 0; j < BELineSplit.Length; j++) {
					string[] BE = BELineSplit[j].Split(',');
					positions.Add (new Vector3(float.Parse(BE[0]), float.Parse(BE[1]), 0));
					if(patrolPositions != null){
						patrolPositions.Add (new List<Vector3>());
					}
				}
				break;
			}
		}
		if(patrolPositions != null){
			for (int i = 0; i < lines.Count; i++){
				if (string.Compare(lines[i], objectName + " PATROL") == 0){
					string BELine = lines[i+1];
					string[] BELineSplit = BELine.Split (' ');
					for (int j = 0; j < BELineSplit.Length; j++) {
						string[] lineSplit2 = BELineSplit[j].Split (';');
						for(int k=0;k<lineSplit2.Length; k++){
							string[] BE = lineSplit2[k].Split(',');
							if(BE[0]=="|"){
								continue;
							}
							patrolPositions[j].Add (new Vector3(float.Parse(BE[0]), float.Parse(BE[1]), 0));
						}
					}
					break;
				}
			}
		}
	}
	public void writePositions(string objectName, StreamWriter sw){
		List<Vector3> positions;
		List<List<Vector3>> patrolPositions = null;
		if(objectName == "SLOW ENEMY"){
			positions = slowEnemyPositions;
			patrolPositions = slowEnemyPatrol;
		}
		else if(objectName == "FAST ENEMY"){
			positions = fastEnemyPositions;
			patrolPositions = fastEnemyPatrol;
		}
		else if(objectName == "POUNCE ENEMY"){
			positions = pounceEnemyPositions;
			patrolPositions = pounceEnemyPatrol;
		}
		else if(objectName == "RANGED ENEMY"){
			positions = rangedEnemyPositions;
			patrolPositions = rangedEnemyPatrol;
		}
		else if(objectName == "TOWER"){
			positions = towerPositions;
		}
		else if(objectName == "DEAD ZONE"){
			positions = deadZonePositions;
		}
		else if(objectName == "PUSH MINE"){
			positions = pushMinePositions;
		}
		else if(objectName == "SLOW MINE"){
			positions = slowMinePositions;
		}
		else if(objectName == "BLOWUP MINE"){
			positions = blowupMinePositions;
		}
		else if(objectName == "INVISIJUICE"){
			positions = invisijuicePositions;
		}
		else{
			positions = new List<Vector3>();
		}
		if (positions.Count > 0){
			sw.WriteLine(objectName);
			string positionsLine = "";
			for (int i = 0; i < positions.Count; i++){
				positionsLine += positions[i][0].ToString() + "," + positions[i][1].ToString();
				if (i < positions.Count - 1){
					positionsLine += " ";
				}
			}
			sw.WriteLine(positionsLine);
		}
		if (patrolPositions != null){
			if(patrolPositions.Count > 0){
				sw.WriteLine(objectName + " PATROL");
				string positionsLine = "";
				for (int i = 0; i < patrolPositions.Count; i++){
					if (i > 0 && i < patrolPositions.Count){
						positionsLine += " ";
					}
					for(int j = 0; j< patrolPositions[i].Count; j++){
						if(j>0 && j < patrolPositions[i].Count){
							positionsLine += ";";
						}
						positionsLine += patrolPositions[i][j][0].ToString() + "," + patrolPositions[i][j][1].ToString();
					}
					if(patrolPositions[i].Count == 0){
						positionsLine += "|";
					}
				}
				sw.WriteLine(positionsLine);
			}
		}
	}
	public void setDimensions(int x, int y){
		tiles = new List<List<int>>();
		width = x;
		height = y;
		for(int i=0;i<width;i++){
			tiles.Add (new List<int>());
			for (int j = 0; j < height; j++){
				tiles[i].Add (0);
			}
		}
	}
}