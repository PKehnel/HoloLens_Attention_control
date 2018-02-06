using UnityEngine;

/*
 * This class makes sure the indicator to find the next box is working correctly even when it is behind the user
*/

public class ObjectIndicatorAudio : MonoBehaviour
{
    public GameObject Cursor;
    public GameObject NextBox;
    
    private bool targetBehind;
    private AudioSource audioSource;



    public void Update()
    {
        CalculateAudioChanges();
    }


    public void SetNextTarget(GameObject nextTarget)
    {
        NextBox = nextTarget;
        audioSource = NextBox.GetComponent<AudioSource>();
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
        if (targetBehind)
        {
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
}
