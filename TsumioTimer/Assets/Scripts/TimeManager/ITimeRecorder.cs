using System.Collections;
using System.Collections.Generic;
using System;

namespace TsumioSystem {

    public interface ITimeRecorder {
        ////=============================================================================
        //// Properties
        ////  
        ////=============================================================================
        DateTime? StartMoment { get; }
        TimeSpan ElapsedTime { get; }
        TimeSpan PausingTime { get; }

        ////=============================================================================
        //// Public Method
        ////  
        ////=============================================================================
        void Start();
        void Pause();
        void Resume();
        void Destroy();
        void Update();
    }

}
