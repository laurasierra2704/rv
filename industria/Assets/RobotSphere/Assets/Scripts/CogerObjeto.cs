using UnityEngine;

public class CogerObjeto : MonoBehaviour
{
    public GameObject handPoint;

    private GameObject pickedObject = null;
    private bool justPickedUp = false; // flag para evitar soltar en el mismo frame

    void Update()
    {
        if (justPickedUp)
        {
            justPickedUp = false; // resetear al siguiente frame
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && pickedObject != null)
        {
            Soltar();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Objeto"))
        {
            if (Input.GetKeyDown(KeyCode.E) && pickedObject == null)
            {
                Agarrar(other);
            }
        }
    }

    private void Agarrar(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        Collider col = other.GetComponent<Collider>();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;

        col.enabled = false;

        other.transform.SetParent(handPoint.transform);
        other.transform.localPosition = Vector3.zero;

        pickedObject = other.gameObject;
        justPickedUp = true; // marcar que se acaba de agarrar
    }

    private void Soltar()
    {
        Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
        Collider col = pickedObject.GetComponent<Collider>();

        pickedObject.transform.SetParent(null);

        rb.useGravity = true;
        rb.isKinematic = false;
        col.enabled = true;

        pickedObject = null;
    }
}