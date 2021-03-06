using UnityEngine;
using System.Collections;
public class PlayerCamera : MonoBehaviour
{
    public Transform targetTransform;
    private Camera _Cam;
    public Camera Cam
    {
        get
        {
            if (_Cam == null)
            {
                _Cam = GetComponent<Camera>();
            }
            return _Cam;
        }
    }

    public bool isMoving;
    public Vector3 CamOffset = Vector3.zero;
    public float senstivityX = 5;
    public float senstivityY = 1;
    public float minY = 30;
    public float maxY = 50;

    private bool isFading = false;
    private float alpha;
    private float currentX = 0;
    private float currentY = 1;
    private float time;
    private Texture2D texture;
    private Color fadeColor;
    public AnimationCurve FadeCurve;

    void Update()
    {
        currentX += Input.GetAxis("Mouse X");
        currentY -= Input.GetAxis("Mouse Y");
        currentX = Mathf.Repeat(currentX, 360);
        currentY = Mathf.Clamp(currentY, minY, maxY);
        isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) ? true : false;
        if (isMoving)
        {
            UpdatePlayerRotation();
        }
    }
    void UpdatePlayerRotation()
    {
        targetTransform.rotation = Quaternion.Euler(0, currentX, 0);
    }

    void LateUpdate()
    {
        Vector3 dist = CamOffset;
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = targetTransform.position + rotation * dist;
        transform.LookAt(targetTransform.position);
        CheckWall();
    }

    public LayerMask wallLayer;
    void CheckWall()
    {
        Vector3 start = targetTransform.position;
        Vector3 dir = transform.position - targetTransform.position;
        float dist = CamOffset.z * -1;

        if (Physics.Raycast(start, dir, out RaycastHit hit, dist, wallLayer))
        {
            float hitDist = hit.distance;
            Vector3 sphereCastCenter = targetTransform.position + (dir.normalized * hitDist);
            transform.position = sphereCastCenter;
        }
    }

    public void FadeTo(int color)
    {
        texture = new Texture2D(1, 1);
        fadeColor = (color == 0 ? Color.black : Color.white);
        isFading = true;
    }

    public void OnGUI()
    {
        if (!isFading) return;

        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();

        time += Time.deltaTime;
        alpha = FadeCurve.Evaluate(time);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);

        if (alpha >= 1)
        {
            isFading = false;
            time = 0;
        } 
    }
}