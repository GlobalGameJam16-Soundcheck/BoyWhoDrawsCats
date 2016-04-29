using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class musicKeepAlive : MonoBehaviour {

	private static musicKeepAlive instance = null;
	public AudioClip hardAudio;
	public AudioClip endAudio;
	private AudioSource mySource;
	private int sceneIndex;
	public int hardLevel;
	private bool changedMusic;
	private bool endAudioPlaying;

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
		mySource = GetComponent<AudioSource> ();
		changedMusic = false;
	}

	void Update(){
		if (!changedMusic) {
			sceneIndex = SceneManager.GetActiveScene ().buildIndex;
			if (sceneIndex >= hardLevel) {
				mySource.Stop ();
				mySource.clip = hardAudio;
				mySource.Play ();
				changedMusic = true;
			}
		} else if (!endAudioPlaying) {
			sceneIndex = SceneManager.GetActiveScene ().buildIndex;
			if (sceneIndex == SceneManager.sceneCountInBuildSettings - 1) {
				mySource.Stop ();
				mySource.clip = endAudio;
				mySource.Play ();
				endAudioPlaying = true;
			}
		}
	}


}
