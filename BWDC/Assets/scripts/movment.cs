using UnityEngine;
using System.Collections;

public class movment : MonoBehaviour {
    public GameObject heighlight;
    Vector3 dest;
    float walkSpeed = 0.2f;
    GridControl grid;

	// Use this for initialization
	void Start () {
        dest = transform.position;
    }

    void Awake()
    {
       grid = Camera.main.GetComponent<GridControl>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            dest = new Vector3 (Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y), 0);
            heighlight.transform.position = dest;
        }
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(dest.x, transform.position.y, 0), walkSpeed);
        int boyTileX = (int)Mathf.Round(transform.position.x);
        int boyTileY = (int)Mathf.Round(transform.position.y);
        if (grid.getTile(boyTileX, boyTileY) == "Block")
        {
            dest = transform.position;
        }
        grid.setTile(boyTileX, boyTileY, "boy");
    }
}
