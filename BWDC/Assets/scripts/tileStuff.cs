using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tileStuff : MonoBehaviour {

	public bool isPlatform = false;
	public bool canRemoveCat = false;

	//[Header("end")]
	public bool isEnd = false;
	private GameObject endObj;

	//[Header("boy")]
	public bool isBoyTile = false;
	private GameObject boy;

	//[Header("ElevatorCat")]
	public GameObject elevCatObj { get; set; }
	public int elevCat = 1; //?
	public bool elevCatCanReach { get; set; }

	//[Header("AttackCat")]
	public List<GameObject> attackCatObjs;
	public int attackCat = 2;
	public int attackCatSpawnLeft = 3;

	//[Header("YarnCat")]
	public List<GameObject> yarnCatObjs;
	public int yarnCat = 4;
	public int yarnCatSpawnLeft = 5;
	public GameObject yarnObj { get; set; }
	public bool yarnCatOnYarn = false;

	//[Header("Rat")]
	public List<GameObject> ratObjs;
	public bool hasARat = false;

	void Update(){
		if (ratObjs != null && ratObjs.Count > 0) {
			if (yarnCatObjs != null && yarnCatObjs.Count > 0) {
				deleteYarnCat (yarnCatObjs [0]);
			}
			if (attackCatObjs != null && attackCatObjs.Count > 0) {
				deleteRat (ratObjs [0]);
				deleteAttackCat (attackCatObjs [0]);
			} 
		}
		if (yarnCatObjs != null && yarnCatObjs.Count > 0 && canPlayWithYarn()) {
			setYarnCatOnYarn (true);
			getYarnObj ().GetComponent<yarnControl> ().activateDoors (false);
			yarnCatObjs [0].GetComponent<yarnCatControl> ().setOnYarn (true);
		}
		checkCanRemoveCat ();
		checkIsBoyTile ();
		checkIsEnd ();
		if (isBoyTile && isEnd) {
			endObj.GetComponent<endControl> ().hitEnd ();
		}
	}

	public void setBoyTile(GameObject boyGam){
		boy = boyGam;
	}

	public GameObject getBoyTile(){
		return boy;
	}

	public void setEndTile(GameObject endGam){
		endObj = endGam;
	}

	private void checkIsEnd(){
		isEnd = (endObj != null);
	}

	private void checkIsBoyTile(){
		isBoyTile = (boy != null);
	}

	public void setIsPlatform(bool isPlat){
		isPlatform = isPlat;
	}

	public bool getIsPlatform(){
		return isPlatform;
	}

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
			if (attackCatObjs == null) {
				attackCatObjs = new List<GameObject> ();
			}
			GameObject attackCatObj = (GameObject)(Instantiate (cat, new Vector3 (x, y - tileSize / 3, 0), Quaternion.identity));
			addAttackCat (attackCatObj);
			attackCatObj.GetComponent<attackCatControl> ().setFacingRight (false);
		} else if (type == yarnCat) {
			if (yarnCatObjs == null) {
				yarnCatObjs = new List<GameObject> ();
			}
			GameObject yarnCatObj = (GameObject)(Instantiate (cat, new Vector3 (x, y - tileSize / 3, 0), Quaternion.identity));
			addYarnCat (yarnCatObj);
			yarnCatObj.GetComponent<yarnCatControl> ().setFacingRight (true);
		} else if (type == yarnCatSpawnLeft) {
			if (yarnCatObjs == null) {
				yarnCatObjs = new List<GameObject> ();
			}
			GameObject yarnCatObj = (GameObject)(Instantiate (cat, new Vector3 (x, y - tileSize / 3, 0), Quaternion.identity));
			addYarnCat (yarnCatObj);
			yarnCatObj.GetComponent<yarnCatControl> ().setFacingRight (false);
		}
	}

	public void placeRat(GameObject rat, float tileSize){
		float x = transform.position.x;
		float y = transform.position.y;
		GameObject ratObj = (GameObject)(Instantiate (rat, new Vector3 (x, y - tileSize / 3, 0), Quaternion.identity));
		addRat (ratObj);
	}

	public void addAttackCat(GameObject cat){
		if (attackCatObjs == null) {
			attackCatObjs = new List<GameObject> ();
		}
		attackCatObjs.Add (cat);
		checkCanRemoveCat ();
	}

	public void addYarnCat(GameObject cat){
		if (yarnCatObjs == null) {
			yarnCatObjs = new List<GameObject> ();
		}
		yarnCatObjs.Add (cat);
		checkCanRemoveCat ();
	}

	public void setYarnObj(GameObject yarn){
		yarnObj = yarn;
	}

	public GameObject getYarnObj(){
		return yarnObj;
	}

	public void setYarnCatOnYarn(bool ycOnYarn){
		yarnCatOnYarn = ycOnYarn;
	}

	public bool canPlayWithYarn(){
		return (getYarnObj() != null && !yarnCatOnYarn);
	}

	public void removeAttackCat(GameObject cat){
		if (attackCatObjs != null && attackCatObjs.Count > 0) {
			attackCatObjs.Remove (cat);
		}
	}

	public void removeYarnCat(GameObject cat){
		if (yarnCatObjs != null && yarnCatObjs.Count > 0) {
			yarnCatObjs.Remove (cat);
		}
	}

	public void removeRat(GameObject rat){
		if (ratObjs != null && ratObjs.Count > 0) {
			ratObjs.Remove (rat);
		}
	}

	public void setElevCat(GameObject cat){
		elevCatObj = cat;
		checkCanRemoveCat ();
	}

	public void addRat(GameObject rat){
		if (ratObjs == null) {
			ratObjs = new List<GameObject> ();
		}
		ratObjs.Add (rat);
	}

	private void checkHasRat(){
		hasARat =  (ratObjs != null && ratObjs.Count > 0);
	}

	private void checkCanRemoveCat(){
		canRemoveCat = (elevCatObj != null || (attackCatObjs != null && attackCatObjs.Count > 0) ||
			(yarnCatObjs != null && yarnCatObjs.Count > 0));
	}

	public GameObject getElevCat(){
		return elevCatObj;
	}

	public bool hasACat(){
//		return (elevCatObj != null && (attackCatObjs != null));
		return canRemoveCat;
	}

	public int deleteAllCats(int elevInkCost, int attackInkCost, int yarnCatCost){
		bool elevDeleted = deleteElevCat ();
		int numAttackDeleted = deleteAttackCats ();
		int numYarnDeleted = deleteYarnCats ();
		int ret = 0;
		if (elevDeleted) {
			ret += elevInkCost;
		}
		ret += (attackInkCost * numAttackDeleted);
		ret += (yarnCatCost * numYarnDeleted);
		return ret;
	}

	public bool deleteElevCat(){
		if (elevCatObj != null) {
			Destroy (elevCatObj);
			setElevCat (null);
			if (ratObjs != null) {
				foreach (GameObject ratObj in ratObjs) {
					ratObj.GetComponent<ratsControl> ().fallDown ();
				}
			}
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
		}
		return ret;
	}

	public void deleteAttackCat(GameObject cat){
		removeAttackCat (cat);
		Destroy (cat);
	}

	public int deleteYarnCats(){
		int ret = 0;
		if (yarnCatObjs != null && yarnCatObjs.Count > 0) {
			foreach (GameObject yarnCatObj in yarnCatObjs) {
				Destroy (yarnCatObj);
				ret++;
			}
			yarnCatObjs = new List<GameObject> ();
		}
		if (ret > 0 && yarnObj != null) {
			yarnObj.GetComponent<yarnControl> ().activateDoors (true);
			setYarnCatOnYarn (false);
		}
		return ret;
	}

	public void deleteYarnCat(GameObject cat){
		if (yarnObj != null) {
			yarnObj.GetComponent<yarnControl> ().activateDoors (true);
			setYarnCatOnYarn (false);
		}
		removeYarnCat (cat);
		Destroy (cat);
	}

	public void deleteAllRats(){
		if (ratObjs != null && ratObjs.Count > 0) {
			foreach (GameObject ratObj in ratObjs) {
				Destroy (ratObj);
			}
			ratObjs = new List<GameObject> ();
		}
	}

	public void deleteRat(GameObject rat){
		removeRat (rat);
		Destroy (rat);
	}

	public GameObject getRat(){
		if (ratObjs == null || ratObjs.Count <= 0) {
			return null;
		}
		GameObject ratObj = ratObjs [0];
		return ratObj;
	}

	public void setDoor(bool isDoor){
		isPlatform = isDoor;
		if (isDoor) {
			if (boy != null) {
				int doorDamage = 1;
				boy.GetComponent<movment> ().getHit (doorDamage);
			}
			deleteAllCats (0, 0, 0);
			deleteAllRats ();
		}
	}

	public void checkIfBoyFallingCusBelowDoorInactive(){
		if (boy != null && elevCatObj == null && !isPlatform) {
			boy.GetComponent<movment> ().setFloating (true);
		}
	}

}
