using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatSwap : MonoBehaviour
{
    public Color glowClr;
    public float glowInts = .6f;
    public List<Renderer> _ren = new List<Renderer>();
    private Renderer[] _renderers;


    public void swapMat(Material mat)
    {
        GetComponent<Renderer>().material = mat;
    }

    public void HighlighMat(bool setter)
    {
        if (_ren.Count == 0)
            _ren.Add(GetComponent<Renderer>());


        foreach (var r in _ren)
        {
            Debug.Log($"ren {r.name}");
            if (!setter)
                foreach (var mat in r.materials)
                    mat.DisableKeyword("_EMISSION");

            else
            {
                foreach (var mat in r.materials)
                {
                    Debug.Log($"{mat.name}");
                    mat.SetColor("_EmissionColor", glowClr * glowInts);
                    mat.EnableKeyword("_EMISSION");
                }
            }
        }
    }
}

public class affectChildren
{
}