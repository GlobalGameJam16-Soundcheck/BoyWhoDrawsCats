using UnityEngine;
using System.Collections;

public class drawingController : MonoBehaviour {

	public GameObject player;
	private movment playerController;

	// Use this for initialization
	void Start () {
		playerController = player.GetComponent<movment> ();
	}
	
	// Update is called once per frame
	void Update () {
		checkElevCatSpawn ();
		checkAttackCatSpawn ();
	}

	private void checkElevCatSpawn(){
		if (Input.GetKeyUp ("1")) {
			playerController.spawnElevCat ();
		}
	}

	private void checkAttackCatSpawn(){
		if (Input.GetKeyUp ("2")) {
			playerController.spawnAttackCat (playerController.attackCatRight);
		} else if (Input.GetKeyUp ("3")) {
			playerController.spawnAttackCat (playerController.attackCatLeft);
		} else if (Input.GetKeyUp ("4")) {
			playerController.spawnYarnCat (playerController.yarnCatRight);
		} else if (Input.GetKeyUp ("5")) {
			playerController.spawnYarnCat (playerController.yarnCatLeft);
		}
	}
}
