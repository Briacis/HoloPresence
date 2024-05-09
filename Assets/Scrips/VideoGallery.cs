using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Video;
using System.Linq;
using TMPro;
using System;  // Include System namespace for DateTime

public class VideoGallery : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign in the inspector
    public Transform content;
    public int contentLength = 650;
    public Transform contentPanel; // Assign in the inspector
    public VideoPlayer videoPlayer; // Assign in the inspector
    public AudioSource audioSource;
    public GameObject ScrollView;
    public GameObject Panel;
    public float spacing = 5f;
    public Button playPauseButton;
    public Button stopButton;
    public Slider seekSlider;
    public GameObject Headder;
    public GameObject videoDisplayObject;
    public GameObject menuTitle;
    public string videoDirectoryPath;
    private string csvFilePath = "videoLog.csv";  // Path to the CSV file

    private bool isSeeking = false;
    private bool isPlaying = false;
    /*    public void TriggerPlay()
        {
            isPlaying = true;
            Debug.Log("isPlaying = true");
        }

        public void TriggerStop()
        {
            isPlaying = false;
            Debug.Log("isPlaying = false");
        }*/

    void Start()
    {
        InitializeCSV();  // Ensure CSV file is ready
        //string videoDirectoryPath = @"D:\Unity\TestMenu\TestFolder\TestSubfolder"; // Update this path
        //string videoDirectoryPath = @"E:\Traditional Crafts"; // Update this path
        //string videoDirectoryPath = @"F:\Final Mayo Clips\Oral History\Betty"; // Update this path
        string[] videoFiles = Directory.GetFiles(videoDirectoryPath, "*.mp4").Reverse().ToArray();

        string menuTitleText = Path.GetFileName(videoDirectoryPath); // This gets the last part of the path, which is typically the folder name

        // Find the TextMeshPro component on the menuTitle GameObject and set its text
        TMP_Text menuTitleTMP = menuTitle.GetComponent<TMP_Text>();
        if (menuTitleTMP != null)
        {
            menuTitleTMP.text = menuTitleText; // Set the folder name as the menu title
        }
        else
        {
            Debug.LogError("TextMeshPro component not found on menuTitle GameObject.");
        }


        // Calculate the total height required for all buttons
        int totalHeight = videoFiles.Length * contentLength;

        // Adjust the content panel's height
        RectTransform contentRect = content.GetComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalHeight);

        foreach (string videoPath in videoFiles)
        {
            string thumbnailPath = Path.ChangeExtension(videoPath, ".png");
            string videoFileName = Path.GetFileNameWithoutExtension(videoPath);

            GameObject button = Instantiate(buttonPrefab) as GameObject;
            button.transform.SetParent(contentPanel, false);
            button.transform.localScale = Vector3.one; // Ensure the button's scale is set correctly after parenting

            // Set the name of the button GameObject for easier identification in the hierarchy (optional)
            button.name = "Button - " + videoFileName;

            // Find the TMP Text component in the button and set its text
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = videoFileName;
            }
            else
            {
                Debug.LogWarning("No TMP_Text component found on the button prefab's children.");
            }

            Texture2D texture = LoadTexture(thumbnailPath);
            if (texture != null)
            {
                button.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                // Optionally handle the case where the texture couldn't be loaded
            }

            button.GetComponent<Button>().onClick.AddListener(() => PlayVideo(videoPath));

            // Set the spacing
            LayoutElement layoutElement = button.GetComponent<LayoutElement>();
            if (layoutElement == null)
            {
                layoutElement = button.AddComponent<LayoutElement>();
            }
            layoutElement.minHeight = spacing;
        }



        //playPauseButton.onClick.AddListener(TogglePlayPause);
        //stopButton.onClick.AddListener(StopVideo);
        //seekSlider.onValueChanged.AddListener(SeekVideo);
        //videoPlayer.loopPointReached += OnVideoEnded;


    }

    public void UpdateDirectoryAndPopulate(string directoryPath)
    {
        videoDirectoryPath = directoryPath;
        PopulateGallery();
    }

    private void PopulateGallery()
    {
        // Clear existing buttons to repopulate the gallery
/*        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }*/

        // Assuming that the path contains video files
        string[] videoFiles = Directory.GetFiles(videoDirectoryPath, "*.mp4").Reverse().ToArray();
        // ... the rest of the code to populate the gallery with video files
    }



    public void InitializeWithDirectory(string directoryPath)
    {
        // Assuming this is the path where the videos are located
        videoDirectoryPath = directoryPath;

        // Clear existing buttons (if reusing the menu)
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Rest of the initialization code that uses 'filefileDirectoryPath' to populate the menu...
        Start();
    }

    void Update()
    {
        /*        if (videoPlayer.isPlaying)
                {
                    seekSlider.value = (float)(videoPlayer.time / videoPlayer.length);

                }*/

        Debug.Log("Seek Value: " + " " + seekSlider.value);
        Debug.Log("Seek Value: " + " " + videoPlayer.time);

        if (!isSeeking && videoPlayer.isPlaying)
        {
            seekSlider.value = (float)(videoPlayer.time / videoPlayer.length);

        }

        // Check if the user is interacting with the slider and update the video's time accordingly
        if (isSeeking)
        {
            float seekTime = seekSlider.value * (float)videoPlayer.length;
            videoPlayer.time = seekTime;
        }
    }

    public void StartSeeking()
    {
        isSeeking = true;
        Debug.Log("StartSeeking");
    }

    public void StopSeeking()
    {
        isSeeking = false;
        Debug.Log("StopSeeking");
    }

    public void TogglePlayPause()
    {
        //if (isPlaying)
        {
            videoPlayer.Pause();
            Debug.Log("Video is Paused");


        }
        /*        else
                {
                    videoPlayer.Play();
                    //VideoCanvas.gameObject.SetActive(false); // Disable the VideoCanvas when playing
                }
                isPlaying = !isPlaying;*/
    }

    public void ContinuePlay()
    {
        videoPlayer.Play();
        Debug.Log("Video is Played");


    }
    public void StopVideo()
    {
        videoPlayer.Stop();
        isPlaying = false;
        seekSlider.value = 0;
        Debug.Log("Video is Stopped");

        // Clear the video
        videoPlayer.clip = null;
        videoPlayer.url = string.Empty;

        // Optional: Hide or reset the RawImage component
        RawImage rawImage = videoDisplayObject.GetComponent<RawImage>();
        if (rawImage != null)
        {
            rawImage.texture = null; // Or assign some default texture
        }

        ScrollView.gameObject.SetActive(true);
        Panel.gameObject.SetActive(true);
        Headder.gameObject.SetActive(true);
        //VideoCanvas.gameObject.SetActive(false);


    }

    private void SeekVideo(float value)
    {
        if (videoPlayer.isPlaying || videoPlayer.isPaused)
        {
            videoPlayer.time = value * videoPlayer.length;
        }
    }

    private void OnVideoEnded(VideoPlayer source)
    {
        isPlaying = false;
        seekSlider.value = 0;
    }


    Texture2D LoadTexture(string filePath)
    {
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            return tex;
        }
        else
        {
            // If file doesn't exist, return a generic colored texture
            return CreateGenericTexture();
        }
    }

    Texture2D CreateGenericTexture()
    {
        Texture2D genericTexture = new Texture2D(1, 1);
        genericTexture.SetPixel(0, 0, Color.gray); // You can change Color.gray to any color you prefer
        genericTexture.Apply();
        return genericTexture;
    }


    // Method to initialize the CSV file
    private void InitializeCSV()
    {
        // Check if the CSV file exists; if not, create it
        if (!File.Exists(csvFilePath))
        {
            using (StreamWriter sw = File.CreateText(csvFilePath))
            {
                sw.WriteLine("Date,Time,Video Title");  // Write the header line
            }
        }
    }

    // Method to log video play events to the CSV
    private void LogVideoPlay(string videoTitle)
    {
        using (StreamWriter sw = File.AppendText(csvFilePath))
        {
            string logEntry = $"{DateTime.Now.ToShortDateString()},{DateTime.Now.ToShortTimeString()},{videoTitle}";
            sw.WriteLine(logEntry);
        }
    }

    public void PlayVideo(string path)
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = path;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.EnableAudioTrack(0, true);

        videoPlayer.Prepare();
        videoPlayer.isLooping = true;
        videoPlayer.Play();
        audioSource.Play();

        // Log the video play event
        string videoTitle = Path.GetFileNameWithoutExtension(path);
        LogVideoPlay(videoTitle);

        ScrollView.gameObject.SetActive(false);
        Panel.gameObject.SetActive(false);
        Headder.gameObject.SetActive(false);
        Debug.Log("Video is Played");
        //VideoCanvas.gameObject.SetActive(false); // Show the video canvas when playing a video


        // Assuming 'videoPlayer' and 'VideoDisplay' are correctly assigned
        RenderTexture renderTexture = videoPlayer.targetTexture;

        // Assign the renderTexture to the RawImage
        RawImage rawImage = GameObject.Find("VideoDisplay").GetComponent<RawImage>();
        if (rawImage != null)
        {
            if (renderTexture != null)
            {
                rawImage.texture = renderTexture;
                videoPlayer.Play();
            }
            else
            {
                Debug.LogError("RenderTexture on VideoPlayer is not set.");
            }
        }
        else
        {
            Debug.LogError("VideoDisplay RawImage component not found.");
        }
    }


}