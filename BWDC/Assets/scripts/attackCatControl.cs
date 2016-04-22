using UnityEngine;
using System.Collections;

public class attackCatControl : allCatsControl {

	private bool facingRight;
	private SpriteRenderer mySprite;
	private bool floating;
	private bool falling;
	private float origMoveSpeed;

	// Use this for initialization
	void Start () {
		base.initialize ();
		mySprite = GetComponent<SpriteRenderer> ();
		floating = false;
		falling = false;
//		movingTimer -= 0.1f;
		movingTimer /= 2.5f;
		moveSpeed = moveSpeed / 1.5f;
		origMoveSpeed = moveSpeed;
		origMovingTimer = movingTimer;
		currI = gridCont.convertToTileCoord (transform.position.x);
		currJ = gridCont.convertToTileCoord (transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		if (timeIsNormal ()) {
			if (!floating && !falling) {
				tileStuff tileScript = tiles [currI, currJ].GetComponent<tileStuff> ();
				tileStuff belowTile = tiles [currI, currJ - 1].GetComponent<tileStuff> ();
				GameObject elevCat = tileScript.getElevCat ();
				if (belowTile.getIsPlatform () || elevCat != null) {
					if (elevCat == null || belowTile.getIsPlatform ()) {
						moveSpeed = origMoveSpeed;
					} else {
						moveSpeed = origMoveSpeed / 2f;
					}
					moveForwards ();
				} else {
					floating = true;
					fallDownwards ();
				}
			} else {
				fallDownwards ();
			}
			moveCat (moveSpeed);
			changeSprite ();
			base.updateTilePos ();
		}
	}

	public void setFacingRight(bool fr){
		facingRight = fr;
	}

	protected override void updateTiles(){
		tileStuff tileScript = tiles [currI, currJ].GetComponent<tileStuff> ();
		tileScript.removeAttackCat (transform.gameObject);
		tileScript = tiles [newI, newJ].GetComponent<tileStuff> ();
		tileScript.addAttackCat (transform.gameObject);
		currI = newI;
		currJ = newJ;
//		if (tileScript.ratObj != null) {
//			tileScript.deleteAttackCat (transform.gameObject);
//			tileScript.deleteRat ();
//		}
	}

	private void changeSprite(){
		if (facingRight) {
			mySprite.flipX = false;
		} else {
			mySprite.flipX = true;
		}
	}

	private void fallDownwards(){
		if (floating) {
			if (checkOnDestTile ()) {
				floating = false;
				falling = true;
			}
		} else {
			int tileDown = currJ - 1;
			tileStuff belowTile;
			tileStuff tileScript;
			if (gridCont.onGrid(currI, tileDown)){
				belowTile = tiles [currI, tileDown].GetComponent<tileStuff> ();
				tileScript = tiles [currI, currJ].GetComponent<tileStuff> ();
				if (belowTile.getIsPlatform () || (tileScript.getElevCat() != null)) {
					falling = false;
				}
			}
			tileSpot = new Vector2 (currI, tileDown);
		}
	}

	private bool checkOnDestTile(){
		return (currI == gridCont.convertToTileCoord (tileSpot.x) &&
				currJ == gridCont.convertToTileCoord (tileSpot.y));
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
				movingTimer = origMovingTimer;
				int tileDown = currJ - 1;
				tileStuff downTile = tiles [tileOver, tileDown].GetComponent<tileStuff> ();
				if (!downTile.getIsPlatform () && (tileScript.getElevCat() == null)) {
					floating = true;
				}
			}
		} else {
			turnAround = true;
		}
		if (turnAround){
			facingRight = !facingRight;
		}
		if (facingRight) {
			tileSpot = new Vector2 (currI + 1, currJ);
		} else {
			tileSpot = new Vector2 (currI - 1, currJ);
		}
	}

}
