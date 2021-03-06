﻿using UnityEngine;
using System.Collections;

public class yarnCatControl : allCatsControl {
	
	private bool facingRight;
	private SpriteRenderer mySprite;
	private bool floating;
	private bool falling;
	private bool onYarn;
	private bool reachedYarnFirstTime;
	private float origMoveSpeed;
    private Animator anim;
	private float animSpeed;

	// Use this for initialization
	void Start () {
		base.initialize ();
		mySprite = GetComponent<SpriteRenderer> ();
		floating = false;
		falling = false;
//		onYarn = false;
		reachedYarnFirstTime = false;
		//		movingTimer -= 0.1f;
		movingTimer /= 2f;
		moveSpeed /= 2f;
		origMoveSpeed = moveSpeed;
		origMovingTimer = movingTimer;
        //		currI = gridCont.convertToTileCoord (transform.position.x);
        //		currJ = gridCont.convertToTileCoord (transform.position.y);
        anim = gameObject.GetComponent<Animator>();
		animSpeed = anim.speed;
	}

	// Update is called once per frame
	void Update () {
		bool slowedDown = false;
		if (timeIsNormal ()) {
			anim.speed = animSpeed;
			if (!onYarn) {
				if (!floating && !falling) {
					tileStuff tileScript = tiles [currI, currJ].GetComponent<tileStuff> ();
					tileStuff belowTile = tiles [currI, currJ - 1].GetComponent<tileStuff> ();
					GameObject elevCat = tileScript.getElevCat ();
					if (belowTile.getIsPlatform () || elevCat != null) {
						if (elevCat == null || belowTile.getIsPlatform ()) {
							moveSpeed = origMoveSpeed;
						} else {
							slowedDown = true;
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
			} else {
				if (!reachedYarnFirstTime) {
					Debug.Log ("play on yarn anim");
					reachedYarnFirstTime = true;
					//						playWithYarn ();
				}
			}
			float speed = moveSpeed;
			if (falling || floating) {
				speed *= 1.5f;
				if (slowedDown) {
					speed *= 1.5f;
				}
			}
			moveCat (speed);
			changeSprite ();
			base.updateTilePos ();
		} else {
			anim.speed = 0f;
		}
	}

//	private void playWithYarn(){
//		tileStuff tileScript = tiles [currI, currJ].GetComponent<tileStuff> ();
//	}

	public void setFacingRight(bool fr){
		facingRight = fr;
	}

	protected override void updateTiles(){
		tileStuff tileScript = tiles [currI, currJ].GetComponent<tileStuff> ();
		tileScript.removeYarnCat (transform.gameObject);
		tileScript = tiles [newI, newJ].GetComponent<tileStuff> ();
		tileScript.addYarnCat (transform.gameObject);
		currI = newI;
		currJ = newJ;
//		if (tileScript.canPlayWithYarn ()) {
//			onYarn = true;
//			tileScript.setYarnCatOnYarn (true);
//			tileScript.getYarnObj ().GetComponent<yarnControl> ().activateDoors (false);
//		}
	}

	public void setOnYarn(bool oy){
		onYarn = oy;
		if (anim == null) {
			anim = GetComponent<Animator> ();
			animSpeed = anim.speed;
		}
        if (oy)
        {
            anim.SetBool("onYarn", true);
        }
		if (gridCont == null) {
			currI = (int)Mathf.Round (transform.position.x);
			currJ = (int)Mathf.Round (transform.position.y);
		} else {
			currI = gridCont.convertToTileCoord (transform.position.x);
			currJ = gridCont.convertToTileCoord (transform.position.y);
		}
		tileSpot = new Vector2 (currI, currJ);
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
