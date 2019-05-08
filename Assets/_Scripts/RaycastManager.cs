using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(ScriptsOrder.Managers)]
public class RaycastManager : MonoBehaviour
{
    public Camera cam;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                hit.transform.GetComponent<IIntractable>()?.Tap();
            }
        }
    }
}