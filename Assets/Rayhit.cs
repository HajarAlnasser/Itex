using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Oculus.Interaction;
using Unity.Mathematics;
[RequireComponent(typeof(LineRenderer))]
public class ExampleClass : MonoBehaviour
{
    public AudioSource audioSource;
    public Animator animator;
    public float distance;
    public float width;
    public LayerMask layerMask;
    public Color color = Color.blue;
    public Color hovercolor = Color.red;
    private List<EyeInteractable> eye =new List<EyeInteractable>();
    private LineRenderer lineRenderer;
    public TextMeshProUGUI u;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        setupline();
    }
    void setupline()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.startColor= color;
        lineRenderer.endColor=color;
        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1,new Vector3(transform.position.x,transform.position.y,transform.position.z+distance));
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 raycast = transform.TransformDirection(Vector3.fwd) * distance;
        if (Physics.Raycast(transform.position, raycast, out hit, Mathf.Infinity, layerMask))
        {
            audioSource=hit.transform.GetComponent<AudioSource>();
            audioSource.Play();
            animator=hit.transform.gameObject.GetComponent<Animator>();
            animator.SetBool("YAY",true);
            u.text = "you found me";
            unselect();
            lineRenderer.startColor = hovercolor;
            lineRenderer.endColor = hovercolor;
            var Eyeinteractable = hit.transform.GetComponent<EyeInteractable>();
            eye.Add(Eyeinteractable);
            Eyeinteractable.IsHover = true;
        
        }
        else
        {
            lineRenderer.startColor = hovercolor;
            unselect(false);
            animator.SetBool("YAY", false);
            u.text = "";
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }
    void unselect(bool clear = false)
    {
        foreach (var eyeinteractable in eye)
        {
            eyeinteractable.IsHover = false;
        }
            
        if (clear)
        {
            eye.Clear();
       }

    }
    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    //void Update()
    //{
    //    Vector3 fwd = transform.TransformDirection(Vector3.forward);
    //    Ray ray = new Ray(transform.position, fwd);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        u.text="There is something in front of the object!";
    //        Debug.Log("There is something in front of the object!");
    //    }
    //}


//    private void FixedUpdate()
//    {
//        // Bit shift the index of the layer (8) to get a bit mask
//        int layerMask = 1 << 8;

//    // This would cast rays only against colliders in layer 8.
//    // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
//    layerMask = ~layerMask;

//    RaycastHit hit;
//        // Does the ray intersect any objects excluding the player layer
//        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
//        {
//            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward)* hit.distance, Color.yellow);
//    Debug.Log("Did Hit");
//        }
//        else
//{
//    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
//    Debug.Log("Did not Hit");
//}
//    }
}