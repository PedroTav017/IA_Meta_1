using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetectorScript : MonoBehaviour
{

    public float angleOfSensors = 10f;
    public float rangeOfSensors = 0.1f;
    protected Vector3 initialTransformUp;
    protected Vector3 initialTransformFwd;
    public float strength;
    public float angleToClosestObj;
    public int numObjects;
    public bool debugMode;

    private ObjectInfo anObject;

    // Start is called before the first frame update
    void Start()
    {

        initialTransformUp = this.transform.up;
        initialTransformFwd = this.transform.forward;
        Debug.Log(initialTransformUp + " | " + initialTransformFwd);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        anObject = GetClosestBlock();
        if (anObject == null) // no object detected
        { 
            strength = 0;
            angleToClosestObj = 0;
        }
    }

    public float GetAngleToClosestObstacle()
    {
        if (anObject != null) angleToClosestObj = anObject.angle;
        return angleToClosestObj;
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
        // YOUR CODE HERE
        throw new NotImplementedException();
    }

    // NEW CODE BELOW 

    public ObjectInfo[] GetVisibleBlocks()
    {
        return (ObjectInfo[])GetVisibleObjects("Wall").ToArray();
    }

    public ObjectInfo GetClosestBlock()
    {
        ObjectInfo[] a = (ObjectInfo[])GetVisibleObjects("Wall").ToArray();
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
                    if (debugMode)
                    {
                        Debug.DrawRay(this.transform.position, Quaternion.AngleAxis((-angleOfSensors * i), initialTransformUp) * initialTransformFwd * hit.distance, Color.blue);
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
