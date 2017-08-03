using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RemoteController : MonoBehaviour
{

    private string hostname;
    private string port;
    private string url;
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
    //public Text debugText;
    public Text hostnameText;
    public Text portText;

    void Start()
    {

        hostname = PlayerPrefs.GetString("hostname", "http://10.0.0.1");
        port = PlayerPrefs.GetString("port", "8006");
        url = hostname + ":" + port + "/?";
        hostnameText.GetComponent<InputField>().text = hostname;
        portText.GetComponent<InputField>().text = port;

        hostnameText.GetComponent<InputField>().onValueChanged.AddListener(onHostnameChanged);
        portText.GetComponent<InputField>().onValueChanged.AddListener(onPortChanged);
        plusButton.onClick.AddListener(onPlusButtonClicked);
        minusButton.onClick.AddListener(onMinusButtonClicked);
        startButton.onClick.AddListener(onStartButtonClicked);
        stopButton.onClick.AddListener(onStopButtonClicked);

    }

    void Update()
    {        
        //debugText.text = /*"x: " + Input.acceleration.x.ToString() + */" y: " + Input.acceleration.y.ToString()/* + " z: " + Input.acceleration.z.ToString()*/;

        if (doLiveUpdate && Mathf.Abs(Input.acceleration.y * -1 - slider.value) > 0.01f)
        {
            slider.value = Input.acceleration.y * -1;
            WWW www = new WWW(url + urlMethodSet + urlAmount + (int)(slider.value*100));
        }
        else
        {
            slider.value = 0;
        }
    }

    void onHostnameChanged(string text)
    {
        hostname = text;
        url = hostname + ":" + port + "/?";
        PlayerPrefs.SetString("hostname", hostname);
    }

    void onPortChanged(string text)
    {
        port = text;
        url = hostname + ":" + port + "/?";
        PlayerPrefs.SetString("port", port);
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
