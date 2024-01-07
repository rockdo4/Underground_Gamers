using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFactory : IObjectFactory<TextMeshPro>
{
    private TextMeshPro textPrefab;
    public TextFactory(TextMeshPro textPrefab)
    {
        this.textPrefab = textPrefab;
    }

    public TextMeshPro CreateObject()
    {
        return GameObject.Instantiate(textPrefab);
    }
}
