using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetectorScript : MonoBehaviour
{ 
public float angleOfSensors = 10f;
public float rangeOfSensors = 0.1f;
protected Vector3 initialTransformUp;
protected Vector3 initialTransformFwd;
public float strength;
public float angle;
public int numObjects;
public bool debug_mode;
public bool hasFloor;

private ObjectInfo anObject;
// Start is called before the first frame update
void Start()
{
    initialTransformUp = this.transform.up;
    initialTransformFwd = this.transform.forward;
}

    // FixedUpdate is called at fixed intervals of time
void FixedUpdate()
{
        anObject = DetectFloor();

        if (anObject == null) // no object detected
        {
            strength = 0;
            angle = 0;
        }

    }

public float GetAngleOfFloor()
{
    if (anObject != null) angle = anObject.angle + 180;
    return angle;
}


public float GetLinearOuput()
{
        if (anObject != null) strength = 5;
        return strength;
}

public virtual float GetGaussianOutput()
{
    return strength;
}

public virtual float GetLogaritmicOutput()
{
    return strength;
}


public ObjectInfo DetectFloor()
{
    RaycastHit hit;
        ObjectInfo result = null;

        for (int i = 0; i * angleOfSensors < 360f; i++)
        {
            Vector3 offset = new Vector3(0, -1f, 0);
            if (Physics.Raycast(this.transform.position, (Quaternion.AngleAxis(-angleOfSensors * i, initialTransformUp) * initialTransformFwd) + offset , out hit, rangeOfSensors))
        {

                Debug.DrawRay(this.transform.position, (Quaternion.AngleAxis((-angleOfSensors * i), initialTransformUp) * initialTransformFwd + offset) * hit.distance, Color.green);


                result = new ObjectInfo(hit.distance, angleOfSensors * i + 90);

                if (hit.transform.gameObject.CompareTag("Floor"))
                {
                    result = null;
                }
                else {
                    return result;
                }
            }
    }
        return result;
}


private void LateUpdate()
{
    this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.transform.parent.rotation.z * -1.0f);

}
}
