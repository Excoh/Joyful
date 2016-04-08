using UnityEngine;
using System.Collections;

public class DistortedPlatform : MonoBehaviour
{

    public float distortInterval = 1; // default= 1 second for block to begin to distort
    public float disappearInterval = 1; // default= 1 second for block to disappear
    public float distortSpeed = 100; // larger the value, faster the speed. Default = 100

    private bool distort = true; // to signify when to distort or not
    private bool disappear = false; // to signify when to reappear

    private float time = 0; // for keeping track of time

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (distort)
        {
            this.GetComponent<Renderer>().material.color = new Color(this.GetComponent<Renderer>().material.color.r, this.GetComponent<Renderer>().material.color.g,
                                                 this.GetComponent<Renderer>().material.color.b, Mathf.Sin(time * Mathf.PI * distortSpeed));
            // rotate tile along y-axis
            this.transform.Rotate(0, Time.deltaTime * distortSpeed, 0);

            // once tile gets to 90 degress along y-axis (disappeared),
            // we start the disappearance interval
            if (this.transform.eulerAngles.y >= 90 && !disappear)
            {
                time = 0;
                distort = false;
                this.transform.eulerAngles = new Vector3(0, 90);
            }

            // once the tile gets to 180 degrees (original position),
            // we restart the whole process
            else if (this.transform.eulerAngles.y >= 180)
            {
                time = 0;
                distort = false;
                disappear = false;
                this.transform.eulerAngles = new Vector3(0, 0);
            }
        }
        else
        {
            this.GetComponent<Renderer>().material.color = new Color(this.GetComponent<Renderer>().material.color.r, this.GetComponent<Renderer>().material.color.g,
                               this.GetComponent<Renderer>().material.color.b, 1);
        }

        // logic for handling time of disappearance
        if (time >= disappearInterval && this.transform.eulerAngles.y >= 90)
        {
            disappear = true;
            distort = true;
        }

        // logic for handling time to wait before distorting
        if (time >= distortInterval)
        {
            distort = true;
        }


    }
}