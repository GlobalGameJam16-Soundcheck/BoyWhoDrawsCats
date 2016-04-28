using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class GridControl : MonoBehaviour {

    int tileWidth;
    int tileHeight;
//    string[,] tiles;
	public GameObject [,] tiles { get; set; }
    public GameObject tile;
	private Sprite blockSprite;
	public GameObject blankTile;
	public GameObject rat;
    public TextAsset levelCsv;
	public float tileSize { get; set; }
	public bool gamePaused { get; set; }

	public Color[] yarnDoorColors;
	public GameObject yarn;
	public GameObject door;

	public GameObject boy;
	public GameObject endPrefab;

	private Dictionary<int, List<GameObject>> doorDict;

	public int tapScene;
	public int elevCatScene;
	public int yarnCatScene;
	public int attackCatScene;
//	attackCatScene = SceneManager.L

	public GameObject[] npcChars;

    // Use this for initialization
    void Start () {
		tileSize = 1f;
//        TextAsset levelCsv = (TextAsset)Resources.Load("level", typeof(TextAsset));
		doorDict = new Dictionary<int, List<GameObject>>();
        string[,] tempTiles = CSVReader.SplitCsvGrid(levelCsv.text);
        tileWidth = tempTiles.GetLength(0);
        tileHeight = tempTiles.GetLength(1) - 1; //get rid of the newlines
		tiles = new GameObject[tileWidth,tileHeight];
        for (int x = 0; x < tileWidth; x++) {
            for(int y = 0; y < tileHeight; y++) {
				bool isPlatform = false;
				GameObject yarnObj = null;
				GameObject endObj = null;
				bool isADoor = false;
				string t = tempTiles [x, tileHeight - 1 - y];
				if (!string.IsNullOrEmpty (t)) {
					t = t.ToLower ();
					t = t.Trim ();
				}
				Debug.Log ("t is: " + t);
				if (t == "b") { //platform
					isPlatform = true;
					tiles [x, y] = (GameObject)(Instantiate (tile, new Vector3 (x, y, 0), Quaternion.identity));
				} else if (string.IsNullOrEmpty (t) || consistsOfWhiteSpace (t) || t == "r") { //blank
					tiles [x, y] = (GameObject)(Instantiate (blankTile, new Vector3 (x, y, 0), Quaternion.identity));
					if (t == "r") { //rat
						tiles [x, y].GetComponent<tileStuff> ().placeRat (rat, tileSize);
					}
				} else if (!string.IsNullOrEmpty (t) && t.Length > 1 && t.Contains ("c")) {//npc character
					tiles [x, y] = (GameObject)(Instantiate (blankTile, new Vector3 (x, y, 0), Quaternion.identity));
					string[] tArr = t.Split (new char[]{ '-' });
					int whichCar = int.Parse (tArr [1]);
					if (whichCar >= 0 && whichCar < npcChars.Length) {
						Instantiate (npcChars [whichCar], new Vector3 (x, y, 0), Quaternion.identity);
					}
				} else if (t == "s") { //start
					tiles [x, y] = (GameObject)(Instantiate (blankTile, new Vector3 (x, y, 0), Quaternion.identity));
					boy.GetComponent<movment> ().initialize (new Vector3 (x, y, 0));
				} else if (t == "e") { //end
					tiles [x, y] = (GameObject)(Instantiate (blankTile, new Vector3 (x, y, 0), Quaternion.identity));
					endObj = (GameObject)(Instantiate (endPrefab, new Vector3 (x, y, 0), Quaternion.identity));
				} else if (!string.IsNullOrEmpty (t) && t.Length > 1 && t.Contains ("d")) { //door
//					Debug.Break ();
					//this is a door that the yarn contains
					isPlatform = true;
					tiles [x, y] = (GameObject)(Instantiate (blankTile, new Vector3 (x, y, 0), Quaternion.identity));
					isADoor = true;
					string[] tArr = t.Split (new char[]{ '-' });
					int doorVal = int.Parse (tArr [1]);
					GameObject newDoorObj = (GameObject)(Instantiate (door, new Vector3 (x, y, 0), Quaternion.identity));
					doorController newDoor = newDoorObj.GetComponent<doorController> ();
//					int active = int.Parse(tArr [2]);
//					newDoor.initialize (x, y, yarnDoorColors [doorVal], doorVal, active);
					newDoor.initialize (x, y, yarnDoorColors [doorVal], doorVal);
					if (!doorDict.ContainsKey (doorVal)) {
						doorDict.Add (doorVal, new List<GameObject> ());
					}
					doorDict [doorVal].Add (newDoorObj);
				} else if (!string.IsNullOrEmpty (t) && t.Length > 1 && t.Contains ("y")) { //yarn
					tiles [x, y] = (GameObject)(Instantiate (blankTile, new Vector3 (x, y, 0), Quaternion.identity));
					Debug.Log ("y-something");
					string[] tArr = t.Split (new char[]{ '-' });
					int yarnVal = int.Parse (tArr [1]);
					GameObject newYarnObj = (GameObject)(Instantiate (yarn, new Vector3 (x, y, 0), Quaternion.identity));
					yarnControl newYarn = newYarnObj.GetComponent<yarnControl> ();
					newYarn.initialize (x, y, yarnDoorColors [yarnVal], yarnVal);
					yarnObj = newYarnObj;
				} else {
//					Debug.Break ();
				}
				tiles [x, y].GetComponent<tileStuff> ().setIsPlatform (isPlatform);
				tiles [x, y].GetComponent<tileStuff> ().setIsADoor (isADoor);
				tiles [x, y].GetComponent<tileStuff> ().setYarnObj (yarnObj);
				tiles [x, y].GetComponent<tileStuff> ().setEndTile (endObj);
            }
        }
	}

	private bool consistsOfWhiteSpace(string s){
		foreach(char c in s){
			if(c != ' ') return false;
		}
		return true;
	}

	void Update(){
		if (Input.GetKeyDown ("r")) {
			reloadScene ();
		}
	}

	public void reloadScene(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public int convertToTileCoord(float x){
		return (int)Mathf.Round (x);
	}

	public bool onGrid(int i, int j){
		return (0 <= i && 0 <= j && i < tileWidth && j < tileHeight);
	}

	public void setDoors(bool active, int doorVal){
		foreach (GameObject doorObj in doorDict[doorVal]){
			if (active) {
				doorObj.GetComponent<doorController> ().setActive ();
			} else {
				doorObj.GetComponent<doorController> ().setInactive ();
			}
		}
	}

}
