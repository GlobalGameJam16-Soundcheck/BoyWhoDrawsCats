using UnityEngine;
using System.Collections;

public class movment : MonoBehaviour {

	//note all the i's and j's are really x and y coordinates

    public GameObject highlight;
	private SpriteRenderer mySprite;
	public Sprite leftSprite;
	private Sprite rightSprite;


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

	public GameObject elevCatPrefab;
	private bool usingElevator = false;
	private bool falling = false;
	private bool floating = true;
	private bool goingUp;
	private int elevCatLayer;
	private int elevCatMaxJ;
	private int elevCatMinJ;
	public int elevCat = 1;
	private Transform currRidingCat = null;

	private float origReachedDestTime = 1f;
	private float reachedDestTime = 1f;

	// Use this for initialization
	void Start () {
		gridCont = Camera.main.GetComponent<GridControl>();
		elevCatLayer = 1 << (LayerMask.NameToLayer ("elevCat"));
		tiles = gridCont.tiles;
		tileWidth = tiles.GetLength (0);
		tileHeight = tiles.GetLength (1);
		int xPos = gridCont.convertToTileCoord(transform.position.x);
		int yPos = gridCont.convertToTileCoord(transform.position.y);
		dest = new Vector3 (xPos, yPos, 0f);
		mySprite = GetComponent<SpriteRenderer> ();
		rightSprite = mySprite.sprite;
    }
	
	// Update is called once per frame
	void Update () {
		changeSprite ();
		camPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(dest.x, dest.y, 0), walkSpeed);
		boyTileI = gridCont.convertToTileCoord (transform.position.x);
		boyTileJ = gridCont.convertToTileCoord (transform.position.y);
//		Debug.Log("bti: " + boyTileI + " btj: " + boyTileJ);
//		if (Input.GetMouseButtonDown (0)) {
//			int mouseI = convertToTileCoord (camPos.x);
//			int mouseJ = convertToTileCoord (camPos.y);
//			tileStuff tileScript = tiles [mouseI, mouseJ].GetComponent<tileStuff> ();
//			if (tileScript.getCanRemoveCat ()) {
//				tileScript.removePlacedCat ();
//			}
//		}
        if (Input.GetMouseButtonUp(0))
        {
			if ((!falling && !floating) || (reachedDestination() && usingElevator)) {
				checkNewPos ();
			}
        }
		if (!reachedDestination ()) {// || !newDestIsValid) {
			checkDestination ();
		} else {
			if (falling) {
				setFallingDest ();
			}
		}
		checkSpawningCats ();
    }

	private void changeSprite(){
//		if (facingRight) {
//			mySprite.sprite = rightSprite;
//		} else {
//			mySprite.sprite = leftSprite;
//		}
	}

	private bool reachedDestination(){
		int i = gridCont.convertToTileCoord (dest.x);
		int j = gridCont.convertToTileCoord (dest.y);
		bool ret = (boyTileI == i && boyTileJ == j);
		if (ret) {
//			reachedDestTime -= Time.deltaTime;
//			if (reachedDestTime > 0f) {
//				return false;
//			}
//			reachedDestTime = origReachedDestTime;
			if (floating) {
				falling = true;
				floating = false;
			} else if (falling) {
				falling = false;
			}
		}
		return ret;
	}

	private void checkSpawningCats(){
		int nextI = boyTileI;// - 1;
		int nextJ = boyTileJ;
//		if (facingRight) {
//			nextI = boyTileI + 1;
//		}
		//a platform cat?
		if (Input.GetKeyUp ("1") || Input.GetMouseButtonUp(1)) {
//			nextJ--;
			if (onGrid (nextI, nextJ - 1)) {
				tileStuff tileScript = tiles [nextI, nextJ].GetComponent<tileStuff> ();
				tileStuff belowTile = tiles [nextI, nextJ - 1].GetComponent < tileStuff> ();
				if (reachedDestination () && !belowTile.getHasElevCat() && !tileScript.getIsPlatform() && !tileScript.getHasElevCat()) {
					tileScript.placeCat (elevCat, elevCatPrefab, gridCont.tileSize);
				}
			}
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
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down, gridCont.tileSize, elevCatLayer);
		if (hit.collider != null){
			//check vertical movement
			if (i == boyTileI && checkVertj != boyTileJ) {
				Debug.Log ("i : " + i + " checkVertJ: " + checkVertj + " boyTileI: " + boyTileI + " boyTileJ: " + boyTileJ);
				//player is trying to move up or down with elev cat
				Debug.Log("player trying to move up");
				j = checkVertj;
				currRidingCat = hit.transform;
				currRidingCat.SetParent (transform);
				elevCatControl ecc = hit.transform.GetComponent<elevCatControl> ();
				elevCatMaxJ = ecc.maxJ;
				elevCatMinJ = ecc.minJ;
				usingElevator = true;
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
			Debug.Log ("xPos: " + i + " yPos: " + j);
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
			Debug.Log ("dest: " + dest + " curr:" + transform.position);
		}
		highlight.transform.position = new Vector3 (gridCont.convertToTileCoord (camPos.x), gridCont.convertToTileCoord (camPos.y), 0f);
	}

	private bool searchingForElevCatToMoveToClick(int clickedI, int clickedJ){
		if (clickedI == boyTileI) {
			//vertical
			RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.up, Mathf.Infinity, elevCatLayer);
			if (hit != null) {
				Debug.Log ("fuck you: " + hit);
//				Debug.Break ();
				elevCatControl ecc = hit.transform.GetComponent<elevCatControl> ();
				if (clickedJ <= ecc.maxJ && clickedJ >= ecc.minJ) {
					ecc.moveTo (clickedI, clickedJ);
					return true;
				}
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
			if (!onGrid (i, j)) {
				return false;
			}
			tileScript = tiles [i, j].GetComponent<tileStuff> ();
			if (tileScript.getHasElevCat ()) {
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
		if (!onGrid (nextI, nextJ)) {
			return false;
		}
		tileStuff tileScript = tiles [nextI, nextJ].GetComponent<tileStuff> ();
		if (!usingElevator) {
			if (tileScript.getIsPlatform ()) {
				return false;
			}
			if (tileScript.getHasElevCat ()) {
				return true;
			}
			int checkIsPlatJ = nextJ - 1;
			if (onGrid (nextI, checkIsPlatJ)) {
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
			return true;
		}
	}

	private bool onGrid(int i, int j){
		return (0 <= i && 0 <= j && i < tileWidth && j < tileHeight);
	}

}
