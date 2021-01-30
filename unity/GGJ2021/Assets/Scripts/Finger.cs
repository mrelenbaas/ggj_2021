using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : MonoBehaviour
{
    private const float SPEED = 20f;
    private const float SIZE = 19.5f;
    private const float FLOOR = 1.5f;
    private const float OFFSET = 2f;

    public Transform top;
    public Transform bottom;
    public Transform left;
    public Transform right;

    private MeshRenderer topMR;
    private MeshRenderer bottomMR;
    private MeshRenderer leftMR;
    private MeshRenderer rightMR;

    public float GetWidthPercentage()
    {
        return transform.position.x / SIZE;
    }

    public float GetHeightPercentage()
    {
        return transform.position.z / SIZE;
    }

    void Start()
    {
        topMR = top.GetComponent<MeshRenderer>();
        bottomMR = bottom.GetComponent<MeshRenderer>();
        leftMR = left.GetComponent<MeshRenderer>();
        rightMR = right.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        Vector3 moveDir = Vector3.zero;
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.z = Input.GetAxis("Vertical");
        transform.position += moveDir * SPEED * Time.deltaTime;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -SIZE, SIZE),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -SIZE, SIZE)
        );

        top.localScale = new Vector3(
            40f,
            bottom.localScale.y,
            Mathf.Clamp(SIZE - transform.position.z - (OFFSET * 2f), 0, (SIZE * 2f))
        );
        top.position = new Vector3(
            top.position.x,
            FLOOR,
            SIZE - ((SIZE - transform.position.z) / 2f) + 0.5f + OFFSET
        );
        if (top.localPosition.z >= 20f)
        {
            top.position = new Vector3(
                top.position.x,
                FLOOR,
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
            FLOOR,
            transform.position.z - ((SIZE + transform.position.z) / 2f) - 0.5f - OFFSET
        );
        if (bottom.localPosition.z < -20f)
        {
            bottom.position = new Vector3(
                bottom.position.x,
                FLOOR,
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
            FLOOR,
            left.position.z
        );
        if (left.localPosition.x < -20f)
        {
            left.position = new Vector3(
                -SIZE - 0.5f,
                FLOOR,
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
            FLOOR,
            right.position.z
        );
        if (right.localPosition.x > 20f)
        {
            right.position = new Vector3(
                SIZE + 0.5f,
                FLOOR,
                right.position.z
            );
            rightMR.enabled = false;
        }
        else
        {
            rightMR.enabled = true;
        }
    }

    //private void OnGUI()
    //{
        //string label = "" + transform.position;
        //GUI.Label(new Rect(10, 10, 140, 20), label);
        /*
        if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button."))
        {
            print("You clicked a button.");
        }
        */
    //}
}
