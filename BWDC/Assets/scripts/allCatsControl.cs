﻿using UnityEngine;
using System.Collections;

public class allCatsControl : MonoBehaviour {

	protected GridControl gridCont;
	protected GameObject[,] tiles;
	protected Vector2 tileSpot;
	protected int currI;
	protected int currJ;
	protected int newI;
	protected int newJ;
	protected float moveSpeed;

//	// Use this for initialization
	void Start () {
		initialize ();
	}

	protected virtual void initialize(){
		gridCont = Camera.main.GetComponent<GridControl>();
		tiles = gridCont.tiles;
		currI = gridCont.convertToTileCoord (transform.position.x);
		currJ = gridCont.convertToTileCoord (transform.position.y);
		newI = currI;
		newJ = currJ;
		moveSpeed = 0.07f;
	}
	
	// Update is called once per frame
	void Update () {
		updateTilePos ();
	}

	protected virtual void updateTilePos(){
		newI = gridCont.convertToTileCoord (transform.position.x);
		newJ = gridCont.convertToTileCoord (transform.position.y);
		if (newI != currI || newJ != currJ) {
			updateTiles ();
		}
	}

	protected virtual void updateTiles (){
		
	}

}
