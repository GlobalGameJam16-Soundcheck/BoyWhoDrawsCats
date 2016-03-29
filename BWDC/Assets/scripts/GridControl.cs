using UnityEngine;
using System.Collections;

public class GridControl : MonoBehaviour {

    int tilesWidth;
    int tilesHeight;
    string[,] tiles;
    public GameObject tile;
    public TextAsset levelCsv;

    // Use this for initialization
    void Start () {

       // TextAsset levelCsv = (TextAsset)Resources.Load("level", typeof(TextAsset));

        string[,] tempTiles = CSVReader.SplitCsvGrid(levelCsv.text);
        tilesWidth = tempTiles.GetLength(0)-2;
        tilesHeight = tempTiles.GetLength(1)-2;
        tiles = new string[tilesWidth, tilesHeight];
        for (int x = 0; x < tilesWidth; x++)
        {
            for(int y = 0; y < tilesHeight; y++)
            {
                tiles[x, y] = tempTiles[x, tilesHeight-1 - y];
                if (tiles[x,y] == "B")
                {
                   Instantiate(tile, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
