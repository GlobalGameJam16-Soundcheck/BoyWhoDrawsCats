using UnityEngine;
using System.Collections;

public class fadeBehavior : MonoBehaviour {

	private SpriteRenderer sr;
	public bool startFading { get; set; }
	public float timeToFade { get; set; }
	private float startAlpha;
	private float targetAlpha;
	private float flag;

	// Use this for initialization
	void Start () {
		sr = transform.gameObject.GetComponent<SpriteRenderer> ();
		startFading = false;
		timeToFade = 3f;
		startAlpha = 255f;
		flag = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (startFading) {
			float ratio = Time.deltaTime / timeToFade;
			float a = Mathf.Abs(startAlpha - targetAlpha) * ratio;
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, sr.color.a - flag * a / 255f);
		}
	}

	public void fadeIn(){
		flag *= -1f;
		startAlpha = 0f;
		targetAlpha = 255f;
	}
}
