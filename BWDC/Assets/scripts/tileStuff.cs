using UnityEngine;
using System.Collections;

public class tileStuff : MonoBehaviour {

	public bool isPlatform = false;
	public bool canRemoveCat = false;

//	public bool hasElevCat = false;
	public GameObject elevCatObj { get; set; }
	public int elevCat = 1; //?

	public void setIsPlatform(bool isPlat){
		isPlatform = isPlat;
	}

	public bool getIsPlatform(){
		return isPlatform;
	}

//	public bool canStandOn(){
//		return isPlatform || hasElevCat;
//	}

//	public bool canPlacePlatCat(){
//		return isPlatform && !hasElevCat;
//	}

	public void placeCat(int type, GameObject cat, float tileSize){
		if (type == elevCat) {
//			hasElevCat = true;
			float x = transform.position.x;
			float y = transform.position.y;
			elevCatObj = (GameObject)(Instantiate (cat, new Vector3 (x, y - tileSize / 3, 0), Quaternion.identity));
//			isPlatform = true;
//			canRemoveCat = true;
//			GetComponent<SpriteRenderer> ().color = new Color(126f, 243f, 57f, 0.5f);
		}
	}

	public void setElevCat(GameObject cat){
		elevCatObj = cat;
		if (elevCatObj != null) {
			canRemoveCat = true;
		} else {
			canRemoveCat = false;
		}
	}

	public GameObject getElevCat(){
		return elevCatObj;
	}

	public bool hasACat(){
		return (elevCatObj != null);
	}
	
//	public void removePlacedCat(){
//		isPlatform = false;
//		GetComponent<SpriteRenderer> ().color = Color.white;
//	}

}
