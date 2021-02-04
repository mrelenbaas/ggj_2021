using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : MonoBehaviour
{
    private const float SPEED = 20f;
    private const float SIZE = 19.5f;
    private float floor = 1.5f;
    private const float OFFSET = 2f;

    public bool isLocal = true;
    public Transform top;
    public Transform bottom;
    public Transform left;
    public Transform right;

    private MeshRenderer topMR;
    private MeshRenderer bottomMR;
    private MeshRenderer leftMR;
    private MeshRenderer rightMR;

    private float pythonX = 0.0f;
    private float pythonY = 0.0f;

    private float x;
    private float width;
    private float xPercent;
    private float xPosition;
    private float y;
    private float height;
    private float yPercent;
    private float yPosition;

    public float GetWidthPercentage()
    {
        return transform.position.x / SIZE;
    }

    public float GetHeightPercentage()
    {
        return transform.position.z / SIZE;
    }

    public void setPosition(float widthPercentage, float heightPercentage)
    {
        pythonX = -SIZE + ((SIZE * 2) * widthPercentage);
        pythonY = SIZE - ((SIZE * 2) * heightPercentage);
        print("python " + pythonX + ", " + pythonY);
    }

    void Start()
    {
        topMR = top.GetComponent<MeshRenderer>();
        bottomMR = bottom.GetComponent<MeshRenderer>();
        leftMR = left.GetComponent<MeshRenderer>();
        rightMR = right.GetComponent<MeshRenderer>();
        if (!isLocal)
        {
            floor += 1.5f;
        }
    }

    void Update()
    {
        Vector3 moveDir = Vector3.zero;
        if (isLocal)
        {
            moveDir.x = Input.GetAxis("Horizontal");
            moveDir.z = Input.GetAxis("Vertical");
            transform.position += moveDir * SPEED * Time.deltaTime;
        }
        else
        {
            //moveDir.x = pythonX;
            //moveDir.z = pythonY;
            transform.position = new Vector3(pythonX, transform.position.y, pythonY);
        }
        //transform.position += moveDir * SPEED * Time.deltaTime;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -SIZE, SIZE),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -SIZE, SIZE)
        );

        x = Input.mousePosition.x;
        width = Screen.width;
        xPercent = Mathf.Clamp(x / width, 0.0f, 1.0f);
        xPosition = -SIZE + (SIZE * 2f * xPercent);
        y = Input.mousePosition.y;
        height = Screen.height;
        yPercent = Mathf.Clamp(y / height, 0.0f, 1.0f);
        yPosition = -SIZE + (SIZE * 2f * yPercent);
        if (isLocal)
        {
            transform.position = new Vector3(
                Mathf.Clamp(xPosition, -SIZE, SIZE),
                transform.position.y,
                Mathf.Clamp(yPosition, -SIZE, SIZE)
            );
        }
        else
        {
            // Android.
        }

        top.localScale = new Vector3(
            40f,
            bottom.localScale.y,
            Mathf.Clamp(SIZE - transform.position.z - (OFFSET * 2f), 0, (SIZE * 2f))
        );
        top.position = new Vector3(
            top.position.x,
            floor,
            SIZE - ((SIZE - transform.position.z) / 2f) + 0.5f + OFFSET
        );
        if (top.localPosition.z >= 20f)
        {
            top.position = new Vector3(
                top.position.x,
                floor,
                SIZE + 0.5f
            );
            topMR.enabled = false;
        }
        else
        {
            topMR.enabled = true;
        }
        
        bottom.localScale = new Vector3(
            40f,
            bottom.localScale.y,
            Mathf.Clamp(SIZE + transform.position.z - (OFFSET * 2f), 0, (SIZE * 2f))
        );
        bottom.position = new Vector3(
            bottom.position.x,
            floor,
            transform.position.z - ((SIZE + transform.position.z) / 2f) - 0.5f - OFFSET
        );
        if (bottom.localPosition.z < -20f)
        {
            bottom.position = new Vector3(
                bottom.position.x,
                floor,
                -SIZE - 0.5f
            );
            bottomMR.enabled = false;
        }
        else
        {
            bottomMR.enabled = true;
        }

        left.localScale = new Vector3(
            Mathf.Clamp(SIZE + transform.position.x - (OFFSET * 2f), 0, (SIZE * 2f)),
            left.localScale.y,
            40f
        );
        left.position = new Vector3(
            transform.position.x - ((SIZE + transform.position.x) / 2f) - 0.5f - OFFSET,
            floor,
            left.position.z
        );
        if (left.localPosition.x < -20f)
        {
            left.position = new Vector3(
                -SIZE - 0.5f,
                floor,
                left.position.z
            );
            leftMR.enabled = false;
        }
        else
        {
            leftMR.enabled = true;
        }

        right.localScale = new Vector3(
            Mathf.Clamp(SIZE - transform.position.x - (OFFSET * 2f), 0, (SIZE * 2f)),
            right.localScale.y,
            40f
        );
        right.position = new Vector3(
            SIZE - ((SIZE - transform.position.x) / 2f) + 0.5f + OFFSET,
            floor,
            right.position.z
        );
        if (right.localPosition.x > 20f)
        {
            right.position = new Vector3(
                SIZE + 0.5f,
                floor,
                right.position.z
            );
            rightMR.enabled = false;
        }
        else
        {
            rightMR.enabled = true;
        }
    }

    private void OnGUI()
    {
        string label = "" + x + " / " + width + " = " + xPercent;
        GUI.Label(new Rect(300, 300, 400, 20), label);
    }
}
