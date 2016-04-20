using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class endControl : MonoBehaviour {

	public void hitEnd(){
		int i = SceneManager.GetActiveScene().buildIndex;
		i = (i + 1) % SceneManager.sceneCountInBuildSettings;
		SceneManager.LoadScene (i);
	}

}
