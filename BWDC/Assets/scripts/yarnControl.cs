using UnityEngine;
using System.Collections;

public class yarnControl : MonoBehaviour {

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
	private bool doorsActivated;

	private float timer;
	private bool one = false;
	private bool two = false;

	// Use this for initialization
	void Start () {
		started = false;
		Invoke ("delayedStart", 0f);
	}

	private void delayedStart(){
		started = true;
		gridCont = Camera.main.GetComponent<GridControl>();
		tiles = gridCont.tiles;
		mySprite = GetComponent<SpriteRenderer> ();
		mySprite.color = startColor;
		tileScript = tiles [tileI, tileJ].GetComponent<tileStuff>();
		doorsActivated = true;
	}

	public void initialize(int i, int j, Color c, int v){
		tileI = i;
		tileJ = j;
		startColor = c;
		val = v;
		Debug.Log ("init");
	}
	
	// Update is called once per frame
	void Update () {
		if (started) {
			timer += Time.deltaTime;
			if (timer > 5f && !one) {
				activateDoors (false);
				one = true;
			} else if (timer > 10f && !two) {
				activateDoors (true);
				two = true;
			}
		}
	}

	public void activateDoors(bool activate){
		gridCont.setDoors (activate, val);
		doorsActivated = activate;
	}
}
