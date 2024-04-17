using UnityEngine;

public class ExitButtonHandler : MonoBehaviour
{
    // Method to be called when the button is clicked
    public void ExitGame()
    {
        // Print a message to the console (useful for debugging, this line can be removed in the final build)
        Debug.Log("Exit button clicked. Exiting the game.");

        // Close the application
        Application.Quit();

        // If running in the Unity editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
