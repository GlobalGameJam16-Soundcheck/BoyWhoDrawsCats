using UnityEngine;
using System.Collections;

public class elevCatControl : allCatsControl {

	private GridControl gridCont;
	private GameObject[,] tiles;
	private int currI;
	private int currJ;

	// Use this for initialization
	void Start () {
		gridCont = Camera.main.GetComponent<GridControl>();
		tiles = gridCont.tiles;
		currI = gridCont.convertToTileCoord (transform.position.x);
		currJ = gridCont.convertToTileCoord (transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		int newI = gridCont.convertToTileCoord (transform.position.x);
		int newJ = gridCont.convertToTileCoord (transform.position.y);
		if (newI != currI || newJ != currJ) {
			tileStuff tileScript = tiles [newI, newJ].GetComponent<tileStuff> ();
			tileScript.setHasElevCat (true);
			tileScript.setHasElevCat (false);
			currI = newI;
			currJ = newJ;
		}
	}
}
