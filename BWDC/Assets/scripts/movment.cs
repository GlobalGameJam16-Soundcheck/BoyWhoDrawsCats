using UnityEngine;
using System.Collections;

public class movment : MonoBehaviour {
	
    public GameObject highlight;
	private SpriteRenderer mySprite;
	public Sprite leftSprite;
	private Sprite rightSprite;


    Vector3 dest;
    float walkSpeed = 0.2f;
    GridControl gridCont;
	private GameObject[,] tiles;
	private float tileSize = 1f;
	private bool facingRight = true;
	private int boyTileI;
	private int boyTileJ;
	private int tileWidth;
	private int tileHeight;
	private bool newDestIsValid = true;
	private Vector3 camPos;

	public GameObject elevCatPrefab;
	private bool usingElevator = false;
	private bool goingUp;
	private int elevCatLayer;
	private int elevCatMaxJ;
	private int elevCatMinJ;
	public int elevCat = 1;
	private Transform currRidingCat = null;

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
		if (facingRight) {
			mySprite.sprite = rightSprite;
		} else {
			mySprite.sprite = leftSprite;
		}
		camPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(dest.x, dest.y, 0), walkSpeed);
		boyTileI = gridCont.convertToTileCoord (transform.position.x);
		boyTileJ = gridCont.convertToTileCoord (transform.position.y);
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
			checkNewPos ();
        }
		if (!newDestIsValid || !reachedDestination()) {
			checkDestination ();
		}
		checkSpawningCats ();
    }

	private bool reachedDestination(){
		int i = gridCont.convertToTileCoord (dest.x);
		int j = gridCont.convertToTileCoord (dest.y);
		return (boyTileI == i && boyTileJ == j);
	}

	private void checkSpawningCats(){
		int nextI = boyTileI - 1;
		int nextJ = boyTileJ;
		if (facingRight) {
			nextI = boyTileI + 1;
		}
		//a platform cat?
		if (Input.GetKeyUp ("1") || Input.GetMouseButtonUp(1)) {
//			nextJ--;
			if (onGrid (nextI, nextJ - 1)) {
				tileStuff tileScript = tiles [nextI, nextJ].GetComponent<tileStuff> ();
				tileStuff belowTile = tiles [nextI, nextJ - 1].GetComponent < tileStuff> ();
				if (reachedDestination () && !belowTile.getHasElevCat() && !tileScript.getIsPlatform() && !tileScript.getHasElevCat()) {
					tileScript.placeCat (elevCat, elevCatPrefab, tileSize);
				}
			}
		}
	}

	private void checkNewPos(){
		newDestIsValid = false;
		tileStuff tileScript;
		int i = gridCont.convertToTileCoord (camPos.x);
		int j = gridCont.convertToTileCoord (transform.position.y);
		int checkVertj = gridCont.convertToTileCoord(camPos.y);
		tileScript = tiles [i, checkVertj].GetComponent<tileStuff> ();
//		if (!tileScript.hasACat ()) { //don't want player to move if despawning a cat;
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down, tileSize, elevCatLayer);
		if (hit.collider != null){
			//check vertical movement
			if (i == boyTileI && checkVertj != boyTileJ) {
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
		}
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
		highlight.transform.position = new Vector3 (gridCont.convertToTileCoord (camPos.x), gridCont.convertToTileCoord (camPos.y), 0f);
//		}
	}

	private void checkDestination (){
		if (reachedDestination()){
			newDestIsValid = true;
			return;
		}
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
			newDestIsValid = true;
		}
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
					Debug.Log (nextI + " " + checkIsPlatJ + " cannotStandOn");
					return false;
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
