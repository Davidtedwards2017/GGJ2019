using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour {

    public Vector3 MouseWorldPos;
    public float DistanceFromCamera = 30;
    void Update()
    {
        Vector3 world;//= Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3 (0, 0, DistanceFromCamera));
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            world = hit.point;
        }
        else
        {
            world = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, DistanceFromCamera));
        }

        transform.position = world;

        //RaycastHit[] allHit = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
        //foreach (RaycastHit hit in allHit)
        //{
        //    Debug.Log(hit.collider.gameObject.name);
        //    if (hit.collider.gameObject.name == "Cube A")
        //    {
        //        //do something here
        //        break;
        //    }
        //}
    }
}
