using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RemoteController : MonoBehaviour {

    public string url = "http://10.0.0.1:8006/?";
    private string urlMethodAdd = "method=add&";
    private string urlMethodSet = "method=set&";
    private string urlAmount = "vol=";

    public int plusMinusAmount = 5;

    private bool doLiveUpdate = false;

    public Button plusButton;
    public Button minusButton;
    public Button startButton;
    public Button stopButton;
    public Slider slider;
    public Text debugText;

    void Start()
    {

        plusButton.onClick.AddListener(onPlusButtonClicked);
        minusButton.onClick.AddListener(onMinusButtonClicked);
        startButton.onClick.AddListener(onStartButtonClicked);
        stopButton.onClick.AddListener(onStopButtonClicked);

    }

    void Update()
    {        
        debugText.text = Input.acceleration.x.ToString();

        if (doLiveUpdate)
        {
            slider.value = Input.acceleration.y;
            WWW www = new WWW(url + urlMethodSet + urlAmount + (slider.value*100));
        }
        else
        {
            slider.value = 0;
        }
    }

    void onStartButtonClicked()
    {
        doLiveUpdate = true;
    }

    void onStopButtonClicked()
    {
        doLiveUpdate = false;
    }

    void onPlusButtonClicked()
    {        
        WWW www = new WWW(url+urlMethodAdd+urlAmount+plusMinusAmount);
        StartCoroutine(WaitForRequest(www));
    }

    void onMinusButtonClicked()
    {
        WWW www = new WWW(url + urlMethodAdd + urlAmount + (plusMinusAmount*-1));
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.data);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }


    
}
