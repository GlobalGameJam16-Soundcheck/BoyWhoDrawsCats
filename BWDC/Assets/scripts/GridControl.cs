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
        tilesWidth = tempTiles.GetLength(0);
        tilesHeight = tempTiles.GetLength(1);
        tiles = new string[tilesWidth, tilesHeight];
        for (int x = 0; x < tilesWidth; x++)
        {
            for(int y = 0; y < tilesHeight; y++)
            {
                tiles[x, y] = tempTiles[x, tilesHeight-1 - y];
                if (tiles[x,y] == "block")
                {
                   Instantiate(tile, new Vector3(x, y-1, 0), Quaternion.identity);
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    //-------------------
    public void setTile(int x, int y, string s)
    {
        tiles[x, y] = s;
    }
    //-------------------
    public string getTile(int x, int y)
    {
        return tiles[x, y];
    }
}
