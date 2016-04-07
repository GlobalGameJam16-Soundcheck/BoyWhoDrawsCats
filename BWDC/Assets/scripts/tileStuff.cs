using UnityEngine;
using System.Collections;

public class tileStuff : MonoBehaviour {

	public bool isPlatform = false;
	public bool canRemoveCat = false;

	//[Header("ElevatorCat")]
	public GameObject elevCatObj { get; set; }
	public int elevCat = 1; //?

	//[Header("AttackCat")]
	public GameObject attackCatObj { get; set; }
	public int attackCat = 2;
	public int attackCatSpawnLeft = 3;

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
		float x = transform.position.x;
		float y = transform.position.y;
		if (type == elevCat) {
			elevCatObj = (GameObject)(Instantiate (cat, new Vector3 (x, y - tileSize / 3, 0), Quaternion.identity));
			setElevCat (elevCatObj);
		} else if (type == attackCat) {
			attackCatObj = (GameObject)(Instantiate (cat, new Vector3 (x, y - tileSize / 3, 0), Quaternion.identity));
			setAttackCat (attackCatObj);
			attackCatObj.GetComponent<attackCatControl> ().setFacingRight (true);
		} else if (type == attackCatSpawnLeft) {
			attackCatObj = (GameObject)(Instantiate (cat, new Vector3 (x, y - tileSize / 3, 0), Quaternion.identity));
			setAttackCat (attackCatObj);
			attackCatObj.GetComponent<attackCatControl> ().setFacingRight (false);
		}
	}

	public void setAttackCat(GameObject cat){
		attackCatObj = cat;
		checkCanRemoveCat ();
	}

	public void setElevCat(GameObject cat){
		elevCatObj = cat;
		checkCanRemoveCat ();
	}

	private void checkCanRemoveCat(){
		canRemoveCat = (elevCatObj != null || attackCatObj != null);
	}

	public GameObject getElevCat(){
		return elevCatObj;
	}

	public bool hasACat(){
		return (elevCatObj != null && attackCatObj != null);
	}

	public void deleteAllCats(){
		if (elevCatObj != null) {
			Destroy (elevCatObj);
			setElevCat (null);
		}
		if (attackCatObj != null) {
			Destroy (attackCatObj);
			setAttackCat (null);
		}
	}
	
//	public void removePlacedCat(){
//		isPlatform = false;
//		GetComponent<SpriteRenderer> ().color = Color.white;
//	}

}
