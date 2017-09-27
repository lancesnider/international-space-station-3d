using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour {

  public Transform target;
  private Vector3 targetPosition;
  private Transform thisCamera;
  private float cameraAngleX = 0.0f;
  private float cameraAngleY = 0.0f;
  public float rotationSpeed = 15f;
  public float distance = 0.5f;

  private bool mouseDown;

  private Vector2 startDragPosition;

  void Start ()
  {
    targetPosition = target.position;
    thisCamera = transform;
  }

	void LateUpdate ()
  {
		Vector2 dragDelta = GetMouseInput();
    if (dragDelta != Vector2.zero)
      OrbitAroundObject(dragDelta);
	}

  Vector2 GetMouseInput ()
  {
    if (Input.GetMouseButtonDown(0)) {
      mouseDown = true;
      startDragPosition = Input.mousePosition;
      Vector3 cameraAngle = thisCamera.eulerAngles;
      cameraAngleX = cameraAngle.x;
      cameraAngleY = cameraAngle.y;
    }else if (Input.GetMouseButtonUp(0)) {
      mouseDown = false;
    }

    if (mouseDown == true) {
      return (Vector2)Input.mousePosition - startDragPosition;
    }

    return Vector2.zero;
  }

  void OrbitAroundObject (Vector2 dragDelta)
  {

    float newAngleX = cameraAngleX + dragDelta.y/10;
    float newAngleY = cameraAngleY - dragDelta.x/10;


    Quaternion rotation = Quaternion.Euler(newAngleX, newAngleY, 0);

    Vector3 negativeDistance = new Vector3(0.0f, 0.0f, -distance);
    Vector3 position = rotation * negativeDistance + targetPosition;

    thisCamera.rotation = rotation;
    thisCamera.position = position;
  }
}