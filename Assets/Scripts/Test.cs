using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    
}

public interface IGearHandler
{
    GearsGridSimulator Simulator { get; }
    
    IObservable<Test> Observable { get; }
    
}
