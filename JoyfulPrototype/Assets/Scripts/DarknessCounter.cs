using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DarknessCounter : MonoBehaviour {

    public int totalDarkness;

    public static int currentDarkness;

    Text text;
	// Use this for initialization
	void Start () {
        currentDarkness = totalDarkness;

        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(currentDarkness < 0)
        {
            currentDarkness = 0;
        }

        decimal darknessPercent = decimal.Round((decimal)currentDarkness / totalDarkness * 100, 2);

        text.text = "" + darknessPercent + "%";
	}

    public static void decreaseDarkness(int amount)
    {
        currentDarkness = currentDarkness - amount;
    }

    public static int getCurrentDarkness()
    {
        return currentDarkness;
    }
}
