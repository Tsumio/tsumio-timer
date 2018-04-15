using System;
using System.Collections;
using System.Collections.Generic;
using TsumioSystem;

namespace TsumioSystem {

    /// <summary>
    /// 完全な停止状態を表すクラス
    /// </summary>
    public class NoWorkingState : IRecorderState {
        ////=============================================================================
        //// Public Method
        ////  
        ////=============================================================================
        public DateTime? GetNextStartMoment() {
            return null;
        }

        public TimeSpan? GetNextElapsedTime() {
            return null;
        }

        public IRecorderState GetNextState() {
            return this;
        }
    }
}