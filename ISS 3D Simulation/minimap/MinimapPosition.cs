using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPosition : MonoBehaviour {

  public Transform earthParent;
  public Transform earth;
  private Transform miniEarthParent;
  public Transform miniEarth;

	// Use this for initialization
	void Start () {
    miniEarthParent = transform;
	}

	// Update is called once per frame
	void Update () {
    miniEarthParent.rotation = earthParent.rotation;
    miniEarth.localRotation = earth.localRotation;
	}
}
