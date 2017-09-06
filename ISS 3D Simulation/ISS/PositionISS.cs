using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionISS : MonoBehaviour {

  public Transform earth;

  // Link to the API for the ISS position
  private string issApiUrl = "http://api.open-notify.org/iss-now.json";
  private Vector3 lastISSSpacetime;
  private Vector3 issVelocity = new Vector3(0,0,0);

  void Start ()
  {
    StartCoroutine (GetISSDataFromAPI());
  }

  void Update ()
  {

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
    Debug.Log(apiText);
    SetISSPosition();
  }

  void SetISSPosition ()
  {

  }

  void ApplyISSVelocity ()
  {
    /*
      We want the animation to look smooth, but we only know position of ISS once every second.

      To keep the movement from looking choppy, we use the last 2 positions to determine its velocity.
      The ISS will move in that direction until the velocity is updated.

      This method is also useful for slow or lost connections where we can't gurantee an update every
      second.
    */
  }

}
