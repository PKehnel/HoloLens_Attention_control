using HoloToolkit.Unity.InputModule;
using UnityEngine;
using Random = System.Random;
using System;
using UnityEngine.SceneManagement;

#if WINDOWS_UWP
using Windows.Storage;
using Windows.System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
#endif

/*
 * This class is used to change the searched box when the user is looking at it and saying "next"
 * It is also responsible for writing the score to a file and changing to the next configuration by calling a method in the LevelManager.cs file
*/

public class BoxManagerAudioWithFilter : BoxManagerAbstract
{

    public ObjectIndicatorAudioWithFilter Indicator;
    public float TimeRemaining; //Time the level lasts
    public Material NormalMaterial;
    public Material HighlightedMaterial; //Material changes when user gazed at next Box
    public GazeManager GazeManager;
    public KeywordManager KeywordManager;
    public bool CountForExperiment; //Data should be written to file if this is true

    private GameObject[] boxes;
    private GameObject nextBox;
    private Random rnd; 
    private int numberOfNextBox; //The Random Selected Number
    private int score;
    private string scoreText;
    private LevelManager levelManager;

    // Use this for initialization
    void Start ()
    {
        InitializeBoxes();
    }

    // Update is called once per frame
    void Update()
    { //If time is over write to file and go to next level
        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining < 0)
        {
            if (CountForExperiment)
            {
                WriteToHoloLens();
            }
            else
            {
                levelManager.LoadNextLevel(); //Since levelManager doesn't get destroyed when another scene is loaded you can use it here  
            }
        }
    }

    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void InitializeBoxes()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        boxes = GameObject.FindGameObjectsWithTag("Box");
        rnd = new Random();
        numberOfNextBox = rnd.Next(0, boxes.Length); //All between 0 and amount of boxes because first parameter is inclusive and second exclusive
        nextBox = boxes[numberOfNextBox];
        nextBox.GetComponent<BoundBoxes_BoundBox>().enabled = true;
        nextBox.GetComponent<AudioSource>().enabled = true;
        Indicator.SetNextTarget(nextBox);
    }

    public override void CheckIfBoxHit(GameObject hitObject)
    {
        if (nextBox.Equals(hitObject))
        {
            hitObject.GetComponent<Renderer>().material = HighlightedMaterial;
            KeywordManager.StartKeywordRecognizer();
        }
    }

    public void ChangeNextBox()
    {
        if (GazeManager.IsGazingAtObject) //Only if the User is looking at the Object and says next this code gets executed
        {
            score++;
            nextBox.GetComponent<BoundBoxes_BoundBox>().enabled = false;
            nextBox.GetComponent<Renderer>().material = NormalMaterial;
            nextBox.GetComponent<AudioSource>().enabled = false;
            GetNextBox();
            nextBox = boxes[numberOfNextBox];
            nextBox.GetComponent<BoundBoxes_BoundBox>().enabled = true;
            nextBox.GetComponent<AudioSource>().enabled = true;
            Indicator.SetNextTarget(nextBox);
            KeywordManager.StopKeywordRecognizer();
        }
    }

    private void GetNextBox() //Makes sure that the same Box isn't selected twice in a row
    {
        int rand = rnd.Next(0, boxes.Length);
        if (rand != numberOfNextBox)
        {
            numberOfNextBox = rand;
        }
        else
        {
            GetNextBox();
        }
    }

    private void WriteToHoloLens() //Write score to a file
    {
#if WINDOWS_UWP

             Task task = new Task(

            async () =>
            {                        
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile textFileForWriteAndRead = await storageFolder.CreateFileAsync("BoxManagerAudioWithFilter.txt",
        Windows.Storage.CreationCollisionOption.OpenIfExists); //This CollisionOption makes sure that if file already exists it opens this one instead of overwriting it
            scoreText = await FileIO.ReadTextAsync(textFileForWriteAndRead);
            scoreText = scoreText + "\r\n" + score.ToString();
            await FileIO.WriteTextAsync(textFileForWriteAndRead, scoreText);
            });
            task.Start();
            task.Wait();

#endif
        levelManager.LoadNextLevel();  //Since levelManager doesn't get destroyed when another scene is loaded you can use it here
    }

}
