using UnityEngine;
using System.Collections;

public class attackCatControl : allCatsControl {

	private bool facingRight;
	private SpriteRenderer mySprite;
	private bool floating;
	private bool falling;

	// Use this for initialization
	void Start () {
		base.initialize ();
		mySprite = GetComponent<SpriteRenderer> ();
		floating = false;
		falling = false;
	}
	
	// Update is called once per frame
	void Update () {
		checkAttackingRat ();
		if (!floating && !falling) {
			moveForwards ();
		} else {
			fallDownwards ();
		}
		transform.position = Vector2.MoveTowards (transform.position, tileSpot, moveSpeed / 3);
		changeSprite ();
		base.updateTilePos ();
	}

	public void setFacingRight(bool fr){
		facingRight = fr;
	}

	protected override void updateTiles(){
		tileStuff tileScript = tiles [newI, newJ].GetComponent<tileStuff> ();
		tileScript.setAttackCat (transform.gameObject);
		tileScript = tiles [currI, currJ].GetComponent<tileStuff> ();
		tileScript.setAttackCat (null);
		currI = newI;
		currJ = newJ;
	}

	private void checkAttackingRat (){
		
	}

	private void changeSprite(){
		if (facingRight) {
			mySprite.flipX = true;
		} else {
			mySprite.flipX = false;
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
				turnAround = true;
			} else {
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
