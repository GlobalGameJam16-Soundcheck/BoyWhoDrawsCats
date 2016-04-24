using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class tutorialSignControl : MonoBehaviour {

	public GameObject[] signs;
	private int sceneIndex;
	public GameObject otherTap;
	private GridControl gridCont;

	public GameObject tutSign;
	public Vector3 tutSignPos;
	public Vector3 tutSignScale;
	private bool tutAccomplished;
	private float velo;
	private bool destroyed;

	public GameObject tutStroke;

	// Use this for initialization
	void Start () {
		sceneIndex = SceneManager.GetActiveScene().buildIndex;
		for (int i = 0; i < signs.Length; i++) {
			SpriteRenderer sr = signs [i].GetComponent<SpriteRenderer> ();
			if (i < sceneIndex || sceneIndex == -1) {
				sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
			} else {
				sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0f);
			}
		}
		gridCont = Camera.main.GetComponent<GridControl>();
		tutAccomplished = false;
		velo = 5f;
		destroyed = false;
	}

	public void setSign(){
		SpriteRenderer sr = signs [sceneIndex].GetComponent<SpriteRenderer> ();
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
		if (sceneIndex == gridCont.elevCatScene && otherTap != null) {
			sr = otherTap.GetComponent<SpriteRenderer> ();
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
		}
	}

	public void setTutAccomplished(bool a){
		tutAccomplished = a;
		if (tutStroke != null) {
			Destroy (tutStroke);
		}
	}

	public void destroyOtherTap(){
		if (otherTap != null) {
			Destroy (otherTap);
		}
	}

	void Update(){
		if (tutAccomplished && !destroyed) {
			float epsilon = 0.5f;
			if (Vector2.Distance (tutSign.transform.position, tutSignPos) > epsilon) {
				Vector3 dir = tutSignPos - tutSign.transform.position;
				dir = dir.normalized * velo;
				tutSign.transform.position = tutSign.transform.position + dir * Time.deltaTime;
			} else {
				Destroy (tutSign);
				destroyed = true;
				setSign ();
			}
		}
	}


}
