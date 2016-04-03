﻿using UnityEngine;
using System.Collections;

public class GridControl : MonoBehaviour {

    int tilesWidth;
    int tilesHeight;
//    string[,] tiles;
	public GameObject [,] tiles { get; set; }
    public GameObject tile;
	private SpriteRenderer tileSpriteRenderer;
	private Sprite blockSprite;
	public Sprite blankTile;
    public TextAsset levelCsv;

    // Use this for initialization
    void Start () {

       // TextAsset levelCsv = (TextAsset)Resources.Load("level", typeof(TextAsset));
		tileSpriteRenderer = tile.GetComponent<SpriteRenderer> ();
		blockSprite = tileSpriteRenderer.sprite;

        string[,] tempTiles = CSVReader.SplitCsvGrid(levelCsv.text);
        tilesWidth = tempTiles.GetLength(0);
        tilesHeight = tempTiles.GetLength(1);
		tiles = new GameObject[tilesWidth,tilesHeight];
        for (int x = 0; x < tilesWidth; x++) {
            for(int y = 0; y < tilesHeight; y++) {
				bool isPlatform = false;
				if (tempTiles[x, tilesHeight-1 - y] == "b") {
					tileSpriteRenderer.sprite = blockSprite;
					isPlatform = true;
				} else {
					tileSpriteRenderer.sprite = blankTile;
				}
				tiles [x, y] = (GameObject)(Instantiate (tile, new Vector3 (x, y, 0), Quaternion.identity));
				tiles [x, y].GetComponent<tileStuff> ().setIsPlatform (isPlatform);
            }
        }
		tileSpriteRenderer.sprite = blockSprite;
	}

	public int convertToTileCoord(float x){
		return (int)Mathf.Round (x);
	}

}
