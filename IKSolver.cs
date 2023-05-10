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

    bool isT0Presed = true;

    //Coeficienti pentru calibrare la brate:
    int bodyCalibration = 0;
    int shoulderCalibration = 311;
    int elbowCalibration = 330;
    int wristCalibration = 0;
    int palmCalibration = 0;


    double varbody = 0;
    double varshould = 0;
    double varpalm = 0;
    double varelbow = 0;
    double varwrist = 0;
    double vargripp = 120;
    double speed = 100;


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
        Transform[] objectChildren = GetComponentsInChildren<Transform>();
        Transform body = objectChildren[1];
        Transform shoulder = objectChildren[2];
        Transform elbow = objectChildren[3];
        Transform wrist = objectChildren[4];
        Transform palm = objectChildren[5];
        Transform gripperRight = objectChildren[6];
        Transform gripperLeft = objectChildren[8];
        long currentTime = GetCurrentTimeMillis();
        long elapsedTime = currentTime - lastUpdateTime;

        if (elapsedTime >= 20) // 1 second delay
        {

        if (Input.GetKey(KeyCode.Keypad8))
        {
            shoulder.transform.Rotate(0.175f , 0f, 0f);
            varshould+= 0.35;
            sender.SendGCode(port, $"G0 Y" + varshould);
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            shoulder.transform.Rotate(-0.175f , 0f, 0f);
            varshould-=0.35;
            sender.SendGCode(port, $"G0 Y" + varshould);
        }



        if (Input.GetKey(KeyCode.A))
        {
            if (!isT0Presed)
            {
                sender.SendGCode(port, $"T0");
                isT0Presed = true;
            }
                wrist.transform.Rotate(0f, 0.3f, 0f);
                varwrist+= 0.3;
                sender.SendGCode(port, $"G0 E" + varwrist);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (!isT0Presed)
            {
                sender.SendGCode(port, $"T0");
                isT0Presed = true;
            }
            wrist.transform.Rotate(0f, -0.3f, 0f);
            varwrist-= 0.3;
            sender.SendGCode(port, $"G0 E" + varwrist);
        }




        if (Input.GetKey(KeyCode.Keypad4))
        {
            body.transform.Rotate(0f, 0.35f, 0f);
            varbody-=0.5;
            sender.SendGCode(port, $"G0 X" + varbody);
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            body.transform.Rotate(0f, -0.35f, 0f);
            varbody+= 0.5;
            sender.SendGCode(port, $"G0 X"+ varbody);
        }




        if (Input.GetKey(KeyCode.W))
        {
            elbow.transform.Rotate(0.2f, 0f, 0f);
            varelbow+=0.5;
            sender.SendGCode(port, $"G0 Z" + varelbow);
        }
        if (Input.GetKey(KeyCode.S))
        {
            elbow.transform.Rotate(-0.2f, 0f, 0f);
            varelbow-=0.5;
            sender.SendGCode(port, $"G0 Z" + varelbow);
        }

        
 

            if (Input.GetKey(KeyCode.UpArrow))
          {
                if (isT0Presed)
                {
                    sender.SendGCode(port, $"T1");
                    isT0Presed = false;
                }
                palm.transform.Rotate(1.5f, 0f, 0f);
            varpalm+= 0.65;
            sender.SendGCode(port, $"G0 E" + varpalm);
          }
          if (Input.GetKey(KeyCode.DownArrow))
          {
                if (isT0Presed)
                {
                    sender.SendGCode(port, $"T1");
                    isT0Presed = false;
                }
                palm.transform.Rotate(-1.5f, 0f, 0f);
            varpalm-= 0.65;
            sender.SendGCode(port, $"G0 E" + varpalm);
          }
        



        if (Input.GetKey(KeyCode.RightArrow))
        {
            vargripp -= 3;
                if (vargripp < 0)
                {
                    vargripp = 0;
                }
                else
                {
                    gripperRight.transform.Rotate(0f, 0f, -0.6f);
                    gripperLeft.transform.Rotate(0f, 0f, 0.6f);
                    sender.SendGCode(port, $"M280 P0 S" + vargripp);
                }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            vargripp += 3;
                if (vargripp > 120)
                {
                    vargripp = 120;
                }
                else
                {
                    gripperRight.transform.Rotate(0f, 0f, 0.6f);
                    gripperLeft.transform.Rotate(0f, 0f, -0.6f);
                    sender.SendGCode(port, $"M280 P0 S" + vargripp);
                }

        }



        //Speed control

        if (Input.GetKey(KeyCode.Q))
        {
            speed -= 10;
            sender.SendGCode(port, $"M220 S" + speed);
        }

        if (Input.GetKey(KeyCode.E))
        {
            speed += 10;
            sender.SendGCode(port, $"M220 S" + speed);

        }

            //sender.SendGCode(port, $"{shoulder.name}{Math.Floor(shoulder.rotation.x*shoulderCalibration)}");
            //sender.SendGCode(port, $"{elbow.name}{Math.Floor(elbow.rotation.x*elbowCalibration)}");

            //sender.SendGCode(port, $"M280 P0 S0");
            //sender.SendGCode(port, $"M220 S10");
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