using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tileStuff : MonoBehaviour {

	public bool isPlatform = false;
	public bool canRemoveCat = false;

	//[Header("ElevatorCat")]
	public GameObject elevCatObj { get; set; }
	public int elevCat = 1; //?

	//[Header("AttackCat")]
	public List<GameObject> attackCatObjs;
	public int attackCat = 2;
	public int attackCatSpawnLeft = 3;

	//[Header("Rat")]
	public GameObject ratObj { get; set; }
	public bool hasARat = false;

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
			if (attackCatObjs == null) {
				attackCatObjs = new List<GameObject> ();
			}
			GameObject attackCatObj = (GameObject)(Instantiate (cat, new Vector3 (x, y - tileSize / 3, 0), Quaternion.identity));
			addAttackCat (attackCatObj);
			attackCatObj.GetComponent<attackCatControl> ().setFacingRight (true);
		} else if (type == attackCatSpawnLeft) {
			GameObject attackCatObj = (GameObject)(Instantiate (cat, new Vector3 (x, y - tileSize / 3, 0), Quaternion.identity));
			addAttackCat (attackCatObj);
			attackCatObj.GetComponent<attackCatControl> ().setFacingRight (false);
		}
	}

	public void placeRat(GameObject rat, float tileSize){
		float x = transform.position.x;
		float y = transform.position.y;
		ratObj = (GameObject)(Instantiate (rat, new Vector3 (x, y - tileSize / 3, 0), Quaternion.identity));
		setRat (ratObj);
	}

	public void addAttackCat(GameObject cat){
		if (attackCatObjs == null) {
			attackCatObjs = new List<GameObject> ();
		}
		attackCatObjs.Add (cat);
		checkCanRemoveCat ();
	}

	public void removeAttackCat(GameObject cat){
		if (attackCatObjs != null && attackCatObjs.Count > 0) {
			attackCatObjs.Remove (cat);
		}
	}

	public void setElevCat(GameObject cat){
		elevCatObj = cat;
		checkCanRemoveCat ();
	}

	public void setRat(GameObject rat){
		ratObj = rat;
		hasARat = (rat != null);
	}

	private void checkCanRemoveCat(){
		canRemoveCat = (elevCatObj != null || (attackCatObjs != null && attackCatObjs.Count > 0));
	}

	public GameObject getElevCat(){
		return elevCatObj;
	}

	public bool hasACat(){
//		return (elevCatObj != null && (attackCatObjs != null));
		return canRemoveCat;
	}

	public int deleteAllCats(int elevInkCost, int attackInkCost){
		bool elevDeleted = deleteElevCat ();
		int numAttackDeleted = deleteAttackCats ();
		int ret = 0;
		if (elevDeleted) {
			ret += elevInkCost;
		}
		ret += (attackInkCost * numAttackDeleted);
		return ret;
	}

	public bool deleteElevCat(){
		if (elevCatObj != null) {
			Destroy (elevCatObj);
			setElevCat (null);
			return true;
		}
		return false;
	}

	public int deleteAttackCats(){
		int ret = 0;
		if (attackCatObjs != null && attackCatObjs.Count > 0) {
			foreach (GameObject attackCatObj in attackCatObjs) {
				Destroy (attackCatObj);
				ret++;
			}
			attackCatObjs = new List<GameObject> ();
//			Destroy (attackCatObj);
//			setAttackCat (null);
		}
		return ret;
	}

	public void deleteAttackCat(GameObject cat){
		removeAttackCat (cat);
		Destroy (cat);
	}

	public void deleteRat(){
		if (ratObj != null) {
			Destroy (ratObj);
			setRat (null);
		}
	}
	
//	public void removePlacedCat(){
//		isPlatform = false;
//		GetComponent<SpriteRenderer> ().color = Color.white;
//	}

}
