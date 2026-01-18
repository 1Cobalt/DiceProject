using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberButton : MonoBehaviour
{
    public Sprite disableTexture;
    public Sprite enableTexture;

    public static bool isReloading = false;

    void Start()
    {
        isReloading = false;
    }

    public void ButtonIsPressed()
    {
        if (isReloading) return;

        if (this.gameObject.name == Camera.main.GetComponent<GameManager>().GetPressedButton())
        {
            Camera.main.GetComponent<GameManager>().SetPressedButton("");
            this.GetComponent<Image>().sprite = disableTexture;
        }
        else if (Camera.main.GetComponent<GameManager>().GetPressedButton() == "")
        {
            Camera.main.GetComponent<GameManager>().SetPressedButton(this.gameObject.name);
            this.GetComponent<Image>().sprite = enableTexture;
        }
        else
        {
            GameObject.Find(Camera.main.GetComponent<GameManager>().GetPressedButton()).GetComponent<Image>().sprite = disableTexture;
            this.GetComponent<Image>().sprite = enableTexture;
            Camera.main.GetComponent<GameManager>().SetPressedButton(this.gameObject.name);
        }

    }

    public void SetSprite(bool isEnabled)
    {
        if (!isEnabled)
            this.GetComponent<Image>().sprite = disableTexture;
        else
            this.GetComponent<Image>().sprite = enableTexture;
    }
}
