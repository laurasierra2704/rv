
using UnityEngine;

public class CogerObjeto : MonoBehaviour
{
   
    public GameObject handPoint;

    private GameObject pickedObject = null;






    void Update()
    {
        if(pickedObject != null)
        {
            if (Input.GetKey("r"))
            {
                pickedObject.GetComponent<Rigidbody>().useGravity = true;
                pickedObject.GetComponent<Rigidbody>().isKinematic = false;

                pickedObject.gameObject.transform.SetParent(null);
                pickedObject = null;
            }
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Objeto"))
        {
            if (Input.GetKey("e") && pickedObject == null)
            {
                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = handPoint.transform.position;
                other.gameObject.transform.SetParent(handPoint.gameObject.transform);
                pickedObject = other.gameObject;
            }
        }
    }


}

/*
using UnityEngine;
using System.IO.Ports;

public class CogerObjeto : MonoBehaviour
{
    public GameObject handPoint;

    private GameObject pickedObject = null;

    SerialPort serial = new SerialPort("COM3", 9600); // Cambia COM

    void Start()
    {
        if (!serial.IsOpen)
        {
            serial.Open();
        }
    }

    void Update()
    {
        if(pickedObject != null)
        {
            if (Input.GetKey("r"))
            {
                pickedObject.GetComponent<Rigidbody>().useGravity = true;
                pickedObject.GetComponent<Rigidbody>().isKinematic = false;

                pickedObject.transform.SetParent(null);
                pickedObject = null;

                // 🔴 Enviar señal de apagar motor
                serial.WriteLine("OFF");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Objeto"))
        {
            if (Input.GetKey("e") && pickedObject == null)
            {
                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = handPoint.transform.position;
                other.transform.SetParent(handPoint.transform);

                pickedObject = other.gameObject;

                // 🟢 Enviar señal de encender motor
                serial.WriteLine("ON");
            }
        }
    }
}
*/