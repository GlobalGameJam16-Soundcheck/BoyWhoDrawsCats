using UnityEngine;
using System.Collections;

public class ratsControl : allCatsControl {

	private bool facingRight;
	private SpriteRenderer mySprite;
	private bool started;
	private float speedDenom;
	private bool onElevCat;
	private GameObject elevCatObj;
	private float origMoveSpeed;
	private float origMoveTimerOnElev;
	private float origMoveSpeedOnElev;
	private bool falling;
	private int damage;

	// Use this for initialization
	void Start () {
		started = false;
		Invoke ("delayedStart", 1f);
	}

	private void delayedStart(){
		base.initialize ();
		mySprite = GetComponent<SpriteRenderer> ();
//		speedDenom = Random.Range (1f, 10f);
		speedDenom = 4f;
		started = true;
		movingTimer *= 1f / 10f;
		moveSpeed *= 2f;
		origMoveSpeed = moveSpeed;
		origMovingTimer = movingTimer;
		onElevCat = false;
		elevCatObj = null;
		origMoveSpeed = moveSpeed;
		origMoveTimerOnElev = movingTimer / 1f;
		origMoveSpeedOnElev = moveSpeed / 2f;
		falling = false;
		damage = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (started && timeIsNormal()) {
			base.updateTilePos ();
			moveForwards ();
			float speed = moveSpeed / speedDenom;
			moveCat (speed);
			changeSprite ();
		}
	}

	private void changeSprite(){
		if (facingRight) {
			mySprite.flipX = true;
		} else {
			mySprite.flipX = false;
		}
	}

	protected override void updateTiles(){
		tileStuff tileScript = tiles [currI, currJ].GetComponent<tileStuff> ();
		tileScript.removeRat (transform.gameObject);
		tileScript = tiles [newI, newJ].GetComponent<tileStuff> ();
		tileScript.addRat (transform.gameObject);
		currI = newI;
		currJ = newJ;
//		if (tileScript.hasAttackCat()) {
//			tileScript.deleteAttackCat ();
//			tileScript.deleteRat ();
//		}
	}

	private void moveForwards(){
//		if (falling) {
//			Debug.Log ("checkFalling");
//			falling = checkFalling ();
//			if (falling) {
//				return;
//			}
//		}
		if (checkFalling ()){// && tiles[currI,currJ].GetComponent<tileStuff>().getElevCat() != null) {
			fallDown ();
			return;
		}
		int tileOver = currI;
		if (facingRight) {
			tileOver++;
		} else {
			tileOver--;
		}
		tileStuff tileOverScript = tiles [tileOver, currJ].GetComponent<tileStuff> ();
		bool turnAround = false;
		bool doRegular = true;
		if (gridCont.onGrid (tileOver, currJ)) {
			int tileDown = currJ - 1;
			tileStuff downTile = tiles [currI, tileDown].GetComponent<tileStuff> ();
			tileStuff downTileOver = tiles [tileOver, tileDown].GetComponent<tileStuff> ();
			if (!onElevCat || downTileOver.getIsPlatform () || elevCatObj == null) { //do regular left right movement
				if (elevCatObj != null && elevCatObj.GetComponent<elevCatControl> ().actuallyMoving && !downTile.getIsPlatform()) {
//					Debug.Break ();
					doRegular = false;
				} else {
					tileStuff tileScript = tiles [currI, currJ].GetComponent<tileStuff> ();
					elevCatObj = tileScript.getElevCat ();
					if (elevCatObj == null) {
						onElevCat = false;
						moveSpeed = origMoveSpeed;
					} else {
						onElevCat = true;
//						doRegular = false;
					}
					turnAround = doRegularMovement (tileOverScript, downTileOver, false);
				}
			} else {
				if (elevCatObj.GetComponent<elevCatControl> ().actuallyMoving) {
					doRegular = false;
				} else {
//					movingTimer
					turnAround = doRegularMovement (tileOverScript, downTileOver, true);
//					moveSpeed = origMoveSpeed;
				}
			}
		} else {
			turnAround = true;
		}
		if (turnAround){
			facingRight = !facingRight;
		}
		float denom = 3f;
		if (doRegular) {
			if (facingRight) {
				tileSpot = new Vector2 (currI + 1, currJ - gridCont.tileSize / denom);
			} else {
				tileSpot = new Vector2 (currI - 1, currJ - gridCont.tileSize / denom);
			}
		} else {
			elevCatControl ecc = elevCatObj.GetComponent<elevCatControl> ();
			Debug.Log ("on a cat move towards that stuff");
			moveSpeed = ecc.getMoveSpeed() * 4f;
			Debug.Log ("new move speed: " + moveSpeed);
			tileSpot = ecc.getTileSpot ();
		}
	}

	private bool doRegularMovement(tileStuff tileOverScript, tileStuff downTileOver, bool turnFaster){
		bool turnAround = false;
		if (turnFaster) {
			float currDiff = origMovingTimer - movingTimer;
			movingTimer = origMoveTimerOnElev - currDiff;
			moveSpeed = origMoveSpeedOnElev;
		}
		if (tileOverScript.getIsPlatform ()) { //must turn around 
			movingTimer -= Time.deltaTime;
			if (movingTimer < 0f) {
				turnAround = true;
				movingTimer = origMovingTimer;
			}
		} else {
//			elevCatObj = tileScript.getElevCat ();
			if (!downTileOver.getIsPlatform () && (tileOverScript.getElevCat() == null)) { //must turn around
//				onElevCat = false;
				movingTimer -= Time.deltaTime;
				if (movingTimer < 0f) {
					turnAround = true;
					movingTimer = origMovingTimer;
				}
			} else {
				tileStuff tileScript = tiles [currI, currJ].GetComponent<tileStuff> ();
				tileStuff downTile = tiles [currI, currJ - 1].GetComponent<tileStuff> ();
				if (tileScript.getElevCat () == null && !downTile.getIsPlatform()) {
					Debug.Log ("***********");
					turnAround = true;
				}
				movingTimer = origMovingTimer;
//				if (elevCatObj != null) {
//					onElevCat = true;
//				}
			}
		}
		return turnAround;
	}

	private bool checkFalling(){
		if (!gridCont.onGrid (currI, currJ) || !gridCont.onGrid (currI, currJ - 1)) {
			return true;
		}
		tileStuff tileScript = tiles [currI, currJ].GetComponent<tileStuff> ();
		tileStuff downTile = tiles [currI, currJ - 1].GetComponent<tileStuff> ();
		if (tileScript.getElevCat () != null || downTile.getIsPlatform()) {
			return false;
		}
		return true;
	}

	public void fallDown(){
		Debug.Log ("falling!");
		tileStuff tileScript;
		moveSpeed = origMoveSpeed;// * 4f;
		falling = true;
		elevCatObj = null;
		int nextJ = currJ;
		int foundJ = nextJ;
		bool notPlat = false;
		while (!notPlat) {
			nextJ--;
			tileScript = tiles [currI, nextJ].GetComponent<tileStuff> ();
			if (tileScript.getIsPlatform ()) {
				notPlat = true;
				foundJ = nextJ + 1;
			} else if (tileScript.getElevCat () != null) {
				notPlat = true;
				foundJ = nextJ;
			} else {
				if (nextJ == currJ) {
					foundJ = nextJ;
					notPlat = true;
				}
			}
		}
		tileSpot = new Vector2 (currI, gridCont.convertToTileCoord(foundJ) - gridCont.tileSize / 3);
	}

	public int getDamage(){
		return damage;
	}

}
