using UnityEngine;
using System.Collections;

public class outlineCatControl : MonoBehaviour {

	private int boyTileI;
	private int boyTileJ;
	private bool setBoyTiles = false;
	private Vector3 destScale = new Vector3(1f, 1f, 1f);
	private float xScaleDiff;
	private float yScaleDiff;
	private float velo = 15f;
	private float initDistance = 10f;

	// Use this for initialization
	void Start () {
		xScaleDiff = transform.localScale.x - 1f;
		yScaleDiff = transform.localScale.y - 1f;
	}

	public void setBoyTileIJ(int bi, int bj){
		boyTileI = bi;
		boyTileJ = bj;
		setBoyTiles = true;
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
		initDistance = Vector2.Distance (transform.position, new Vector2 (bi, bj));
	}
	
	// Update is called once per frame
	void Update () {
		if (setBoyTiles) {
			float epsilon = 0.5f;
			Vector2 dest = new Vector2 (boyTileI, boyTileJ);
			float distance = Vector2.Distance (transform.position, dest);
			if (distance > epsilon) {
				Vector2 dir = dest - (Vector2)transform.position;
				dir = dir.normalized * velo;
				transform.position = (Vector2)transform.position + dir * Time.deltaTime;
				float xScale = xScaleDiff * (distance / initDistance);
				float yScale = yScaleDiff * (distance / initDistance);
				transform.localScale = new Vector3 (xScale, yScale, 1f);
			} else {
				Destroy (transform.gameObject);
			}
		}
	}
}
