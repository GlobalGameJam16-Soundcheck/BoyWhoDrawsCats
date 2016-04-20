using UnityEngine;
using System.Collections;

public class penTipControl : MonoBehaviour {

	// Use this for initialization

	private SpriteRenderer mySprite;

	public void startTrail(){
		transform.position = new Vector2 (transform.position.x, transform.position.y);
		transform.gameObject.SetActive (true);
		Destroy (transform.gameObject, 1f);
		mySprite = GetComponent<SpriteRenderer> ();
		transform.Rotate (new Vector3 (0f, 0f, 45f));
	}
//	void Start () {
//		
//	}
	
	// Update is called once per frame
	void Update () {
		mySprite.color = new Color (mySprite.color.r, mySprite.color.g, mySprite.color.b, 95f * mySprite.color.a / 100f);
	}
}
