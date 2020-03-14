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
    if (anObject != null) angle = anObject.angle;
    return angle;
}


public float GetLinearOuput()
{
    if (anObject != null) strength = 1.0f / (anObject.distance + 1.0f);
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


public ObjectInfo[] GetVisibleFloor()
{
    return (ObjectInfo[])GetVisibleObjects("Floor").ToArray();
}

public ObjectInfo DetectFloor()
{
    ObjectInfo[] a = (ObjectInfo[])GetVisibleObjects("Floor").ToArray();
    if (a.Length == 0)
    {
        return null;
    }
    return a[a.Length - 1];
}

public List<ObjectInfo> GetVisibleObjects(string objectTag)
{
    RaycastHit hit;
    List<ObjectInfo> objectsInformation = new List<ObjectInfo>();

    for (int i = 0; i * angleOfSensors < 360f; i++)
    {
        if (Physics.Raycast(this.transform.position, Quaternion.AngleAxis(-angleOfSensors * i, initialTransformUp) * initialTransformFwd, out hit, rangeOfSensors))
        {

            if (hit.transform.gameObject.CompareTag(objectTag))
            {
                if (debug_mode)
                {
                    Debug.DrawRay(this.transform.position, Quaternion.AngleAxis((-angleOfSensors * i), initialTransformUp) * initialTransformFwd * hit.distance, Color.green);
                }
                ObjectInfo info = new ObjectInfo(hit.distance, angleOfSensors * i + 90);
                objectsInformation.Add(info);
            }
        }
    }

    objectsInformation.Sort();

    return objectsInformation;
}


private void LateUpdate()
{
    this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.transform.parent.rotation.z * -1.0f);

}
}
