using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuScript : MonoBehaviour
{

    public GameObject settingsPanel;
    public GameObject backgroundResourcesNameArchive;
    public GameObject nameResourcesArchive;
    string nameArchive = RScript.getNameResourcesArchive();

    private void Start()
    {
        //SceneManager.LoadSceneAsync("MapScene", LoadSceneMode.Additive);
        //SceneManager.LoadScene("MapScene", LoadSceneMode.Additive);
        RScript.ReqestPermission();
        SetResourcesArchive();

    }
    public void Play()
    { 
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }

    private void Update()
    {
        nameArchive = RScript.getNameResourcesArchive();

        if (nameArchive == null)
        {
            GameObject.Find("PlayBtn").GetComponent<Button>().interactable = false;
        }
        else
        {
            GameObject.Find("PlayBtn").GetComponent<Button>().interactable = true;

        }
    }

    public void SetResourcesArchive()
    {
        StartCoroutine(RScript.ResourceFromArchive());

    }

    public void OpenSettings()
    {
        nameArchive = RScript.getNameResourcesArchive();
        if (settingsPanel.activeSelf == false)
            settingsPanel.SetActive(true);
        if(nameArchive == null)
        {
            backgroundResourcesNameArchive.GetComponent<Image>().color = new Color(255, 0, 0, 100);
            nameResourcesArchive.GetComponent<Text>().text = "None";
        }
        else
        {
            backgroundResourcesNameArchive.GetComponent<Image>().color = new Color(0, 255, 0, 100);
            nameResourcesArchive.GetComponent<Text>().text = nameArchive;
            GameObject go = new GameObject();
            /*go.AddComponent<Image>().sprite = Sprite.Create(RScript.GetTexture(), new Rect(0, 0, RScript.GetTexture().width, RScript.GetTexture().height),
                                new Vector2(0.5f, 0.5f));
            go.GetComponent<RectTransform>().transform.SetParent(settingsPanel.transform);
            go.GetComponent<RectTransform>().transform.localPosition = new Vector3(0, 0, 0);
            */
        }

    }
}
