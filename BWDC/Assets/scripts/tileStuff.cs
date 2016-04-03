using UnityEngine;
using System.Collections;

public class tileStuff : MonoBehaviour {

	public bool isPlatform = false;
	private bool canRemoveCat = false;

	private bool hasElevCat = false;
	public int elevCat = 1; //?

	public void setIsPlatform(bool isPlat){
		isPlatform = isPlat;
	}

	public bool getIsPlatform(){
		return isPlatform;
	}

	public bool canStandOn(){
		return isPlatform || hasElevCat;
	}

	public bool canPlacePlatCat(){
		return !isPlatform && !hasElevCat;
	}

	public void placeCat(int type, GameObject cat, float tileSize){
		if (type == elevCat) {
			hasElevCat = true;
			float x = transform.position.x;
			float y = transform.position.y;
			Instantiate (cat, new Vector3 (x, y + tileSize / 3, 0), Quaternion.identity);
//			isPlatform = true;
//			canRemoveCat = true;
//			GetComponent<SpriteRenderer> ().color = new Color(126f, 243f, 57f, 0.5f);
		}
	}

	public void setHasElevCat(bool b){
		hasElevCat = b;
	}

	public bool getHasElevCat(){
		return hasElevCat;
	}

	public bool hasACat(){
		return hasElevCat;
	}
	
//	public void removePlacedCat(){
//		isPlatform = false;
//		GetComponent<SpriteRenderer> ().color = Color.white;
//	}

}
