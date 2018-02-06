using UnityEngine;

/*
 * This class makes sure the indicator to find the next box is working correctly even when it is behind the user
*/

public class ObjectIndicatorVisual : MonoBehaviour
{
    public GameObject Cursor;
    public LineRenderer LineRenderer;
    public GameObject DirectionIndicatorObject;
    public GameObject NextBox;
    
    private float distancePercent;
    private bool targetBehind;
    private Vector3 cursorPosition;
    private Vector3 cursorIndicatorDirection;
    private Vector3 middlePoint;
    private Vector3 lineTargetPosition;
    private Vector3 middlePointToTarget;
    private Quaternion directionIndicatorDefaultRotation;
    private bool directionIndicatorVisible;

    public void Awake()
    {
        DirectionIndicatorObject = InstantiateDirectionIndicator(DirectionIndicatorObject);
    }

    private GameObject InstantiateDirectionIndicator(GameObject directionIndicator)
    {
        GameObject indicator = Instantiate(directionIndicator);
        directionIndicatorDefaultRotation = indicator.transform.rotation;
        return indicator;
    }

    public void Update()
    {       
        Vector3 camToObjectDirection = NextBox.transform.position - Cursor.transform.position;  // Vector between cursorPosition and NextBox
        camToObjectDirection.Normalize(); //Same Direction but Magnitude/Length = 1
        DirectionIndicatorObject.transform.rotation = GetDirectionIndicatorRotation();
        DirectionIndicatorObject.transform.position = Cursor.transform.position;
    }

    public void SetNextTarget(GameObject nextTarget)
    {
        LineRenderer.material.color = Color.green;
        DirectionIndicatorObject.GetComponent<SpriteRenderer>().material.color = Color.green;
        cursorPosition = Cursor.transform.position;
        NextBox = nextTarget;
        if (Camera.main.transform.forward.z * NextBox.transform.position.z < 0) //Case when Box is behind the User (+*+>0 & -*->0 & +*-.<0 & -*+<=)
        {
            float crossProduct = Vector3.Cross(cursorPosition, NextBox.transform.position - cursorPosition).y;       
            targetBehind = true;
            middlePoint = NextBox.transform.position;
            middlePoint.z = 0;
            if (crossProduct <= 0 && cursorPosition.z > 0 || crossProduct > 0 && cursorPosition.z < 0) //Target left in global space
            {
                middlePoint.x = -1.5f; 
            }
            else //Target right in global space
            {
                middlePoint.x = 1.5f; 
            }
            middlePoint.y = (middlePoint.y + cursorPosition.y) / 2;
            middlePointToTarget = NextBox.transform.position - middlePoint; //Vector between NextBox and middlePoint
        }
        else //Box in front
        {
            
        }
    }

    private Quaternion GetDirectionIndicatorRotation()
    {
        cursorPosition = Cursor.transform.position;
        if (targetBehind)
        {
            distancePercent = CalculateDistance(cursorPosition.z);
            cursorIndicatorDirection = (middlePoint + distancePercent * middlePointToTarget) - cursorPosition; //Vector between current middlePoint and cursorPosition
            cursorIndicatorDirection.Normalize();
            lineTargetPosition = middlePoint + distancePercent * middlePointToTarget;
            if (Camera.main.transform.forward.z > -0.1f &&
                Camera.main.transform.forward.z < 0.1f) //cursorPosition.z is 0 at the start so we use Camera.main.transform.forward.z here
            {
                targetBehind = false;
            }
        }
        else
        {
            cursorIndicatorDirection = NextBox.transform.position - cursorPosition;
            cursorIndicatorDirection.Normalize();
            lineTargetPosition = NextBox.transform.position;
        }
        Vector3[] positions = new[] {cursorPosition, lineTargetPosition};
        LineRenderer.SetPositions(positions);
        return Quaternion.LookRotation(Camera.main.transform.forward, cursorIndicatorDirection) *
                   directionIndicatorDefaultRotation;
    }

    private float CalculateDistance(float zValue) //Approximates how near the User is to the original position of the middlePoint based on the Z-Coordinate
    {
        return Mathf.Max(1 - (Mathf.Abs(zValue) / 1.5f), 0);
    }

    public void OnDestroy()
    {
        Destroy(DirectionIndicatorObject);
    }
}
