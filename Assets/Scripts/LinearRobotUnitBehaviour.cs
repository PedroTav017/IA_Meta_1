using System;
using System.Collections;
using UnityEngine;

public class LinearRobotUnitBehaviour : RobotUnit
{
    public enum ActivationType { None, Linear, Gaussian, NegLog };
    public ActivationType activationResource;
    public ActivationType activationBlock;

    // Limiares/thresholds (cortes verticais)
    private const float min_threshold = 0.25f;
    private const float max_threshold = 0.75f;
    // Limites/limits (cortes horizontais)
    private const float min_limit = 0.05f;
    private const float max_limit = 0.6f;

    public float weightResource;
    public float resourceValue;
    public float resouceAngle;

    public float weightBlock;
    public float blockValue;
    public float blockAngle;

    public float weightFloor;
    public float floorValue;
    public float floorAngle;

    void Update()
    {

        // force for resource/pickup
        // get sensor data
        resourceValue = 0;
        switch (activationResource)
        {
            case ActivationType.Linear:
                resourceValue = weightResource * resourcesDetector.GetLinearOuput();
                break;
            case ActivationType.Gaussian:
                resourceValue = weightResource * resourcesDetector.GetGaussianOutput();
                break;
            case ActivationType.NegLog:
                resourceValue = weightResource * resourcesDetector.GetLogaritmicOutput();
                break;
        }

        float strengthResource = resourceValue;
        if (activationResource == ActivationType.None)
        {
            if (strengthResource < min_threshold) strengthResource = min_threshold;
            if (strengthResource > max_threshold) strengthResource = max_threshold;
        }


        resouceAngle = resourcesDetector.GetAngleToClosestResource();

        // apply to the ball
        applyForce(resouceAngle, strengthResource); // go towards






        /**************************************************/

        // force for wall/blocks
        // get sensor data
        blockAngle = blockDetector.GetAngleToClosestObstacle();

        blockValue = weightBlock * blockDetector.GetLinearOuput();

        // apply to the ball
        applyForce(blockAngle, -blockValue); // go the opposite way






        /**************************************************/

        // force for floor
        // get sensor data
        floorAngle = floorDetector.GetAngleOfFloor();

        floorValue = weightFloor * floorDetector.GetLinearOuput();

        // apply to the ball
        applyForce(floorAngle, -floorValue); // go the opposite way
    }


}






