using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubMenu : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign in the inspector
    public Transform content;
    public int contentLength = 650;
    public Transform contentPanel; // Assign in the inspector
    public GameObject ScrollView;
    public GameObject Panel;
    public float spacing = 5f;
    public string fileDirectoryPath;
    public VideoGallery videoGalleryScript;
    //public GameObject subMenu;
    public GameObject videoGallery;
    public GameObject Headder;
    public GameObject menuTitle;

    void Start()
    {
        //string fileDirectoryPath = @"D:\Unity\TestMenu"; // Update this path
        //string fileDirectoryPath = @"E:\Traditional Crafts"; // Update this path
        //string fileDirectoryPath = @"F:\Final Mayo Clips\Oral History\Betty"; // Update this path

        string[] fileEntries = Directory.GetDirectories(fileDirectoryPath).Reverse().ToArray(); // This will get both files and directories

        // Update the menu title with the directory name
        string menuTitleText = Path.GetFileName(fileDirectoryPath);
        TMP_Text menuTitleTMP = menuTitle.GetComponent<TMP_Text>();
        if (menuTitleTMP != null)
        {
            menuTitleTMP.text = menuTitleText;
        }
        else
        {
            Debug.LogError("TextMeshPro component not found on menuTitle GameObject.");
        }

        // Adjust the content panel's height (only necessary if using a dynamic layout that doesn't auto-size)
        // int totalHeight = fileEntries.Length * contentLength;
        // RectTransform contentRect = content.GetComponent<RectTransform>();
        // contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalHeight);

        // Create a button for each file or directory
        foreach (string entryPath in fileEntries)
        {
            string thumbnailPath = Path.ChangeExtension(entryPath, ".png");
            string entryName = Path.GetFileName(entryPath);

            GameObject button = Instantiate(buttonPrefab) as GameObject;
            button.transform.SetParent(contentPanel, false);
            button.transform.localScale = Vector3.one;

            button.name = "Button - " + entryName;

            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = entryName;
            }
            else
            {
                Debug.LogWarning("No TMP_Text component found on the button prefab's children.");
            }

            // Use a generic icon for directories, and thumbnail for files if exists
            Texture2D texture = LoadTexture(thumbnailPath);
            if (texture != null)
            {
                button.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                // Optionally handle the case where the texture couldn't be loaded
            }

            if (texture != null)
            {
                button.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                // If a thumbnail doesn't exist, consider setting a default image or leaving it out
            }

            Button btnComponent = button.GetComponent<Button>();
            if (btnComponent != null)
            {
                // Add your click listener here
                // For directories, you may want to navigate into the subfolder
                // For files, you may want to perform the default action or open the file
                btnComponent.onClick.AddListener(() => HandleFileClick(entryPath));
            }

            // Adjust layout element if needed
            LayoutElement layoutElement = button.GetComponent<LayoutElement>();
            if (layoutElement == null)
            {
                layoutElement = button.AddComponent<LayoutElement>();
            }
            layoutElement.minHeight = spacing;
        }
    }

    // Call this method to initialize the submenu with contents from a specific directory
    public void InitializeWithDirectory(string directoryPath)
    {
        // Assuming this is the path where the videos are located
        fileDirectoryPath = directoryPath;

        // Clear existing buttons (if reusing the menu)
/*        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }*/

        // Rest of the initialization code that uses 'filefileDirectoryPath' to populate the menu...
        
    }

    // Implement your file click handler
    void HandleFileClick(string path)
    {

        // Determine if the path is a file or directory and handle accordingly
        if (Directory.Exists(path))
        {
            videoGalleryScript.UpdateDirectoryAndPopulate(path);

            this.gameObject.SetActive(false);
            videoGallery.SetActive(true);

            // For directories, you might navigate into the directory
            Debug.Log("Directory clicked: " + path);
            // You can call another method here to handle the navigation
        }
        else if (File.Exists(path))
        {
            // For files, you can do something like play the video or open the file
            Debug.Log("File clicked: " + path);
            // For example, if it's a video, call your PlayVideo method
        }
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
        genericTexture.SetPixel(0, 0, Color.blue); // You can change Color.gray to any color you prefer
        genericTexture.Apply();
        return genericTexture;
    }

}

