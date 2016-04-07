using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GridControl : MonoBehaviour {

    int tileWidth;
    int tileHeight;
//    string[,] tiles;
	public GameObject [,] tiles { get; set; }
    public GameObject tile;
	private Sprite blockSprite;
	public GameObject blankTile;
    public TextAsset levelCsv;
	public float tileSize { get; set; }

    // Use this for initialization
    void Start () {
		tileSize = 1f;

       // TextAsset levelCsv = (TextAsset)Resources.Load("level", typeof(TextAsset));

        string[,] tempTiles = CSVReader.SplitCsvGrid(levelCsv.text);
        tileWidth = tempTiles.GetLength(0);
        tileHeight = tempTiles.GetLength(1) - 1; //get rid of the newlines
		tiles = new GameObject[tileWidth,tileHeight];
        for (int x = 0; x < tileWidth; x++) {
            for(int y = 0; y < tileHeight; y++) {
				bool isPlatform = false;
				if (tempTiles[x, tileHeight-1 - y] == "b") {
					isPlatform = true;
					tiles [x, y] = (GameObject)(Instantiate (tile, new Vector3 (x, y, 0), Quaternion.identity));
					if (y == 0) {
						Debug.Log ("y is 0, not blanks");
					}
				} else {
					tiles [x, y] = (GameObject)(Instantiate (blankTile, new Vector3 (x, y, 0), Quaternion.identity));
					if (y == 0) {
						Debug.Log ("y is 0, blank tile");
					}
				}

				tiles [x, y].GetComponent<tileStuff> ().setIsPlatform (isPlatform);
            }
        }
	}

	void Update(){
		if (Input.GetKeyDown ("r")) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}

	public int convertToTileCoord(float x){
		return (int)Mathf.Round (x);
	}

	public bool onGrid(int i, int j){
		return (0 <= i && 0 <= j && i < tileWidth && j < tileHeight);
	}

}
