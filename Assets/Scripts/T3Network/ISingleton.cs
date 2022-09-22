using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISingleton 
{
    void Remove();
}

public interface ISigleton<T> : ISingleton where T : class, new()
{
    void StartUp(T instance);
}
