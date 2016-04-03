using UnityEngine;
using System.Collections;

public class movment : MonoBehaviour {
	
    public GameObject highlight;
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
	public int elevCat = 1;

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
    }
	
	// Update is called once per frame
	void Update () {
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
		if (Input.GetKeyUp ("1")) {
			nextJ--;
			tileStuff tileScript = tiles [nextI, nextJ].GetComponent<tileStuff> ();
			if (reachedDestination() && tileScript.canPlacePlatCat ()) {
				tileScript.placeCat (elevCat, elevCatPrefab, tileSize);
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
				usingElevator = true;
				hit.transform.SetParent (transform);
			} else {
				usingElevator = false;
			}
		} else {
			usingElevator = false;
		}
		Debug.Log ("xPos: " + i + " yPos: " + j);
		Vector3 potential = new Vector3 (i, j, 0);
		if (boyTileI == i) {
			if (!usingElevator) {
				facingRight = !facingRight;
			} else {
				goingUp = (j > boyTileJ);
			}
		} else {
			facingRight = (i > boyTileI);
		}
		dest = potential;
		Debug.Log (dest + " " + transform.position);
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
				nextJ--;
			}
//			Debug.Log (nextI + " " + nextJ + " nextI and nextJ going Up");
		}
		if (!destIsValidTile (nextI, nextJ)) {
			dest = new Vector3 ((float)gridCont.convertToTileCoord (transform.position.x),
				(float)gridCont.convertToTileCoord (transform.position.y), 0f);
		} else {
			newDestIsValid = true;
		}
	}

	private bool destIsValidTile(int i, int j){
		if (!onGrid (i, j)) {
			return false;
		}
		if (!usingElevator) {
			tileStuff tileScript = tiles [i, j].GetComponent<tileStuff> ();
			if (tileScript.getIsPlatform ()) {
				return false;
			}
			int nextI = boyTileI;
			int nextJ = boyTileJ - 1;
			if (facingRight) {
				nextI++;
			} else {
				nextI--;
			}
			if (onGrid (nextI, nextJ)) {
				tileScript = tiles [nextI, nextJ].GetComponent<tileStuff> ();
				if (!tileScript.canStandOn ()) {
					Debug.Log (nextI + " " + nextJ + " cannotStandOn");
					return false;
				}
			}
			return true;
		} else {
			return true;
		}
	}

	private bool onGrid(int i, int j){
		return (0 <= i && 0 <= j && i < tileWidth && j < tileHeight);
	}

}
