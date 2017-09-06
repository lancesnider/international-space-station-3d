using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Boomlagoon.JSON;

public class PositionISS : MonoBehaviour {

  public Transform earthParent;
  public Transform earth;

  // Link to the API for the ISS position
  private string issApiUrl = "http://api.open-notify.org/iss-now.json";

  private Vector2 lastISSPosition;
  private float lastAPITime;
  private Vector2 issPositionDelta;
  private Vector3 velocity;

  void Start ()
  {
    StartCoroutine (GetISSDataFromAPI());
  }

  void Update ()
  {
    EstimateISSPosition();
  }

  IEnumerator GetISSDataFromAPI ()
  {
    while (true) {
      WWW www = new WWW (issApiUrl);
      yield return www;
      if (www.text != "") {
        ParseISSJSONData(www.text);
      } else {
        Debug.Log("Error: " + www.error);
      }
      yield return new WaitForSeconds(1.0f);
    }
  }

  void ParseISSJSONData (string apiText)
  {
    JSONObject json = JSONObject.Parse(apiText);
    JSONObject iss_position = json.GetObject("iss_position");
    float longitude = float.Parse(iss_position.GetString("longitude"));
    float latitude = float.Parse(iss_position.GetString("latitude"));

    SetISSPosition(longitude, latitude);
    DetermineVelocity(longitude, latitude);
  }

  void EstimateISSPosition ()
  {
    if(lastISSPosition == new Vector2(0,0)) return;

    float timeElapsed = Time.time - lastAPITime;
    float scale = timeElapsed/velocity[2];
    Debug.Log(velocity);
    Debug.Log(velocity[0] * scale);
    float longitude = lastISSPosition[0] + velocity[0] * scale;
    float latitude = lastISSPosition[1] + velocity[1] * scale;
    SetISSPosition(longitude, latitude);
  }

  void SetISSPosition (float longitude, float latitude)
  {
    earthParent.eulerAngles = new Vector3(0, 0, latitude);
    earth.localEulerAngles = new Vector3(0, longitude-90, 0);
  }

  void DetermineVelocity (float longitude, float latitude)
  {
    float timeNow = Time.time;
    float timeDifference = timeNow - lastAPITime;
    lastAPITime = timeNow;

    Vector2 currentISSPosition = new Vector2(longitude, latitude);
    issPositionDelta = currentISSPosition - lastISSPosition;
    lastISSPosition = currentISSPosition;

    velocity = new Vector3(issPositionDelta[0], issPositionDelta[1], timeDifference);
  }

}
