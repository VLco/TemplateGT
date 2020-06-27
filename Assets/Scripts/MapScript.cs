using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MapScript : MonoBehaviour
{
    int countPoint = RScript.countPoint;

    public GameObject route;
    public GameObject marker;
    public GameObject cross;
    public GameObject end;

    
    Dictionary<int, GameObject> listMarker = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        ShowRoute();
        
    }

    void ShowRoute()
    {

        GameObject obj = Instantiate(marker, route.transform);
        obj.transform.localPosition = new Vector3(Random.Range(20, route.GetComponent<RectTransform>().rect.width - 20),
            Random.Range(20-route.GetComponent<RectTransform>().rect.height, -20));
        obj.name = RScript.curPoint.ToString();
        obj.GetComponent<Button>().onClick.AddListener(()=> ShowInfoForPoint(obj));
        listMarker.Add(RScript.curPoint, obj);
        for(int i = 0; i <= countPoint; i++)
        {
            if(i < RScript.curPoint) 
            { 
                GameObject tobj = Instantiate(cross, route.transform);
                tobj.transform.localPosition = listMarker[i].transform.localPosition;
                tobj.name = i.ToString();
                Destroy(listMarker[i]);
                listMarker[i] = tobj;
            }
                
        }
 
        

    }



    public void ShowInfoForPoint(GameObject panel)
    {
        //Debug.Log(btn.name + "---" + currentPoint);
        GameObject.Find("InfoTitle").GetComponent<Text>().text = RScript.resources[RScript.curPoint].infoTitle;
        GameObject.Find("InfoText").GetComponent<Text>().text = RScript.resources[RScript.curPoint].infoText;
        route.GetComponent<RectTransform>().parent.gameObject.SetActive(false);
    }


    public void ToQuest()
    { 
        SceneManager.LoadScene("QuestScene", LoadSceneMode.Additive);
    }


    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
