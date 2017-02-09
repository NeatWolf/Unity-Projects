using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiElementHider {

    private List<CanvasRenderer> _elements;
    private string _tag;

    public UiElementHider(string tag)
    {
        _tag = tag;
        _elements = new List<CanvasRenderer>();
    }

    public void Hide()
    {
        _elements.Clear();
        var allElements = GameObject.FindGameObjectsWithTag(_tag);
        var activeElements = allElements.Where(e => e != null && e.activeSelf);
        HideElements(activeElements);
    }

    public void Show()
    {
        if (_elements == null) return;
        ShowElements();
    }

    private void HideElements(IEnumerable<GameObject> elements)
    {
        var canvasRenderers = elements.Select(t => t.GetComponentsInChildren<CanvasRenderer>());
        if (canvasRenderers == null) return;

        foreach(var rends in canvasRenderers.Where(r => r != null))
        {
            foreach(var rend in rends.Where(r => r != null))
            {
                if (rend.GetAlpha() > 0f)
                {
                    rend.SetAlpha(0);
                    _elements.Add(rend);
                }
            }
        }
    }

    private void ShowElements()
    {
        foreach (var rend in _elements.Where(r => r != null))
        {
            rend.SetAlpha(1);
        }
    }
}
