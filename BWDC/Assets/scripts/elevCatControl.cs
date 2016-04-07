using UnityEngine;
using System.Collections;

public class elevCatControl : allCatsControl {

	private int jRange;
	public int maxJ { get; set; }
	public int minJ { get; set; }
	private bool moving;

	// Use this for initialization
	void Start () {
		base.initialize();
		jRange = 3;
		maxJ = currJ + jRange;
		minJ = currJ - jRange;
		moving = false;
	}
	
	// Update is called once per frame
	void Update () {
		base.updateTilePos();
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

	protected override void updateTiles (){
		tileStuff tileScript = tiles [newI, newJ].GetComponent<tileStuff> ();
		tileScript.setElevCat (transform.gameObject);
		tileScript = tiles [currI, currJ].GetComponent<tileStuff> ();
		tileScript.setElevCat (null);
		currI = newI;
		currJ = newJ;
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
