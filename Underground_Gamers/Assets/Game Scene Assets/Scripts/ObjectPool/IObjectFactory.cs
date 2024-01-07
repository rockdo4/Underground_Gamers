using System.Collections;
using UnityEngine;

public interface IObjectFactory<T> where T : Object
{
    T CreateObject();
}
