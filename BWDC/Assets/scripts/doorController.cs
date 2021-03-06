﻿using UnityEngine;
using System.Collections;

public class doorController : MonoBehaviour {

	public int tileI { get; set; }
	public int tileJ { get; set; }
	private tileStuff tileScript;
	public int val { get; set; }
	public bool inactive { get; set; }
	public Color startColor { get; set; }
	private bool started;
	private GridControl gridCont;
	private GameObject[,] tiles;
	private SpriteRenderer mySprite;
	private int doorDamage;
	private int numOnYarn;
	private AudioSource mySource;
	public AudioClip webOn;
	public AudioClip webOff;

	// Use this for initialization
	void Start () {
		started = false;
		Invoke ("delayedStart", 0f);
	}

	private void delayedStart(){
		started = true;
		gridCont = Camera.main.GetComponent<gridGrabber>().returnGrid();
		tiles = gridCont.tiles;
		mySprite = GetComponent<SpriteRenderer> ();
		mySprite.color = startColor;
		tileScript = tiles [tileI, tileJ].GetComponent<tileStuff>();
		doorDamage = 1;
		numOnYarn = 1;
		setActive ();
		mySource = GetComponent<AudioSource> ();
	}

	public void initialize(int i, int j, Color c, int v){
		tileI = i;
		tileJ = j;
		startColor = c;
		val = v;
//		if (active == 1) {
//			numOnYarn = 1;
//			setActive ();
//		} else {
//			numOnYarn = 2;
//			setInactive ();
//		}
	}
	
	// Update is called once per frame
	void Update () {
		if (started) {
			
		}
	}

	public void setActive(){
		if (numOnYarn > 0) {
			numOnYarn--;
		}
		Debug.Log("numOnYarn: " + numOnYarn);
		if (numOnYarn == 0) {
			if (mySource != null) {
				mySource.clip = webOn;
				mySource.Play ();
			}
			inactive = false;
			mySprite.color = new Color (mySprite.color.r, mySprite.color.g, mySprite.color.b, 1f);
			tileScript.setDoor (true);
		}
	}

	public void setInactive(){
		numOnYarn++;
		Debug.Log("numOnYarn: " + numOnYarn);
		if (numOnYarn > 0) {
			if (mySource != null) {
				mySource.clip = webOff;
				mySource.Play ();
			}
			inactive = true;
			mySprite.color = new Color (mySprite.color.r, mySprite.color.g, mySprite.color.b, 0.2f);
			tileScript.setDoor (false);
			int aboveTileJ = tileJ + 1;
			if (gridCont.onGrid (tileI, aboveTileJ)) {
				tileStuff aboveTile = tiles [tileI, aboveTileJ].GetComponent<tileStuff> ();
				aboveTile.checkIfBoyFallingCusBelowDoorInactive ();
			}
		}
	}

}
