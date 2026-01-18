using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileBehaviour : MonoBehaviour
{
    private GameManager gameManager;

    private Color oldColor;
    private Color newColor;
    private bool isChanging = false;
    public float timeToChange = 0.0f;

    public static int animationsCount = 0;
    public static bool isAnyAnimationPlaying = false;

    void Start()
    {
        gameManager = Camera.main.GetComponent<GameManager>();
        TileBehaviour.isAnyAnimationPlaying = false;
        TileBehaviour.animationsCount = 0;
    }




    void Update()
    {
        if (isChanging)
            this.transform.Find("tile").GetComponent<Renderer>().material.color = Color.Lerp(oldColor, newColor, timeToChange);

        if (Input.touchCount == 1)
        {

            Touch touch = Input.GetTouch(0); // Get the first touch

            if (touch.phase == TouchPhase.Began)
            {
                // Handle touch began (e.g., raycast to detect object)
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                   if(hit.collider.tag == "Tile")
                    {
                        TileIsTouched();
                    }
                }
            }
        }
    }

    public static bool GetAnimationStatus()
    {
        if (TileBehaviour.animationsCount == 0)
            return false;
        else
            return true;
    }

    public void ChangeColor(Color ColorToChange)
    {
        newColor = ColorToChange;
        timeToChange = 0.0f;
        oldColor = this.transform.Find("tile").GetComponent<Renderer>().material.color;
        isChanging = true;
        StartCoroutine(PlayColorAnimation());
    }

    private IEnumerator PlayColorAnimation()
    {
        // Ждём, пока не освободится "анимационный канал"
        while (isAnyAnimationPlaying)
            yield return null;

        isAnyAnimationPlaying = true;

        this.GetComponent<Animation>().Play("ColorAnimation");

        // ждём конца анимации
        yield return new WaitForSeconds(this.GetComponent<Animation>()["ColorAnimation"].length);

        isAnyAnimationPlaying = false;
    }

    public IEnumerator PlaySettingAnimation()
    {
        // Ждём, пока не освободится "анимационный канал"
        TileBehaviour.animationsCount++;

        while (isAnyAnimationPlaying)
            yield return null;

        isAnyAnimationPlaying = true;

        var anim = this.gameObject.transform.Find("tile").GetComponent<Animation>();
        anim.Play("TileAnimation");

        yield return new WaitForSeconds(anim["TileAnimation"].length);
        yield return new WaitForSeconds(1.0f);

        isAnyAnimationPlaying = false;
        TileBehaviour.animationsCount--;

    }

    void TileIsTouched()
    {
        if (gameManager.GetPressedButton() != "" && gameManager.GetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0') == 0 && Time.deltaTime == 1.0f && NumberButton.isReloading == false)
        {
            if (gameManager.GetPressedButton()[8] == '1')
            {
                //this.GetComponent<Renderer>().material.color = Color.blue;
                gameManager.SetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0', 1);
            }
            else if (gameManager.GetPressedButton()[8] == '2')
            {
                //this.GetComponent<Renderer>().material.color = Color.red;
                gameManager.SetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0', 2);
            }
            else if (gameManager.GetPressedButton()[8] == '3')
            {
                //this.GetComponent<Renderer>().material.color = Color.green;
                gameManager.SetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0', 3);
            }
            else if (gameManager.GetPressedButton()[8] == '4')
            {
                //this.GetComponent<Renderer>().material.color = Color.green;
                gameManager.SetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0', 4);
            }
            else if (gameManager.GetPressedButton()[8] == '5')
            {
                //this.GetComponent<Renderer>().material.color = Color.green;
                gameManager.SetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0', 5);
            }
            else if (gameManager.GetPressedButton()[8] == '6')
            {
                //this.GetComponent<Renderer>().material.color = Color.green;
                gameManager.SetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0', 6);
            }


            GameObject.Find(gameManager.GetPressedButton()).GetComponent<NumberButton>().SetSprite(false);
            gameManager.SetPressedButton("");
            
            
        }
    }


    void OnMouseDown()
    {
        if (gameManager.GetPressedButton() != "" && gameManager.GetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0') == 0 && Time.deltaTime != 0.0f && NumberButton.isReloading == false)
        
        {
            if (gameManager.GetPressedButton()[8] == '1')
            {
                //this.GetComponent<Renderer>().material.color = Color.blue;
                gameManager.SetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0', 1);
            }
            else if (gameManager.GetPressedButton()[8] == '2')
            {
                //this.GetComponent<Renderer>().material.color = Color.red;
                gameManager.SetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0', 2);
            }
            else if (gameManager.GetPressedButton()[8] == '3')
            {
                //this.GetComponent<Renderer>().material.color = Color.green;
                gameManager.SetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0', 3);
            }
            else if (gameManager.GetPressedButton()[8] == '4')
            {
                //this.GetComponent<Renderer>().material.color = Color.green;
                gameManager.SetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0', 4);
            }
            else if (gameManager.GetPressedButton()[8] == '5')
            {
                //this.GetComponent<Renderer>().material.color = Color.green;
                gameManager.SetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0', 5);
            }
            else if (gameManager.GetPressedButton()[8] == '6')
            {
                //this.GetComponent<Renderer>().material.color = Color.green;
                gameManager.SetTileValue(this.gameObject.name[5] - '0', this.gameObject.name[7] - '0', 6);
            }

            GameObject pressedButton = GameObject.Find(gameManager.GetPressedButton());

            if (pressedButton != null)
            { 
            pressedButton.GetComponent<NumberButton>().SetSprite(false);
            }
            gameManager.SetPressedButton("");


        }
    }
}
