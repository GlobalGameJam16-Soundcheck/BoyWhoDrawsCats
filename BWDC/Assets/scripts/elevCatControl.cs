using UnityEngine;
using System.Collections;

public class elevCatControl : allCatsControl {

	private int jRange;
	public int maxJ { get; set; }
	public int minJ { get; set; }
	public bool moving { get; set; }
	private bool posSet;

	// Use this for initialization
	void Start () {
		base.initialize();
		jRange = 3;
		maxJ = currJ + jRange;
		minJ = currJ - jRange;
		moving = false;
		posSet = false;
	}
	
	// Update is called once per frame
	void Update () {
		base.updateTilePos();
		if (!moving) {
			if (transform.parent == null) {
				tileSpot = new Vector2 (newI, newJ - gridCont.tileSize / 3);
				transform.position = Vector2.MoveTowards (transform.position, tileSpot, moveSpeed);
			} else {
				//fixme make sure i'm at the bottom of the boy's tile if the boy isnt moving i guess
			}
		} else {
			transform.position = Vector2.MoveTowards (transform.position, tileSpot, moveSpeed);
		}
		checkMoving (newI, newJ);
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

	public int moveTo(int i, int j){
		if (j == currJ) {
			moving = false;
			return currJ;
		}
		int foundJ = j;
		tileStuff tileScript;
		bool movingDown = (j < currJ);
		int nextJ = currJ;
		bool notPlat = false;
		while (!notPlat) {
			if (movingDown) {
				nextJ--;
			} else {
				nextJ++;
			}
			Debug.Log (movingDown);
			tileScript = tiles [i, nextJ].GetComponent<tileStuff> ();
			if (tileScript.getIsPlatform () || tileScript.getElevCat () != null) {
				notPlat = true;
				if (movingDown) {
					foundJ = nextJ + 1;
				} else {
					foundJ = nextJ - 1;
				}
			} else {
				if (j == nextJ || nextJ == currJ) {
					foundJ = nextJ;
					notPlat = true;
				}
			}
		}
		moving = true;
		tileSpot = new Vector2 (i, gridCont.convertToTileCoord(foundJ) - gridCont.tileSize / 3);
		return gridCont.convertToTileCoord (foundJ);
	}

	public void setPos(int i, int j){
		float epsilon = 0.05f;
		Vector2 tryVect = new Vector2 (i, j - gridCont.tileSize / 3);
		if (Vector2.Distance (tryVect, transform.position) > epsilon) {
			tileSpot = tryVect;
			posSet = true;
		}
//		if (moving) {
//			Debug.Log ("moving");
//			tileSpot = new Vector2 (i, j - gridCont.tileSize / 3);
//			posSet = true;
//		} else {
//			Debug.Log ("not moving");
//		}
	}

}
