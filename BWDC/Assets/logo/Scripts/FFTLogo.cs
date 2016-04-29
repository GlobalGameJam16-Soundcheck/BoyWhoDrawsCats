using UnityEngine;
using System.Collections;

public class FFTLogo : MonoBehaviour {

	public GameObject logo;
	private fadeBehavior fade;
	private float timer;
	private float delay;

	// Use this for initialization
	void Start () {
		fade = logo.GetComponent<fadeBehavior> ();
		timer = 0f;
		delay = 2f;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= delay && !fade.startFading) {
			fade.fadeIn ();
			fade.timeToFade = transform.GetChild (0).GetComponent<AudioSource> ().clip.length;
			fade.startFading = true;
		}
        if (Input.GetKeyDown("space") == true)
        {
            int i = Application.loadedLevel;
            Application.LoadLevel(i + 1);
        }
        if (Input.GetMouseButtonDown(0))
        {
            int i = Application.loadedLevel;
            Application.LoadLevel(i + 1);
        }
	}
}
