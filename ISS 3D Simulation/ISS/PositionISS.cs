using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Boomlagoon.JSON;

public class PositionISS : MonoBehaviour {

  public GameObject earthParent;
  public GameObject earth;

  private string issApiUrl = "http://api.open-notify.org/iss-now.json";
  private int positionsToRecord = 3;
  private Vector2[] recentISSPositions = new Vector2[5];
  private IEnumerator coroutine;

  void Start ()
  {
    InvokeRepeating("CallISSAPI", 0f, 1.0f);
    InitialISSPosition();
    InvokeRepeating("SetISSPosition", positionsToRecord, 1.0f);
  }

  void CallISSAPI ()
  {
    if(coroutine != null) StopCoroutine(coroutine);
    coroutine = GetISSDataFromAPI();
    StartCoroutine(coroutine);
  }

  IEnumerator GetISSDataFromAPI ()
  {
    WWW www = new WWW (issApiUrl);
    yield return www;
    if (www.text != "") {
      Vector2 position = ParseISSJSONData(www.text);
      AddPositionToArray(position);
    } else {
      Debug.Log("Error: " + www.error);
    }
  }

  Vector2 ParseISSJSONData (string apiText)
  {
    JSONObject json = JSONObject.Parse(apiText);
    JSONObject iss_position = json.GetObject("iss_position");
    float longitude = float.Parse(iss_position.GetString("longitude"));
    float latitude = float.Parse(iss_position.GetString("latitude"));
    return new Vector2(longitude, latitude);
  }

  void InitialISSPosition () {

  }

  void SetISSPosition ()
  {
    float latitude = recentISSPositions[positionsToRecord-1][1];
    float longitude = recentISSPositions[positionsToRecord-1][0];

    iTween.RotateTo(earthParent, iTween.Hash(
      "rotation", new Vector3(0,0,latitude),
      "time", 1.0f,
      "easeType", "linear"
    ));

    iTween.RotateTo(earth, iTween.Hash(
      "rotation", new Vector3(0, longitude-90, 0),
      "time", 1.0f,
      "easeType", "linear",
      "islocal", true
    ));
  }

  void AddPositionToArray (Vector2 newItem)
  {
    for (int i = positionsToRecord-1; i > 0; i--) {
      recentISSPositions[i] = recentISSPositions[i-1];
    }
    recentISSPositions[0] = newItem;
  }
}
