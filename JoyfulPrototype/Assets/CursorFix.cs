using UnityEngine;
using System.Collections;

public class CursorFix : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
	
	// Update is called once per frame
	void Update () {

    }
}
