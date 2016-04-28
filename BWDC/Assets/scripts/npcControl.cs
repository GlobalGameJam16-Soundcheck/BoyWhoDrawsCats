using UnityEngine;
using System.Collections;

public class npcControl : MonoBehaviour {

	public GameObject textBubble;
	private SpriteRenderer textSR;
	private GridControl gridCont;
	private GameObject[,] tiles;
	private int tileI;
	private int tileJ;
	private tileStuff left;
	private tileStuff right;
	private tileStuff thisTile;

	// Use this for initialization
	void Start () {
		Invoke ("delayedStart", 1f);
	}

	private void delayedStart(){
		textSR = textBubble.GetComponent<SpriteRenderer> ();
		gridCont = Camera.main.GetComponent<gridGrabber>().returnGrid();
		tiles = gridCont.tiles;
		tileI = gridCont.convertToTileCoord (transform.position.x);
		tileJ = gridCont.convertToTileCoord (transform.position.y);
		left = null;
		if (gridCont.onGrid (tileI - 1, tileJ)) {
			left = tiles [tileI - 1, tileJ].GetComponent<tileStuff> ();
		}
		right = null;
		if (gridCont.onGrid (tileI + 1, tileJ)) {
			right = tiles [tileI + 1, tileJ].GetComponent<tileStuff> ();
		}
		thisTile = null;
		if (gridCont.onGrid (tileI, tileJ)) {
			thisTile = tiles [tileI, tileJ].GetComponent<tileStuff> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (textSR != null) {
			if ((left != null && left.getBoyTile () != null) || (right != null && right.getBoyTile () != null) ||
				(thisTile != null && thisTile.getBoyTile() != null)) {
				textSR.color = new Color (textSR.color.r, textSR.color.g, textSR.color.b, 1f);
			} else {
				textSR.color = new Color (textSR.color.r, textSR.color.g, textSR.color.b, 0f);
			}
		}
	}
}
