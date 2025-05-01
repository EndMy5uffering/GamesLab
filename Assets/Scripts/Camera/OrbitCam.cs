using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCam : MonoBehaviour
{

    public InputActionReference zoomInOutControlls;
    public InputActionReference camPanControlls;
    public InputActionReference pointerDirections;

    public Transform target; // The object to orbit around
    public float distance = 10.0f; // Distance from the target
    public float rotationSpeed = 5.0f; // Speed of rotation
    private Vector2 angle = new Vector2(0.0f, 0.0f); // Horizontal rotation angle
    public float minDistance = 2.0f; // Minimum distance from the target
    public float maxDistance = 20.0f; // Maximum distance from the target
    private float currentDistance = 2.0f; // Current distance from the target
    private Vector2 pointer;
    private bool panning = false;

    void Awake()
    {
        zoomInOutControlls.asset.Enable();
        zoomInOutControlls.action.performed += action => Zoom(action.ReadValue<float>());
        camPanControlls.action.started += action => panning = true;
        camPanControlls.action.canceled += action => panning = false;
        pointerDirections.action.performed += action => pointer = action.ReadValue<Vector2>();
    }

    void Zoom(float dir) 
    {
        Debug.Log("Zoom");
        currentDistance -= dir * 2; 
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
    }

    void PanCamera()
    {
        angle += pointer * rotationSpeed;
        angle.y = Mathf.Clamp(angle.y, -89f, 89f);
        pointer = new Vector2();
    }

    void Update()
    {
        if(panning) PanCamera();
        Vector3 direction = new Vector3(0, 0, -currentDistance);
        Quaternion rotation = Quaternion.Euler(angle.y, angle.x, 0);
        transform.position = target.position + rotation * direction;
        transform.LookAt(target.position);
    }
}