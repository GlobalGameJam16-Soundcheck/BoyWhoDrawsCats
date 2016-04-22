using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class tutorialSignControl : MonoBehaviour {

	public GameObject[] signs;
	private int sceneIndex;
	public GameObject otherTap;
	private GridControl gridCont;

	// Use this for initialization
	void Start () {
		sceneIndex = SceneManager.GetActiveScene().buildIndex;
		for (int i = 0; i < signs.Length; i++) {
			SpriteRenderer sr = signs [i].GetComponent<SpriteRenderer> ();
			if (i == sceneIndex) {
				sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0.5f);
			} else if (i < sceneIndex) {
				sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
			} else {
				sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0f);
			}
		}
		gridCont = Camera.main.GetComponent<GridControl>();
	}
	
	// Update is called once per frame
	public void setSign(){
		SpriteRenderer sr = signs [sceneIndex].GetComponent<SpriteRenderer> ();
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
		if (sceneIndex == gridCont.elevCatScene && otherTap != null) {
			sr = otherTap.GetComponent<SpriteRenderer> ();
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
		}
	}
//	void Update () {
//		
//	}
}
