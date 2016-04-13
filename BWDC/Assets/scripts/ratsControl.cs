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
//		movingTimer /= 2f;
//		origMovingTimer = movingTimer;
		onElevCat = false;
		elevCatObj = null;
		origMoveSpeed = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (started) {
			base.updateTilePos ();
			moveForwards ();
			transform.position = Vector2.MoveTowards (transform.position, tileSpot, moveSpeed / speedDenom);
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
		tileScript.setRat (null);
		tileScript = tiles [newI, newJ].GetComponent<tileStuff> ();
		tileScript.setRat (transform.gameObject);
		currI = newI;
		currJ = newJ;
//		if (tileScript.hasAttackCat()) {
//			tileScript.deleteAttackCat ();
//			tileScript.deleteRat ();
//		}
	}

	private void moveForwards(){
		int tileOver = currI;
		if (facingRight) {
			tileOver++;
		} else {
			tileOver--;
		}
		tileStuff tileOverScript;
		bool turnAround = false;
		bool doRegular = true;
		if (gridCont.onGrid (tileOver, currJ)) {
			int tileDown = currJ - 1;
			tileStuff downTile = tiles [currI, tileDown].GetComponent<tileStuff> ();
			tileStuff downTileOver = tiles [tileOver, tileDown].GetComponent<tileStuff> ();
			if (!onElevCat || downTileOver.getIsPlatform () || elevCatObj == null) { //do regular left right movement
				if (elevCatObj != null && elevCatObj.GetComponent<elevCatControl> ().actuallyMoving) {
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
					tileOverScript = tiles [tileOver, currJ].GetComponent<tileStuff> ();
					turnAround = doRegularMovement (tileOverScript, downTileOver);
				}
			} else {
				doRegular = false;
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

	private bool doRegularMovement(tileStuff tileOverScript, tileStuff downTileOver){
		bool turnAround = false;
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

}
