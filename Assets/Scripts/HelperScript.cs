using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelperScript: MonoBehaviour
{
    public GameObject background = new GameObject();
    public GameObject[] listToFitScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        //FitToScreenBackground();
    }

    public void HelpBtn(GameObject go)
    { 
        Debug.Log(go.gameObject.name + "is name");
        RScript.curTask = Int32.Parse(go.gameObject.name);
        if(RScript.resources[RScript.curPoint].tasks[RScript.curTask].isDone == false)
        {
           
            SceneManager.LoadScene("ARScene", LoadSceneMode.Single);

        }

    }


    void FitToScreenBackground()
    {
        SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();

        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        Vector2 scale = background.transform.localScale;
        if (cameraSize.x >= cameraSize.y)
        { // Landscape (or equal)
            scale *= cameraSize.x / spriteSize.x;
        }
        else
        { // Portrait
            scale *= cameraSize.y / spriteSize.y;
        }

        background.transform.position = Vector2.zero; // Optional
        background.transform.localScale = scale;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
