using UnityEngine;

/*
 * This class makes sure the indicator to find the next box is working correctly even when it is behind the user
*/

public class ObjectIndicatorAudioWithFilter : MonoBehaviour
{
    public GameObject Cursor;
    public GameObject NextBox;
    public float LowestCutoffFrequency;
    public float HighestCutoffFrequency;
    
    private bool targetBehind;
    private AudioSource audioSource;
    private AudioLowPassFilter lowPassFilter;



    public void Update()
    {
        CalculateAudioChanges();
    }


    public void SetNextTarget(GameObject nextTarget)
    {
        NextBox = nextTarget;
        audioSource = NextBox.GetComponent<AudioSource>();
        lowPassFilter = NextBox.GetComponent<AudioLowPassFilter>();
        lowPassFilter.cutoffFrequency = CalculateCutoffFrequency(Camera.main.transform.forward.y * 2, NextBox.transform.position.y); //Using Camera.main.transform.forward.y * 2 here because we want to approximate the height the úser is looking at at an distance of z=2 to be equal to the boxes position (z=2)
        if (Camera.main.transform.forward.z * NextBox.transform.position.z < 0) //Case when Box is behind the User (+*+>0 & -*->0 & +*-.<0 & -*+<=)
        {
            audioSource.volume = 0.0f;
            targetBehind = true;
        }
        else //Box in front
        {
        }
    }

    private void CalculateAudioChanges()
    {
        lowPassFilter.cutoffFrequency = CalculateCutoffFrequency(Camera.main.transform.forward.y * 2, NextBox.transform.position.y);
        if (targetBehind)
        {
            //audioSource.volume = 0.1f + 0.4f * TriangularFunction(cursorPosition.z); //0.4f because i want volume to be between 0.1 and 0.5
            if (Camera.main.transform.forward.z > -0.1f &&
                Camera.main.transform.forward.z < 0.1f) //cursorPosition.z is 0 at the start so we use Camera.main.transform.forward.z here
            {
                audioSource.volume = 0.5f;
                targetBehind = false;
            }
        }
        else
        {
        }
    }
    private float CalculateCutoffFrequency(float gazePosition, float boxPosition) //Calculates the value of the lowpassfilter
    {
        float diff = gazePosition - boxPosition;
        return LowestCutoffFrequency + HighestCutoffFrequency * Mathf.Min(1, Mathf.Max(0.5f - diff, 0));
    }
}
