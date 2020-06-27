using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
    
    public GameObject content;
    public GameObject task;

    bool allIsDone = true;
    GameObject[] list = new GameObject[50];

    // Start is called before the first frame update
    void Start()
    {
        LoadTask();
    }

    void LoadTask()
    {
        allIsDone = true;
        for (int n = 0; n < RScript.resources[RScript.curPoint].countTasks; n++)
        {
            list[n] = Instantiate(task, content.transform);
            list[n].transform.localPosition = new Vector3(0,
                -50-task.GetComponent<RectTransform>().rect.height * n);
            list[n].name = n.ToString();
            list[n].GetComponent<Toggle>().isOn = RScript.resources[RScript.curPoint].tasks[n].isDone;
            list[n].GetComponentInChildren<Text>().text = RScript.resources[RScript.curPoint].tasks[n].title;
            //list[n].GetComponent<Toggle>().onValueChanged.AddListener(delegate { toAR(list[n]); });
            if (!RScript.resources[RScript.curPoint].tasks[n].isDone)
            {
                allIsDone = false;
            }

        }

    }

    public void toAR(GameObject go)
    {
        Debug.Log(go.name + "is name");
        SceneManager.LoadScene("ARScene", LoadSceneMode.Single);
        
    }

    public void Back()
    {
        if (allIsDone)
            RScript.curPoint++;
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
