using UnityEngine;
using System.Collections;

public class musicKeepAlive : MonoBehaviour {

    // Use this for initialization
//    void Awake()
//    {
//        DontDestroyOnLoad(transform.gameObject);
//    }

	private static musicKeepAlive instance = null;

	public static musicKeepAlive Instance {
		get { return instance; }
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
