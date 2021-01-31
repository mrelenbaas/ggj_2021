using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class Serial : MonoBehaviour
{
    public Finger finger;
    public Finger pythonFinger;

    SerialPort sp;
    float next_time; int ii = 0;
    string label;
    // Use this for initialization
    void Start()
    {
        string the_com = "";
        next_time = Time.time;

        foreach (string mysps in SerialPort.GetPortNames())
        {
            //print(mysps);
            if (mysps != "COM1") { the_com = mysps; break; }
        }
        the_com = "COM5";
        sp = new SerialPort("\\\\.\\" + the_com, 9600);
        if (!sp.IsOpen)
        {
            //print("Opening " + the_com + ", baud 9600");
            sp.Open();
            sp.ReadTimeout = 50;
            sp.Handshake = Handshake.None;
            if (sp.IsOpen)
            { 
                //print("Open");
            }
        }

        StartCoroutine
        (
            AsynchronousReadFromArduino
            ((string s) => MyCallback(s),     // Callback
                () => Debug.LogError("Error!"), // Error callback
                1000f                          // Timeout (milliseconds)
            )
        );
    }

    private void MyCallback(string s)
    {
        //print("<<< " + s);
        string[] splitArray = s.Split(char.Parse(","));
        //print(splitArray[0]);
        //print(splitArray[1]);
        pythonFinger.setPosition(float.Parse(splitArray[0]), float.Parse(splitArray[1]));
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > next_time)
        {
            if (!sp.IsOpen)
            {
                sp.Open();
                print("opened sp");
            }
            if (sp.IsOpen)
            {
                //print("Writing " + ii);
                //sp.Write((ii.ToString()));
                string message = "" + finger.GetWidthPercentage() + "," + finger.GetHeightPercentage();
                sp.Write(message);
                //print(">>> " + message);
            }
            next_time = Time.time + 1;
            if (++ii > 9) ii = 0;
        }

        //label = ReadFromArduino();
        //print(label);

    }

    public string ReadFromArduino(int timeout = 0)
    {
        sp.ReadTimeout = timeout;
        try
        {
            return sp.ReadLine();
        }
        catch (System.TimeoutException e)
        {
            return null;
        }
    }

    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do
        {
            try
            {
                dataString = sp.ReadLine();
                //print("<<< " + dataString);
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);

                StartCoroutine
                (
                    AsynchronousReadFromArduino
                    ((string s) => MyCallback(s),     // Callback
                        () => Debug.LogError("Error!"), // Error callback
                        1000f                          // Timeout (milliseconds)
                    )
                );
                yield break; // Terminates the Coroutine
            }
            else
                yield return null; // Wait for next frame

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);

        if (fail != null)
            fail();

        yield return null;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 140, 20), "something");
        /*
        if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button."))
        {
            print("You clicked a button.");
        }
        */
    }
}
