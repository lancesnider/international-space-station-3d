using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class ISSPosition : MonoBehaviour {

	public float latitude;
	public float longitude;
	public double timestamp;

	public GameObject earthParent;
	public GameObject earth;

	public int iss_altitude = 420;
	public int earth_radius = 6371;
	public string url = "http://api.open-notify.org/iss-now.json";

	void Start () {
		StartCoroutine (WaitForRequest());
	}

	IEnumerator WaitForRequest ()
	{

		while (true) {
			WWW www = new WWW (url);
			yield return www;
			if (www.text != "") {
				JSONObject json = JSONObject.Parse(www.text);
				timestamp = json.GetNumber("timestamp");
				JSONObject iss_position = json.GetObject("iss_position");
				longitude = float.Parse(iss_position.GetString("longitude"));
				latitude = float.Parse(iss_position.GetString("latitude"));
				Debug.Log(iss_position);
				rotateEarth();
			} else {
				Debug.Log ("Error poooo: " + www.error);
			}

			yield return new WaitForSeconds(1.0f);
		}
	}

	void rotateEarth ()
	{
		earthParent.transform.eulerAngles = new Vector3(0, 0, latitude);
		earth.transform.localEulerAngles = new Vector3(0, longitude-90, 0);
	}
}

