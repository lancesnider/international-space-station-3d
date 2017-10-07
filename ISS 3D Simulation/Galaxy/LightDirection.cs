using System.Collections;
using UnityEngine;
using System;

public class LightDirection : MonoBehaviour {

  private Transform sun;
  public int sunDistance = 100;

  public bool useCurrentTime;

  public Transform earthTransform;
  public MeshRenderer earthRenderer;
  private Material earthMaterial;

  [Range(0,24)]
  public float timeOfDay;
  [Range(1,365)]
  public int dayOfYear;

  // The tropics of Cancer and Capricorn are +/- 23.5 lat
  private float tropicsDegrees = 23.5f;

  // This is a 2Pi estimate so I don't have to call Math.pi all the time
  private float pi2 = 6.28318f;

  // WARNING: This is an ESTIMATE! But I think it's good enough for v1 of a basic animation.
  //
  // It doesn't take into consideration:
  // - the time between summer/winter Solstice ISN'T 365/2
  // - leap year
  // - the fact that pi has more than 5 decimal places :)
  //
  // I based this on the summer Solstice being the 171th day of the year
  // Summer Solstice is 46.8% of the way through the year (171/365=.468)
  // The length of the cosine curve is 2pi or about 6.28318
  // To make sure Jun 21 is the top of the cosine curve, I multiply 2pi * 46.8%
  private float summerSolsticeCosOffset = 2.94f;

	void Start () {
    sun = transform;
    earthMaterial = earthRenderer.material;
	}

  void Update () {
    if (useCurrentTime)
      SetCurrentDateTime();

    float yRotation = ConvertTimeOfDayToDirection();
    float xRotation = ConvertDayOfYearToDirection();

    RotateSun(new Vector3(xRotation, yRotation, 0));
    UpdateEarthMaterials();
  }

  void SetCurrentDateTime () {
    DateTime now = DateTime.UtcNow;
    // 1440 minutes in a day
    timeOfDay = ((float)now.Minute + (float)now.Hour * 60)/1440 * 24;
    dayOfYear = now.DayOfYear;
  }

  float ConvertTimeOfDayToDirection () {
    return timeOfDay/24 * 360;
  }

  float ConvertDayOfYearToDirection () {
    // On summer Solstice the sun should tilt downward 23.5 degrees (facing the tropic of cancer)
    // On winter Solstice the sun should tilt upward 23.5 degrees (facing the tropic of capricorn)
    // The positions between should be a cosin curve, rather than linear

    //   Summer Solstice
    //         |
    //        ___         ____ tropic of cancer
    //      /     \
    //     /       \      ---- equator
    //  __/         \__   ____ tropic of capricorn
    //               |
    //          Winter Solstice

    float percentageOfYear = dayOfYear/365f;
    float positionOnCosineCurve = Mathf.Cos(percentageOfYear * pi2 + summerSolsticeCosOffset);

    return positionOnCosineCurve * tropicsDegrees;
  }

  void RotateSun (Vector3 lightRotation) {
    Quaternion quaterionSunRotation = Quaternion.Euler(lightRotation);
    Vector3 negativeDistance = new Vector3(0.0f, 0.0f, -sunDistance);

    sun.eulerAngles = lightRotation;
    sun.position = quaterionSunRotation * negativeDistance;
  }

  void UpdateEarthMaterials () {
    Vector3 lightDir = sun.rotation * Vector3.forward * -1;
    earthMaterial.SetVector("_LightDir", lightDir);
  }
}
