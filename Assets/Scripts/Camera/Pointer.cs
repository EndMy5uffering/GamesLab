using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(SpringSnapToTarget))]
public class Pointer : MonoBehaviour
{
    private GameObject selected;
    private List<Material> tmat = new List<Material>();

    public Material selectMat;

    SpringSnapToTarget spring;

    void Awake()
    {
        spring = this.GetComponent<SpringSnapToTarget>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void ResetSelected()
    {
        if(selected == null) return;

        selected.GetComponent<Renderer>().SetMaterials(tmat);
        selected = null;
    } 

    void SelectObj(GameObject obj)
    {
        selected = obj;
        selected.GetComponent<Renderer>().GetMaterials(tmat);
        selected.GetComponent<Renderer>().SetMaterials(new List<Material>() {selectMat});
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        ResetSelected();

        if ( !Physics.Raycast (ray,out hit,10000.0f)) return;

        GameObject hitobj = hit.transform.gameObject;
        if(!hitobj.CompareTag("Clickable")) return;

        if(hitobj == selected) return;

        SelectObj(hitobj);

        if(Input.GetMouseButtonUp(0)) 
        {
            //orbitcam.target = hitobj.transform;
            spring.SetTarget(hitobj.transform);
        }
    }
}
