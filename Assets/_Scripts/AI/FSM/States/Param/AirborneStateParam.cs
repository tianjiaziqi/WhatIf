using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhatIf
{
    public enum EAirborneEnterType
    {
        FromJump,
        FromFall
    }
    public class AirborneStateParam : StateEnterParamBase
    {
        public EAirborneEnterType EntryType { get; private set; }

        public static AirborneStateParam FromJump() => new AirborneStateParam { EntryType = EAirborneEnterType.FromJump };
        public static AirborneStateParam FromFall() => new AirborneStateParam { EntryType = EAirborneEnterType.FromFall };
    }
}
