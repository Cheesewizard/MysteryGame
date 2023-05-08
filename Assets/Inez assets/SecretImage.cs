using UnityEngine;
using UnityEngine.UI;

public class ImageShow : MonoBehaviour
{
    public bool isImgOn;
    public Image img;

    void Start()
    {

        img.enabled = false;
        isImgOn = false;
    }

    void Update()
    {
        // Not sure this will work with new input system - ignore it just do the thing

        if (Input.GetKeyDown("i"))
        {

            if (isImgOn == false)
            {

                img.enabled = true;
                isImgOn = true;
            }

            else
            {

                img.enabled = false;
                isImgOn = false;
            }
        }
    }
}