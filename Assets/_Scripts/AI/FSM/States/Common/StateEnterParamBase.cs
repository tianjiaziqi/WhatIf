using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all state parameters, all state param need to inherit from this.
/// </summary>
public abstract class StateEnterParamBase
{
    
}

// Special case: No parameter
public class NoParam : StateEnterParamBase
{
    // Singleton
    public static readonly NoParam Instance = new NoParam();
    private NoParam(){}
}
