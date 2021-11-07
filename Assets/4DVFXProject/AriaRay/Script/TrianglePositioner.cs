using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrianglePositioner : MonoBehaviour
{
    public GameObject originObj;
    public GameObject centerObj;
    public int numOfVerteces = 3;
    public float radius = 2.0f;
    private List<GameObject> copiedObjects;

    void Start()
    {
        this.CreateTriangle();
    }

    void Update()
    {
        
    }
    
    void CreateTriangle()
    {
        this.copiedObjects = new List<GameObject>(3);
        this.copiedObjects.Add(originObj);
        for (int i = 1; i < this.numOfVerteces; i++)
        {
            GameObject copiedObject = Object.Instantiate(originObj) as GameObject;
            copiedObject.transform.parent = originObj.transform.parent;
            this.copiedObjects.Add(copiedObject);
        }

        //Vector3[] vertices = new Vector3[3];
        for (int i = 0; i < this.numOfVerteces; i++)
        {
            float angle = i * (360.0f / numOfVerteces) * Mathf.Deg2Rad;
            //vertices[i] = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
            Vector3 position = this.copiedObjects[i].transform.position;
            position.x = radius * Mathf.Sin(angle);
            position.z = radius * Mathf.Cos(angle);
            //Vector3 vertexToCenterDir = Vector3.zero - vertex;
            this.copiedObjects[i].transform.position = position;
            
            this.copiedObjects[i].transform.LookAt(centerObj.transform);
        }
        
    }
}
