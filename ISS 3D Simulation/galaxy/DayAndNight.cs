using System.Collections;
using UnityEngine;
using System;

public class DayAndNight : MonoBehaviour
{
  public Transform light = null;
  public MeshRenderer earthRenderer = null;
  public MeshRenderer atmosphereRenderer = null;
  public MeshRenderer miniEarthRenderer = null;

  private float tropicsDegrees = 23.5f;

  // This is a 2Pi estimate so I don't have to call Math.pi all the time
  private float pi2 = 6.28318f;

  // WARNING: This is an ESTIMATE! But I think it's good enough for v1 of a basic animation.
  //
  // It doesn't take into consideration:
  // - the time between summer/winter equinox ISN'T 365/2
  // - leap year
  // - the fact that pi has more than 5 decimal places
  //
  // I based this on the summer equinox being the 171th day of the year
  // So 171/365=.468, so summer equinox is 46.8% of the way through the year
  // The lenght of the cosine curve is 2pi or about 6.28318
  // To make sure Jun 21 is the top of the cosine curve, I multiply 2pi * 46.8%
  private float summerEquinoxCosOffset = 2.94f;

  void Start ()
  {
    // The earth only rotates about .04 degrees in 10 seconds so this is more than enough.
    InvokeRepeating("RotateSun", 0, 1.0f);
  }

  void RotateSun ()
  {
    DateTime now = DateTime.UtcNow;
    float xRotation = GetSeasonRotation(now);
    // float xRotation = GetSeasonRotation(new DateTime(2017,6,21));

    float timeDirection = ((float)now.Minute + (float)now.Hour * 60)/1440*360;
    light.eulerAngles = new Vector3(xRotation, timeDirection+90, 0);
    UpdateEarthMaterials();
  }

  float GetSeasonRotation (DateTime now)
  {
    // On summer equinox the sun should tilt downward 23.5 degrees (facing the tropic of cancer)
    // On winter equinox the sun should tilt upward 23.5 degrees (facing the tropic of capricorn)
    // The positions between should be a cosin curve, rather than linear

    //      June 21
    //         |
    //        ___         ____ tropic of cancer
    //      /     \
    //     /       \      ---- equator
    //  __/         \__   ____ tropic of capricorn

    float percentageOfYear = now.DayOfYear/365f;
    float positionOnCosineCurve = Mathf.Cos(percentageOfYear * pi2 + summerEquinoxCosOffset);
    return positionOnCosineCurve * -tropicsDegrees;


  }

  void UpdateEarthMaterials ()
  {
    Vector3 lightDir = Quaternion.Inverse(light.rotation) * Vector3.forward;
    earthRenderer.material.SetVector("_LightDir", lightDir);
    miniEarthRenderer.material.SetVector("_LightDir", lightDir);
    atmosphereRenderer.material.SetVector("_LightDir", lightDir);
  }
}
