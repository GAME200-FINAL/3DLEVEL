using UnityEditor;
using UnityEngine;

public class CamController2 : MonoBehaviour
{
    [SerializeField] private float rotationSpeedX = 2f; // 40f, how fast the camera offset will rotate left and right
    [SerializeField] private float rotationSpeedY = 1f; // 40f, how fast the camera offset will rotate up and down

    [Header("Objects needed to position the camera")]
    [SerializeField] private Transform lookAtObject; // The object the camera will always rotate to look at
    [SerializeField] private Transform offset; // The empty object the camera will always lerp toward
    [SerializeField] private Transform cameraLookUpHeight; // The empty object representing the highest the camera will look up at low angles

    [SerializeField] private LayerMask ignoreLayers;
    private const float ROTATION_MAX = 55f; // 45f
    private const float ROTATION_MIN = 8; // 5f
    private Quaternion rotation;
    private RaycastHit hit = new RaycastHit();

    private static readonly int[] defaultBlockageAngles = new int[4] {2, -4, 6, -10};
    private static readonly int[] turningRightAngles = new int[3] {-1, -2, -3};
    private static readonly int[] turningLeftAngles = new int[3] {1, 2, 3};

    [Range(6, 15)]
    [SerializeField]
    private float maximumDistance = 15f;

    private void Awake()
    {
       // rotation = transform.rotation;
    }

    private void LateUpdate()
    {
        //transform.rotation = rotation;

        //if (SceneController.instance.isPaused) return;

        // Horizontal movement!
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X")  * rotationSpeedX, Space.World);

        // Vertical Movement!
        var angle = Input.GetAxis("Mouse Y") * rotationSpeedY;
        transform.Rotate(Vector3.right, angle);

        //lookAtObject.position = Vector3.Lerp(cameraLookUpHeight.position, transform.position, 5*Time.deltaTime);
        //AdjustCameraDistance(maximumDistance);
        //AvoidBlockages();
        Camera.main.transform.position = offset.position;
        Camera.main.transform.LookAt(lookAtObject);
       // rotation = transform.rotation;
    }





    private void AdjustLookAtPosition(float lerpPosition)
    {
        
    }

}
