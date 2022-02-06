using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ExecuteInEditMode allows functions to run in the inspector
[ExecuteInEditMode]
public class OutlineManager : MonoBehaviour
{
    GameObject outline;
    Material outlineMaterial;
    [Range(0,10)][SerializeField] float outlineSize = 1f;
    [ColorUsageAttribute(true, true)] [SerializeField] Color outlineColour;

    // Reset is called when this script is added
    private void Reset()
    {
        if(transform.parent)
        {
            if (GameObject.Find("/" + transform.parent.gameObject.name + "/" + gameObject.name + "/Outline"))
            {
                outline = GameObject.Find("/" + transform.parent.gameObject.name + "/" + gameObject.name + "/Outline");
                DestroyImmediate(outline);
            }
        }
        else if (GameObject.Find("/" + gameObject.name + "/Outline"))
        {
            outline = GameObject.Find("/"+ gameObject.name +"/Outline");
            DestroyImmediate(outline);
        }

        outline = Instantiate(this.gameObject, new Vector3(0, 0, 0), Quaternion.identity);
        outline.name = "Outline";
        DestroyImmediate(outline.GetComponent<OutlineManager>());
        outline.transform.localScale = new Vector3(-1,1,1);
        outline.transform.SetParent(this.transform,false);             
        outlineMaterial = new Material(Shader.Find("Shader Graphs/Outline Shader"));
        outline.GetComponent<Renderer>().material = outlineMaterial;
        outline.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        outline.GetComponent<Renderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        outline.GetComponent<Renderer>().reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        outline.GetComponent<Renderer>().allowOcclusionWhenDynamic = false;
    }


    
    void Start()
    {
        if (transform.parent)
        {
            if (outline == null)
            {
                outline = GameObject.Find("/" + transform.parent.gameObject.name + "/" + gameObject.name + "/Outline");
                outlineMaterial = outline.GetComponent<Renderer>().sharedMaterial;
            }
        }
        else if (outline == null)
        {
            outline = GameObject.Find("/" + gameObject.name + "/Outline");
            outlineMaterial = outline.GetComponent<Renderer>().sharedMaterial;
        }
        else if (outline)
        {
            outlineMaterial = outline.GetComponent<Renderer>().sharedMaterial;
        }

    }

    void Update()
    {

        outlineMaterial.SetColor("_Color", outlineColour);
        outlineMaterial.SetFloat("_Size", outlineSize);

    }

    private void OnDestroy()
    {
        DestroyImmediate(outline);
    }
}
