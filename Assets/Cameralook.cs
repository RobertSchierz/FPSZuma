using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameralook : MonoBehaviour {


    [Range(0.0f, 2.0f)]
    public float sensitivity = 1.0f;
    public float minimumX = -360F;
    public float maximumX = 360F;

    float rotationX = 0F;


    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;

    public float frameCounter = 20;

    Quaternion originalRotation;

 
    void Start () {
        originalRotation = transform.localRotation;

    }
	

	void Update () {

        rotAverageX = 0f;

        rotationX += Input.GetAxis("Mouse X") * sensitivity;

        rotArrayX.Add(rotationX);

        if (rotArrayX.Count >= frameCounter)
        {
            rotArrayX.RemoveAt(0);
        }
        for (int i = 0; i < rotArrayX.Count; i++)
        {
            rotAverageX += rotArrayX[i];
        }
        rotAverageX /= rotArrayX.Count;

        rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

        Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
        transform.localRotation = originalRotation * xQuaternion;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }

}
