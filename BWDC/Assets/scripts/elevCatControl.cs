using UnityEngine;
using System.Collections;

public class elevCatControl : allCatsControl {

	private int jRange;
	public int maxJ { get; set; }
	public int minJ { get; set; }
	public bool moving { get; set; }
	public bool actuallyMoving { get; set; }
	private Vector2 oldPos;
	public GameObject[] legs;
	private int initI;
	private int initJ;

	// Use this for initialization
	void Start () {
		base.initialize();
		jRange = 3;
		maxJ = currJ + jRange;
		minJ = currJ - jRange;
		moveSpeed = 3f;
		moving = false;
		actuallyMoving = false;
		oldPos = transform.position;
		initI = gridCont.convertToTileCoord (transform.position.x);
		initJ = gridCont.convertToTileCoord (transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		if (timeIsNormal ()) {
			base.updateTilePos ();
			checkLegs ();
			if (!moving) {
				if (transform.parent == null) {
					tileSpot = new Vector2 (newI, newJ - gridCont.tileSize / 3);
					moveCat (moveSpeed);
				} else {
					//fixme make sure i'm at the bottom of the boy's tile if the boy isnt moving i guess
				}
			} else {
//				transform.position = Vector2.MoveTowards (transform.position, tileSpot, moveSpeed);
				moveCat(moveSpeed);
			}
			checkMoving (newI, newJ);
		}
	}

	private void checkLegs(){
		for (int i = 0; i < legs.Length; i++) {
			if (i < (currJ - initJ)) {
				legs [i].SetActive (true);
			} else {
				legs [i].SetActive (false);
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
			movingTimer -= Time.deltaTime;
			if (movingTimer < 0f) {
				movingTimer = origMovingTimer;
				moving = false;
			}
		}
//		Debug.Log("old: " + oldPos + " new: " + transform.position);
		if (Vector2.Distance (oldPos, transform.position) <= 0.005) {
			actuallyMoving = false;
		} else {
			actuallyMoving = true;
		}
		oldPos = transform.position;
	}

	public int moveTo(int i, int j){
		if (j == currJ) {
//			moving = false;
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
//			Debug.Log (movingDown);
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

	public Vector2 getTileSpot(){
		return tileSpot;
	}

	public float getMoveSpeed(){
		return moveSpeed;
	}

}
