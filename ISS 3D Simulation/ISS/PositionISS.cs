using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionISS : MonoBehaviour {

  public GameObject earthParent;
  public GameObject earth;
  public GameObject target;
  public bool introAnimationEnabled = true;

  private GetISSData getDataScript;
  public int positionsToRecord = 3;

  void Start ()
  {
    getDataScript = GetComponent<GetISSData>();

    if (introAnimationEnabled == true ) {
      earthParent.transform.localScale = new Vector3(.25f, .25f, .25f);
      transform.localScale = new Vector3(.001f, .001f, .001f);
    }

    InvokeRepeating("SetISSPosition", positionsToRecord, 1.0f);
  }

  public void AnimateToInitialISSPosition (Vector2 position)
  {
    if (introAnimationEnabled == true ) {
      iTween.ScaleTo(earthParent, iTween.Hash("scale", Vector3.one,"time", 4.0f,"delay", 2.0f,"easeType", "easeInOutSine"));
      iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one, "time", 1.0f, "delay", 5.0f, "easeType", "easeOutSine"));
    }

    iTween.ScaleFrom(target, iTween.Hash("scale", new Vector3(50,50,50), "time", 4.0f, "delay", 2.0f,"easeType", "easeInOutSine"));
    // Subtract the time it took to load the initial data. Otherwise, you get overlapping animations.
    AnimateISS(position[0], position[1], positionsToRecord - Time.time, "easeInOutSine");

  }

  void SetISSPosition ()
  {
    float latitude = getDataScript.recentISSPositions[positionsToRecord-1][1];
    float longitude = getDataScript.recentISSPositions[positionsToRecord-1][0];
    AnimateISS(longitude, latitude, 1.0f, "linear");
  }

  void AnimateISS (float longitude, float latitude, float duration, string easeType)
  {
    iTween.RotateTo(earthParent, iTween.Hash("rotation", new Vector3(0,0,latitude), "time", duration, "easeType", easeType));
    iTween.RotateTo(earth, iTween.Hash("rotation", new Vector3(0, longitude-90, 0), "time", duration, "easeType", easeType, "islocal", true));
  }
}
