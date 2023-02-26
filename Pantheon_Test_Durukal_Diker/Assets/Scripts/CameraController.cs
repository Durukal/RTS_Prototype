using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private Camera Camera;

    [SerializeField]
    private float panSpeed;

    [SerializeField]
    private float panBorderThickness;

    private Vector3 cameraPosition;

    [SerializeField]
    private Vector2 panLimit;

    [SerializeField]
    private float scrollSpeed;

    [SerializeField]
    private float minScale;

    [SerializeField]
    private float maxScale;

    void Start() {
        Camera = Camera.main;
    }

    void Update() {
        cameraPosition = Camera.transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness) {
            cameraPosition.y += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness) {
            cameraPosition.y -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness) {
            cameraPosition.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness) {
            cameraPosition.x -= panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.orthographicSize -= scroll * scrollSpeed * 100f * Time.deltaTime;
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, -panLimit.x, panLimit.x);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, -panLimit.y, panLimit.y);
        Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize, minScale, maxScale);

        Camera.transform.position = cameraPosition;
    }
}