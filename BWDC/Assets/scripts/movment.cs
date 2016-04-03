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

	public int elevCat = 1;

	// Use this for initialization
	void Start () {
        dest = transform.position;
		gridCont = Camera.main.GetComponent<GridControl>();
		tiles = gridCont.tiles;
		tileWidth = tiles.GetLength (0);
		tileHeight = tiles.GetLength (1);
    }
	
	// Update is called once per frame
	void Update () {
		camPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(dest.x, transform.position.y, 0), walkSpeed);
		boyTileI = convertToTileCoord (transform.position.x);
		boyTileJ = convertToTileCoord (transform.position.y);
		if (Input.GetMouseButtonDown (0)) {
			int mouseI = convertToTileCoord (camPos.x);
			int mouseJ = convertToTileCoord (camPos.y);
			tileStuff tileScript = tiles [mouseI, mouseJ].GetComponent<tileStuff> ();
			if (tileScript.getCanRemoveCat ()) {
				tileScript.removePlacedCat ();
			}
		}
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
		int i = convertToTileCoord (dest.x);
		int j = convertToTileCoord (dest.y);
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
			tileStuff tileScript = tiles [nextI, nextJ].GetComponent<tileStuff> ();
			if (reachedDestination() && tileScript.canPlacePlatCat ()) {
				tileScript.placeCat (elevCat);
			}
		}
	}

	private void checkNewPos(){
		newDestIsValid = false;
		int i = convertToTileCoord (camPos.x);
		int j = convertToTileCoord (transform.position.y);
		Debug.Log ("xPos: " + i + " yPos: " + j);
		Vector3 potential = new Vector3 (i, j, 0);
		if (boyTileI == i){
			facingRight = !facingRight;
		} else {
			facingRight = (i > boyTileI);
		}
		tileStuff tileScript = tiles [i, j].GetComponent<tileStuff> ();
		if (!tileScript.getCanRemoveCat ()) {
			dest = potential;
			highlight.transform.position = new Vector3 (convertToTileCoord (camPos.x), convertToTileCoord (camPos.y), 0f);
		}
	}

	private void checkDestination (){
		if (reachedDestination()){
			newDestIsValid = true;
			return;
		}
		int nextI = boyTileI;
		int nextJ = boyTileJ;
		if (facingRight) {
			nextI++;
		} else {
			nextI--;
		}
		if (!destIsValidTile (nextI, nextJ)) {
			dest = new Vector3 ((float)convertToTileCoord (transform.position.x),
				(float)convertToTileCoord (transform.position.y), 0f);
		} else {
			newDestIsValid = true;
		}
	}

	private bool destIsValidTile(int i, int j){
		if (!onGrid (i, j)) {
			return false;
		}
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
	}

	private bool onGrid(int i, int j){
		return (0 <= i && 0 <= j && i < tileWidth && j < tileHeight);
	}

	private int convertToTileCoord(float x){
		return (int)Mathf.Round (x);
	}

}
