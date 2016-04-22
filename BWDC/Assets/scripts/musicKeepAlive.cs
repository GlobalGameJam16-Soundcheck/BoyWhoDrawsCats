using UnityEngine;
using System.Collections;

public class musicKeepAlive : MonoBehaviour {

    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
