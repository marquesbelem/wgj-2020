using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {

    public enum ComparationType {
        Equals,
        Different,
        LesserThan,
        GreaterThan,
        LesserThanOrEquals,
        GreaterThanOrEquals
    }

    public enum MessageType {
        Start, 
        Update, 
        FixedUpdate, 
        LateUpdate, 
        Enable, 
        Disable, 
        Destroy, 
        ApplicationFocus, 
        ApplicationPause, 
        ApplicationQuit
    }

    public enum RoundMethod {
        Round,
        Floor,
        Ceil
    }

    public enum ColorSystem {
        RGB,
        HSV,
        HSL,
        HCV,
        HCL,
        RGBA,
        HSVA,
        HSLA,
        HCVA,
        HCLA
    }

    [Flags] public enum TransformProperty {
        None = 0,
        Position = 1,
        Rotation = 2,
        Scale = 4
    }

}

