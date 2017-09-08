using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Boomlagoon.JSON;

public class GetISSData : MonoBehaviour {

  private string issApiUrl = "http://api.open-notify.org/iss-now.json";
  private IEnumerator coroutine;
  public Vector2[] recentISSPositions = new Vector2[5];
  private bool isInitialPosition = true;
  private PositionISS positionScript;

	// Use this for initialization
	void Start () {
    positionScript = GetComponent<PositionISS>();
		InvokeRepeating("CallISSAPI", 0f, 1.0f);
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

  void AddPositionToArray (Vector2 newItem)
  {
    if (isInitialPosition == true) {
      positionScript.AnimateToInitialISSPosition(newItem);
      isInitialPosition = false;
    }

    for (int i = positionScript.positionsToRecord-1; i > 0; i--) {
      recentISSPositions[i] = recentISSPositions[i-1];
    }
    recentISSPositions[0] = newItem;
  }
}
