using System.Collections;
using UnityEngine;
using System;

public class DayAndNight : MonoBehaviour
{
  public Transform light = null;
  public MeshRenderer earthRenderer = null;
  public MeshRenderer atmosphereRenderer = null;
  public MeshRenderer miniEarthRenderer = null;

  void Start()
  {
    // The earth only rotates about .04 degrees in 10 seconds so this is more than enough.
    InvokeRepeating("RotateSun", 0, 10.0f);
  }

  void RotateSun()
  {
    DateTime now = DateTime.UtcNow;
    float timeDirection = ((float)now.Minute + (float)now.Hour * 60)/1440*360;
    light.eulerAngles = new Vector3(0, timeDirection+90, 0);
    UpdateEarthMaterials();
  }

  void UpdateEarthMaterials()
  {
    Vector3 lightDir = Quaternion.Inverse(light.rotation) * Vector3.forward;
    earthRenderer.material.SetVector("_LightDir", lightDir);
    miniEarthRenderer.material.SetVector("_LightDir", lightDir);
    atmosphereRenderer.material.SetVector("_LightDir", lightDir);
  }
}
