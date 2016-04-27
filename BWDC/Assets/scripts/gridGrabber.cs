using UnityEngine;
using System.Collections;

public class gridGrabber : MonoBehaviour {

	public GameObject gridObj;

	public GridControl returnGrid(){
		return gridObj.GetComponent<GridControl> ();
	}

}
