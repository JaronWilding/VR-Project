using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInputs : VRInputModule
{

    public override void Process()
    {
        base.Process();

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            ProcessPress(p_Data);
        }

        // Release
        if (Input.GetKeyUp(KeyCode.JoystickButton0))
        {
            ProcessRelease(p_Data);
        }
    }
}
