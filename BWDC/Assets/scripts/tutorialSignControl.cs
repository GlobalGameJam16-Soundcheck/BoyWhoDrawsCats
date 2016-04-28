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

	private int att;//index to signs
	private int yarn;
	private int elev;
	private int tap;

	// Use this for initialization
	void Start () {
		att = 3;
		yarn = 2;
		elev = 1;
		tap = 0;
		sceneIndex = SceneManager.GetActiveScene().buildIndex;
		gridCont = Camera.main.GetComponent<gridGrabber>().returnGrid();
		for (int i = 0; i < signs.Length; i++) {
			SpriteRenderer sr = signs [i].GetComponent<SpriteRenderer> ();
			if (sceneIndex > gridCont.attackCatScene) {
				sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
			} else if (sceneIndex > gridCont.yarnCatScene) {
				if (i < att) {
					sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
				} else {
					sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0f);
				}
			} else if (sceneIndex > gridCont.elevCatScene) {
				if (i < yarn) {
					sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
				} else {
					sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0f);
				}
			} else if (sceneIndex > gridCont.tapScene) {
				if (i < elev) {
					sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
				} else {
					sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0f);
				}
			} else {
				sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0f);
			}
		}
		tutAccomplished = false;
		velo = 5f;
		destroyed = false;
	}

	public void setSign(){
		int i = tap;
		if (sceneIndex == gridCont.tapScene) {
			i = tap;
		} else if (sceneIndex == gridCont.elevCatScene) {
			i = elev;
		} else if (sceneIndex == gridCont.yarnCatScene) {
			i = yarn;
		} else if (sceneIndex == gridCont.attackCatScene) {
			i = att;
		}
		SpriteRenderer sr = signs [i].GetComponent<SpriteRenderer> ();
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
		if (sceneIndex == gridCont.elevCatScene && otherTap != null) {
			sr = otherTap.GetComponent<SpriteRenderer> ();
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
		}
	}

	public void setTutAccomplished(bool a){
		tutAccomplished = a;
		if (!destroyed) {
			SpriteRenderer sr = tutSign.GetComponent<SpriteRenderer> ();
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
		}
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
