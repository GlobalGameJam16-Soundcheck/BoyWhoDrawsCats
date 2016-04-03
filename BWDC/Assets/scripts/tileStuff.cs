using UnityEngine;
using System.Collections;

public class tileStuff : MonoBehaviour {

	private bool isPlatform = false;
	private bool canRemoveCat = false;
	public int elevCat = 1; //?

	public void setIsPlatform(bool isPlat){
		isPlatform = isPlat;
	}

	public bool getIsPlatform(){
		return isPlatform;
	}

	public bool canStandOn(){
		return isPlatform;
	}

	public bool canPlacePlatCat(){
		return !isPlatform;
	}

	public void placeCat(int type){
		if (type == elevCat) {
			isPlatform = true;
			canRemoveCat = true;
			GetComponent<SpriteRenderer> ().color = new Color(126f, 243f, 57f, 0.5f);
		}
	}

	public bool getCanRemoveCat(){
		return canRemoveCat;
	}
	
	public void removePlacedCat(){
		isPlatform = false;
		GetComponent<SpriteRenderer> ().color = Color.white;
	}

}
