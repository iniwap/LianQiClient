using UnityEngine;
using System.Collections;

public class ControlUtils{


	public static Vector3 Pos2ZPlane(float z = 0.0f)
	{
		Vector2 mPos = Input.mousePosition;

		return ScreenPixelPos2ZPlane (mPos, z, Camera.main);
	}

    public static Vector3 ScreenPixelPos2ZPlane(Vector2 pos, float z = 0.0f, Camera cam = null)
	{
        if(cam == null)
        {
            cam = Camera.main;
        }
		Ray R  = cam.ScreenPointToRay (pos);
		Vector3 posOnZPlane = 
			R.origin + ((z - R.origin.z) / R.direction.z )* R.direction;

		return posOnZPlane;
	}

}
