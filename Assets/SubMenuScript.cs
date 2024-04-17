using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using TMPro;

public class SubMenuScript : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign in the inspector
    public Transform contentPanel; // Assign in the inspector
    public TMP_Text menuTitle; // Assign in the inspector
    public GameObject backToMainMenuButton; // Assign in the inspector
    public float buttonSpacing = 5f; // Adjust spacing between buttons as needed

    private string directoryPath;

    private void Start()
    {
        
    }

    public void Initialize(string path)
    {
        directoryPath = path;
        PopulateSubMenu();
    }

    private void PopulateSubMenu()
    {
        // Update the menu title with the directory name
        string menuTitleText = Path.GetFileName(directoryPath);
        menuTitle.text = menuTitleText;

        // Get all subdirectories in the directory
        string[] subDirectories = Directory.GetDirectories(directoryPath);

        foreach (string subDir in subDirectories)
        {
            // Instantiate button prefab and set it up
            GameObject button = Instantiate(buttonPrefab, contentPanel);
            button.transform.localScale = Vector3.one;
            button.name = "Button - " + Path.GetFileName(subDir);

            // Set the button text to the subdirectory name
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = Path.GetFileName(subDir);
            }

            // Add an on-click event to the button
            button.GetComponent<Button>().onClick.AddListener(() => OpenSubDirectory(subDir));

            // Set the spacing if using LayoutElement
            LayoutElement layoutElement = button.GetComponent<LayoutElement>();
            if (layoutElement != null)
            {
                layoutElement.minHeight = buttonSpacing;
            }
        }

        // Optionally add a button to go back to the main menu
        backToMainMenuButton.GetComponent<Button>().onClick.AddListener(ReturnToMainMenu);
    }

    private void OpenSubDirectory(string subDir)
    {
        // Logic to open the subdirectory, which could be spawning another submenu
        Debug.Log("Opening Subdirectory: " + subDir);
        // If you're using separate scenes, you could load a new scene here
        // Otherwise, you could instantiate a new submenu prefab and initialize it with the subdirectory
    }

    private void ReturnToMainMenu()
    {
        // Logic to return to the main menu
        // This could be as simple as deactivating this submenu and reactivating the main menu
        Debug.Log("Returning to Main Menu");
    }
}
