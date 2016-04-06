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
	private bool moving;
	private Vector2 tileSpot;
	private float moveSpeed = 0.07f;

	// Use this for initialization
	void Start () {
		gridCont = Camera.main.GetComponent<GridControl>();
		tiles = gridCont.tiles;
		currI = gridCont.convertToTileCoord (transform.position.x);
		currJ = gridCont.convertToTileCoord (transform.position.y);
		jRange = 3;
		maxJ = currJ + jRange;
		minJ = currJ - jRange;
		moving = false;
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
		checkMoving (newI, newJ);
		if (moving) {
			transform.position = Vector2.MoveTowards (transform.position, tileSpot, moveSpeed);
		} else {
			if (transform.parent == null) {
				tileSpot = new Vector2 (newI, newJ - gridCont.tileSize / 3);
				transform.position = Vector2.MoveTowards (transform.position, tileSpot, moveSpeed);
			}
		}
	}

	private void checkMoving(int i, int j){
		int tsI = gridCont.convertToTileCoord (tileSpot.x);
		int tsJ = gridCont.convertToTileCoord (tileSpot.y);
		if (i == tsI && j == tsJ) {
			moving = false;
		}
	}

	public void moveTo(int i, int j){
		moving = true;
		tileSpot = new Vector2 (i, j - gridCont.tileSize / 3);
	}
}
