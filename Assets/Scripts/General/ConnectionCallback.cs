using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ConnectionCallback(bool isSuccess, string message = "");
public delegate void ConnectionCallback<in T>(T value, bool isSuccess, string info = "");
public delegate void ConnectionCallback<in T1, in T2>(T1 value1, T2 value2, bool isSuccess, string info = "");
public delegate void ConnectionCallback<in T1, in T2, in T3>(T1 value1, T2 value2, T3 value3, bool isSuccess, string info = "");
