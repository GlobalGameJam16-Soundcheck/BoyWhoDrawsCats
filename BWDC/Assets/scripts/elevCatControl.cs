using UnityEngine;
using System.Collections;

public class elevCatControl : allCatsControl {

	private GridControl gridCont;
	private GameObject[,] tiles;
	private int currI;
	private int currJ;
	private int jRange;
	public int maxJ { get; set; }
	public int minJ { get; set; }

	// Use this for initialization
	void Start () {
		gridCont = Camera.main.GetComponent<GridControl>();
		tiles = gridCont.tiles;
		currI = gridCont.convertToTileCoord (transform.position.x);
		currJ = gridCont.convertToTileCoord (transform.position.y);
		jRange = 3;
		maxJ = currJ + jRange;
		minJ = currJ - jRange;
	}
	
	// Update is called once per frame
	void Update () {
		int newI = gridCont.convertToTileCoord (transform.position.x);
		int newJ = gridCont.convertToTileCoord (transform.position.y);
		if (newI != currI || newJ != currJ) {
			tileStuff tileScript = tiles [newI, newJ].GetComponent<tileStuff> ();
			tileScript.setHasElevCat (true);
			tileScript = tiles [currI, currJ].GetComponent<tileStuff> ();
			tileScript.setHasElevCat (false);
			currI = newI;
			currJ = newJ;
		}
		if (transform.parent == null) {
			Vector2 tileSpot = new Vector2 (newI, newJ - gridCont.tileSize / 3);
			transform.position = Vector2.MoveTowards (transform.position, tileSpot, 0.2f);
		}
	}
}
