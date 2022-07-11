using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager 
{
    private static readonly Lazy<UserDataManager> _instance = new Lazy<UserDataManager>(() => new UserDataManager());
    public static UserDataManager Instance { get { return _instance.Value; } }

}
