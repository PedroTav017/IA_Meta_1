using System;
public class ObjectInfo :  IComparable<ObjectInfo>
{
    public float distance {get;}
    public float angle { get; }

    public ObjectInfo(float distance, float angle)
    {
        this.distance = distance;
        this.angle = angle;
    }


    public int CompareTo(ObjectInfo other)
    {
        if (this.distance < other.distance)
        {
            return 1;
        }
        else if (this.distance == other.distance)
        {
            return 0;
        }
        return -1;
    }
}
