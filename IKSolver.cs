using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Text;
using System.Threading.Tasks;
using SerialComm;

public class IKSolver : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;

    public GameObject Parent;
    public GameObject Target;

    List<Vector3> effectorPositions = new List<Vector3>();

    SerialPort port = new SerialPort("COM5", 9600);
    Sender sender = new Sender();

    //Coeficienti pentru calibrare la brate:
    int bodyCalibration = 0;
    int shoulderCalibration = 311;
    int elbowCalibration = 330;
    int wristCalibration = 0;
    int palmCalibration = 0;

    private long lastUpdateTime;

    void Start()
    {
        Transform[] objectChildren = GetComponentsInChildren<Transform>();

        port.Open();
        System.Threading.Thread.Sleep(1000); //Delay of 1 second
        lastUpdateTime = GetCurrentTimeMillis();
        foreach (Transform child in objectChildren)
        {
            child.eulerAngles = new Vector3(0f, 0f, 0f);
        }



    }

    void Update()
    {



        //Camera.main.transform.LookAt(Parent.transform);

        Transform[] objectChildren = GetComponentsInChildren<Transform>();
        Transform body = objectChildren[1];
        Transform shoulder = objectChildren[2];
        Transform elbow = objectChildren[3];
        Transform wrist = objectChildren[4];
        Transform palm = objectChildren[5];
        Transform gripperRight = objectChildren[6];
        Transform gripperLeft = objectChildren[8];


        //gripperRight.transform.Rotate(0f, 0f, 90f);


        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            shoulder.transform.Rotate(3f, 0f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            shoulder.transform.Rotate(-3f, 0f, 0f);
        }


        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            wrist.transform.Rotate(0f, 3f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            wrist.transform.Rotate(0f, -3f, 0f);
        }



        if (Input.GetKeyDown(KeyCode.A))
        {
            body.transform.Rotate(0f, 3f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            body.transform.Rotate(0f, -3f, 0f);
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
            elbow.transform.Rotate(3f, 0f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            elbow.transform.Rotate(-3f, 0f, 0f);
        }



        if (Input.GetKeyDown(KeyCode.UpArrow))
        { 
            palm.transform.Rotate(3f, 0f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            palm.transform.Rotate(-3f, 0f, 0f);
        }


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gripperRight.transform.Rotate(0f, 0f, -3f);
            gripperLeft.transform.Rotate(0f, 0f, 3f);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gripperRight.transform.Rotate(0f, 0f, 3f);
            gripperLeft.transform.Rotate(0f, 0f, -3f);
        }

        long currentTime = GetCurrentTimeMillis();
        long elapsedTime = currentTime - lastUpdateTime;
       

        Debug.Log(shoulder.name);
        if (elapsedTime >= 200) // 1 second delay
        {
            //sender.SendGCode(port, $"{shoulder.name}{Math.Floor(shoulder.rotation.x*shoulderCalibration)}");
            //sender.SendGCode(port, $"{elbow.name}{Math.Floor(elbow.rotation.x*elbowCalibration)}");

            //sender.SendGCode(port, $"M280 P0 S0");
            //sender.SendGCode(port, $"G0 X0");
            //sender.SendGCode(port, $"{wrist.name}+{wrist.rotation.y}");
            //sender.SendGCode(port, $"{palm.name}+{palm.rotation.x / calibrationIndex}");
            
            lastUpdateTime = currentTime;
        }
    }

    private long GetCurrentTimeMillis()
    {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}