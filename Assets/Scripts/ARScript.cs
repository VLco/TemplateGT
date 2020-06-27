using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;

public class ARScript : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    GameObject desctask;

    Texture2D[] texture2D;
    GameObject tmp;
    ObjectTracker tobj;
    

	// Start is called before the first frame update
	void Start()
    {
        ShowMenuElement();        
    }

    void setImageTarget(Texture2D texture2D)
    {
        var objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        // get the runtime image source and set the texture to load
        var runtimeImageSource = objectTracker.RuntimeImageSource;

        runtimeImageSource.SetImage(texture2D, 0.5f, "task"+RScript.curTask.ToString());
        // create a new dataset and use the source to create a new trackable
        var dataset = objectTracker.CreateDataSet();
        var trackableBehaviour = dataset.CreateTrackable(runtimeImageSource, "task" + RScript.curTask.ToString());

        // add the DefaultTrackableEventHandler to the newly created game object
        trackableBehaviour.gameObject.AddComponent<DefaultTrackableEventHandler>();

        // activate the dataset
        objectTracker.ActivateDataSet(dataset);


        tmp = Instantiate(canvas, trackableBehaviour.transform);
        tobj = objectTracker;
    }

    void ShowMenuElement()
    {
        texture2D = RScript.resources[RScript.curPoint].tasks[RScript.curTask].textures;

        if (RScript.resources[RScript.curPoint].tasks[RScript.curTask].typeTask == typeTask.Find)
        {
            VuforiaARController.Instance.RegisterVuforiaStartedCallback(() => setImageTarget(texture2D[0]));
            desctask.GetComponent<Text>().text = RScript.resources[RScript.curPoint].tasks[RScript.curTask].desc;
            tmp.GetComponentInChildren<Button>().onClick.AddListener(()=>EndScene());
        }
        else if (RScript.resources[RScript.curPoint].tasks[RScript.curTask].typeTask == typeTask.MultiFind)
        {
            VuforiaARController.Instance.RegisterVuforiaStartedCallback(() => setImageTarget(texture2D[0]));

        }
        else if (RScript.resources[RScript.curPoint].tasks[RScript.curTask].typeTask == typeTask.Tour)
        {
            VuforiaARController.Instance.RegisterVuforiaStartedCallback(() => setImageTarget(texture2D[0]));

        }

    }

    void EndScene()
    {
        RScript.resources[RScript.curPoint].tasks[RScript.curTask].isDone = true;
        tobj.Stop();
        tobj.DestroyAllDataSets(true);
        SceneManager.LoadScene("QuestScene", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
