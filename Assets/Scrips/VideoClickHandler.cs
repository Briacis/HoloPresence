using UnityEngine;
using UnityEngine.Video;

public class VideoClickHandler : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject VideoCanvas;

    private void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer not assigned in VideoClickHandler.");
        }
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            Debug.Log("Clicked at position: " + worldPos);

            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.gameObject.name);

                if (hit.collider.gameObject == gameObject)
                {
                    // Toggle play/pause
                    if (videoPlayer.isPlaying)
                    {
                        videoPlayer.Pause();
                        VideoCanvas.gameObject.SetActive(true);
                    }
/*                    else
                    {
                        videoPlayer.Play();
                        VideoCanvas.gameObject.SetActive(false);
                    }*/
                }
            }
        }
    }
}
