using UnityEngine;
using System.Collections;

public class movment : MonoBehaviour {

	//note all the i's and j's are really x and y coordinates

	[Header("For the boy")]
    public GameObject highlight;
	public GameObject deleteLight;
	public GameObject drawingOn;
	public float mouseHoldTimer { get; set; }
	private float origMouseHoldTimer;
	public int inkLeft;
	public int health;
	private bool flashing;
	private float flashTimer;
	private float origFlashTimer;
	private int flashCount = 0;
	private SpriteRenderer mySprite;
	public bool gamePaused { get; set; }

    Vector3 dest;
    private float walkSpeed = 0.07f;
    GridControl gridCont;
	private GameObject[,] tiles;
	private bool facingRight = true;
	private int boyTileI;
	private int boyTileJ;
	private int tileWidth;
	private int tileHeight;
//	private bool newDestIsValid = true;
	private Vector3 camPos;

	[Header("ElevatorCat")]
	public GameObject elevCatPrefab;
	private bool usingElevator = false;
	private bool falling = false;
	private bool floating = true;
	private bool goingUp;
//	private int elevCatLayer;
	private int elevCatMaxJ;
	private int elevCatMinJ;
	public int elevCat = 1;
	private Transform currRidingCat = null;
	public int elevInkCost;

	[Header("AttackCat")]
	public GameObject attackCatPrefab;
	public int attackCat = 2;
	public int attackCatRight = 2;
	public int attackCatLeft = 3;
	public int attackInkCost;

	// Use this for initialization
	void Start () {
		gridCont = Camera.main.GetComponent<GridControl>();
//		elevCatLayer = 1 << (LayerMask.NameToLayer ("elevCat"));
		tiles = gridCont.tiles;
		tileWidth = tiles.GetLength (0);
		tileHeight = tiles.GetLength (1);
		int xPos = gridCont.convertToTileCoord(transform.position.x);
		int yPos = gridCont.convertToTileCoord(transform.position.y);
		dest = new Vector3 (xPos, yPos, 0f);
//		mySprite = GetComponent<SpriteRenderer> ();
//		rightSprite = mySprite.sprite;
		mouseHoldTimer = 0.15f;
		origMouseHoldTimer = mouseHoldTimer;
		mySprite = GetComponent<SpriteRenderer> ();
		flashTimer = 0.1f;
		origFlashTimer = flashTimer;
    }
	
	// Update is called once per frame
	void Update () {
		camPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		checkGotHit ();
		if (health > 0){
			changeSprite ();
			if (!gridCont.gamePaused) {
				transform.position = Vector3.MoveTowards (transform.position, new Vector3 (dest.x, dest.y, 0), walkSpeed);
				boyTileI = gridCont.convertToTileCoord (transform.position.x);
				boyTileJ = gridCont.convertToTileCoord (transform.position.y);
				checkHitByRat ();
			}
			if (Input.GetMouseButton (0)) {
				mouseHoldTimer -= Time.deltaTime;
				Debug.Log (mouseHoldTimer);
				if (mouseHoldTimer <= 0) {
					gridCont.gamePaused = true;
					if (!drawingOn.activeInHierarchy) {
						drawingOn.SetActive (true);
					}
				}
			}
	        if (Input.GetMouseButtonUp(0)) {
				gridCont.gamePaused = false;
				if (drawingOn.activeInHierarchy) {
					drawingOn.SetActive (false);
				}
				float oldMouseTimer = mouseHoldTimer;
				mouseHoldTimer = origMouseHoldTimer;
				if (oldMouseTimer > 0f) {
					if ((!falling && !floating) || (reachedDestination () && usingElevator)) {
						checkNewPos ();
					}
				} else {
					Debug.Log ("must be drawing?");
				}
	        }
			if (!gridCont.gamePaused) {
				if (!reachedDestination ()) {
					checkDestination ();
				} else {
					if (falling) {
						setFallingDest ();
					}
				}
			}
		} else {
			
		}
    }

	private void checkGotHit(){
		if (flashing) {
			flashTimer -= Time.deltaTime;
			if (flashTimer <= 0) {
				flashTimer = origFlashTimer;
				if (flashCount % 2 == 0) {
					mySprite.color = new Color (mySprite.color.r, mySprite.color.g, mySprite.color.b, 0.2f);
				} else {
					mySprite.color = new Color (mySprite.color.r, mySprite.color.g, mySprite.color.b, 1f);
				}
				flashCount++;
			}
		}
	}

	private void checkHitByRat(){
		GameObject ratObj = tiles [boyTileI, boyTileJ].GetComponent<tileStuff> ().getRat ();
		if (ratObj != null) {
			health -= ratObj.GetComponent<ratsControl>().getDamage ();
			if (health <= 0) {
//				Time.timeScale /= 2;
				flashing = true;
				Invoke ("death", 2f);
			}
		}
	}

	private void death(){
		gridCont.reloadScene ();
	}

	private void changeSprite(){

	}

	private bool reachedDestination(){
		int i = gridCont.convertToTileCoord (dest.x);
		int j = gridCont.convertToTileCoord (dest.y);
		bool ret = (boyTileI == i && boyTileJ == j);
		if (ret) {
			if (floating) {
				falling = true;
				floating = false;
			} else if (falling) {
				falling = false;
			}
		}
		return ret;
	}

	public void deleteCats(){
		int clickedI = gridCont.convertToTileCoord (camPos.x);
		int clickedJ = gridCont.convertToTileCoord (camPos.y);
		if (gridCont.onGrid(clickedI, clickedJ)){
			tileStuff tileScript = tiles [clickedI, clickedJ].GetComponent<tileStuff>();
			deleteLight.transform.position = new Vector2 (clickedI, clickedJ);
			inkLeft += tileScript.deleteAllCats (elevInkCost, attackInkCost);
			Debug.Log ("inkLeft: " + inkLeft);
			if (clickedI == boyTileI && clickedJ == boyTileJ) {
				int lowerJ = boyTileJ - 1;
				if (gridCont.onGrid (clickedI, lowerJ)) {
					tileStuff belowTile = tiles [clickedI, lowerJ].GetComponent<tileStuff> ();
					if (tileScript.getElevCat () == null && !belowTile.getIsPlatform()) {
						floating = true;
						dest = new Vector3 (clickedI, lowerJ, 0f);
						Debug.Log ("new dest gotta float no more elev cat");
					}
				}
			}
		}
	}

	public void spawnElevCat(){
		bool cannotSpawn = false;
		int nextInkLeft = inkLeft - elevInkCost;
		if (nextInkLeft <= 0) {
			Debug.Log ("no more ink");
			return;
		}
		tileStuff tileScript = tiles [boyTileI, boyTileJ].GetComponent<tileStuff> ();
		if (reachedDestination () && !tileScript.getIsPlatform ()) {
			if (tileScript.getElevCat () == null) {
				tileScript.placeCat (elevCat, elevCatPrefab, gridCont.tileSize);
				inkLeft = nextInkLeft;
			} else {
				cannotSpawn = true;
			}
		} else {
			cannotSpawn = true;
		}
		if (cannotSpawn) {
			Debug.Log ("cannot spawn, show animation or sound as to why cannot spawn");
		}
	}

	public void spawnAttackCat(int ac){
		int nextInkLeft = inkLeft - attackInkCost;
		if (nextInkLeft <= 0) {
			Debug.Log ("no more ink");
			return;
		}
		attackCat = ac;
		bool cannotSpawn = false;
		tileStuff tileScript = tiles [boyTileI, boyTileJ].GetComponent<tileStuff> ();
		if (reachedDestination () && !tileScript.getIsPlatform ()) {
			tileScript.placeCat (attackCat, attackCatPrefab, gridCont.tileSize);
			inkLeft = nextInkLeft;
		} else {
			cannotSpawn = true;
		}
		if (cannotSpawn) {
			Debug.Log ("cannot spawn, show animation or sound as to why cannot spawn");
		}
	}

	private void checkNewPos(){
//		newDestIsValid = false;
		tileStuff tileScript;
		int i = gridCont.convertToTileCoord (camPos.x);
		int j = gridCont.convertToTileCoord (transform.position.y);
		int checkVertj = gridCont.convertToTileCoord(camPos.y);
		tileScript = tiles [i, checkVertj].GetComponent<tileStuff> ();
		bool moveElevCat = false;
//		if (!tileScript.hasACat ()) { //don't want player to move if despawning a cat;
//		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down, gridCont.tileSize, elevCatLayer);
//		if (hit.collider != null){
		GameObject elevCatObj = tiles[boyTileI, boyTileJ].GetComponent<tileStuff>().getElevCat();
		if (elevCatObj != null) {
			//check vertical movement
			if (i == boyTileI && checkVertj != boyTileJ) {
				Debug.Log ("i : " + i + " checkVertJ: " + checkVertj + " boyTileI: " + boyTileI + " boyTileJ: " + boyTileJ);
				//player is trying to move up or down with elev cat
				Debug.Log("player trying to move up");
				elevCatControl ecc = elevCatObj.transform.GetComponent<elevCatControl> ();
				if (!ecc.moving) {
					Debug.Log ("cat not moving rn");
					j = checkVertj;
					currRidingCat = elevCatObj.transform;
					currRidingCat.SetParent (transform);
					elevCatMaxJ = ecc.maxJ;
					elevCatMinJ = ecc.minJ;
					usingElevator = true;
				} else {
					Debug.Log("cat still moving hold up");
					usingElevator = false;
					if (currRidingCat != null) {
						currRidingCat.parent = null;
						currRidingCat = null;
					}
				}
			} else {
				usingElevator = false;
				if (currRidingCat != null) {
					currRidingCat.parent = null;
					currRidingCat = null;
				}
			}
		} else {
			usingElevator = false;
			if (currRidingCat != null) {
				currRidingCat.parent = null;
				currRidingCat = null;
			}
			moveElevCat = searchingForElevCatToMoveToClick (i, checkVertj);
		}
		if (!moveElevCat) {
//			Debug.Log ("xPos: " + i + " yPos: " + j);
			Vector3 potential = new Vector3 (i, j, 0);
			if (boyTileI == i) {
				if (usingElevator) {
					goingUp = (j > boyTileJ);
				}
				if (boyTileJ == checkVertj) {
					facingRight = !facingRight;
				}
			} else {
				facingRight = (i > boyTileI);
			}
			dest = potential;
//			Debug.Log ("dest: " + dest + " curr:" + transform.position);
		}
		highlight.transform.position = new Vector3 (gridCont.convertToTileCoord (camPos.x), gridCont.convertToTileCoord (camPos.y), 0f);
	}

	private bool searchingForElevCatToMoveToClick(int clickedI, int clickedJ){
		if (clickedI == boyTileI) {
			//vertical
			int checkJ = boyTileJ;
			tileStuff tileScript;
			GameObject elevCatObj;
			while (gridCont.onGrid (clickedI, checkJ)) {
				tileScript = tiles [clickedI, checkJ].GetComponent<tileStuff> ();
				elevCatObj = tileScript.getElevCat ();
				if (elevCatObj != null) {
					elevCatControl ecc = elevCatObj.transform.GetComponent<elevCatControl> ();
					int jToMoveTo = clickedJ;
					if (clickedJ > ecc.maxJ) {
						jToMoveTo = ecc.maxJ;
					} else if (clickedJ < ecc.minJ) {
						jToMoveTo = ecc.minJ;
					}
					int elevCatMovingToJ = ecc.moveTo (clickedI, jToMoveTo);
					return true;
				}
				checkJ++;
			}
		}
		return false;
	}

	private void checkDestination (){
		if (!floating && !falling) {
			int nextI = boyTileI;
			int nextJ = boyTileJ;
			if (!usingElevator) {
				if (facingRight) {
					nextI++;
				} else {
					nextI--;
				}
			} else {
				nextI = boyTileI;
				if (goingUp) {
					nextJ++;
				} else {
//				nextJ -= 2;
					nextJ--;
				}
			}
			if (!destIsValidTile (nextI, nextJ)) {
				dest = new Vector3 (boyTileI, boyTileJ, 0f);
			} else {
//				newDestIsValid = true;
			}
		}
	}

	private bool setFallingDest(){
		int i = boyTileI;
		int j = boyTileJ;
		Debug.Log (boyTileI + " " + i + " " + j + " falling");
		bool foundPlat = false;
		tileStuff tileScript;
		while (!foundPlat) {
			if (!gridCont.onGrid (i, j)) {
				return false;
			}
			tileScript = tiles [i, j].GetComponent<tileStuff> ();
			if (tileScript.getElevCat () != null) {
				dest = new Vector3 (i, j, 0f);
				Debug.Log ("found falling dest on cat: " + dest);
				return true;
			} else if (tileScript.getIsPlatform ()) {
				dest = new Vector3 (i, j + 1, 0f);
				Debug.Log ("found falling dest on plat: " + dest);
				return true;
			}
			j--;
		}
		return true;
	}

	private bool destIsValidTile(int nextI, int nextJ){
		if (!gridCont.onGrid (nextI, nextJ)) {
			return false;
		}
		tileStuff tileScript = tiles [nextI, nextJ].GetComponent<tileStuff> ();
		if (!usingElevator) {
			if (tileScript.getIsPlatform ()) {
				return false;
			}
			if (tileScript.getElevCat () != null) {
				return true;
			}
			int checkIsPlatJ = nextJ - 1;
			if (gridCont.onGrid (nextI, checkIsPlatJ)) {
				tileScript = tiles [nextI, checkIsPlatJ].GetComponent<tileStuff> ();
				if (!tileScript.getIsPlatform ()) {
					dest = new Vector3 (nextI, nextJ, 0f);
//					setFallingDest (nextI, checkIsPlatJ);
					floating = true;
					return true;
				}
			} else {
				return false;
			}
			return true;
		} else {
			if (nextJ > elevCatMaxJ || nextJ < elevCatMinJ) {
				return false;
			}
			//fixme use an elev cat counter?
			if (tileScript.getIsPlatform ()) {
				return false;
			}
			GameObject elevCatObj = tileScript.getElevCat ();
			if (elevCatObj != null && !elevCatObj.Equals(currRidingCat.gameObject)) {
				return false;
			}
			return true;
		}
	}

}
