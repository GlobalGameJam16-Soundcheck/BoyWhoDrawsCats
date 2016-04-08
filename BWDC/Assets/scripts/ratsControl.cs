using UnityEngine;
using System.Collections;

public class ratsControl : allCatsControl {

	private bool facingRight;
	private SpriteRenderer mySprite;
	private bool started;
	private float speedDenom;

	// Use this for initialization
	void Start () {
		started = false;
		Invoke ("delayedStart", 2f);
	}

	private void delayedStart(){
		base.initialize ();
		mySprite = GetComponent<SpriteRenderer> ();
//		speedDenom = Random.Range (1f, 10f);
		speedDenom = 4f;
		started = true;
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
		if (tileScript.attackCatObj != null) {
			tileScript.deleteAttackCat ();
			tileScript.deleteRat ();
		}
	}

	private void moveForwards(){
		int tileOver = currI;
		if (facingRight) {
			tileOver++;
		} else {
			tileOver--;
		}
		tileStuff tileScript;
		bool turnAround = false;
		if (gridCont.onGrid (tileOver, currJ)) {
			tileScript = tiles [tileOver, currJ].GetComponent<tileStuff> ();
			if (tileScript.getIsPlatform ()) {
				movingTimer -= Time.deltaTime;
				if (movingTimer < 0f) {
					turnAround = true;
					movingTimer = origMovingTimer;
				}
			} else {
				int tileDown = currJ - 1;
				tileStuff downTile = tiles [tileOver, tileDown].GetComponent<tileStuff> ();
				if (!downTile.getIsPlatform () && (tileScript.getElevCat () == null)) {
					movingTimer -= Time.deltaTime;
					if (movingTimer < 0f) {
						turnAround = true;
						movingTimer = origMovingTimer;
					}
				} else {
					movingTimer = origMovingTimer;
				}
			}
		} else {
			turnAround = true;
		}
		if (turnAround){
			facingRight = !facingRight;
		}
		float denom = 3f;
		if (facingRight) {
			tileSpot = new Vector2 (currI + 1, currJ - gridCont.tileSize / denom);
		} else {
			tileSpot = new Vector2 (currI - 1, currJ - gridCont.tileSize / denom);
		}
	}

}
