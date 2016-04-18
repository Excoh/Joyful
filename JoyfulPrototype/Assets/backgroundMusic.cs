using UnityEngine;
using System.Collections;

public class backgroundMusic : MonoBehaviour {
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(Application.loadedLevel == 5)
        {
            if (PlayerPrefs.GetInt("CurrentScene") == 1)
            {
                Destroy(this.gameObject);
            }
            else {
                this.gameObject.GetComponent<AudioSource>().mute = true;
            }
        } else if(Application.loadedLevel == 0 || Application.loadedLevel == 4)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.gameObject.GetComponent<AudioSource>().mute = false;
        }
    }

    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    
}
