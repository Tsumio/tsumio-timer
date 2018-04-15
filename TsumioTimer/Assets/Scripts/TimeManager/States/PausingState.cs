using System;
using System.Collections;
using System.Collections.Generic;
using TsumioSystem;

namespace TsumioSystem {

    /// <summary>
    /// 一時停止状態を表すクラス
    /// </summary>
    public class PausingState : IRecorderState {

        ////=============================================================================
        //// Local Field
        ////  
        ////=============================================================================
        
        /// <summary>
        /// 親から渡されたデータ
        /// </summary>
        private TimeRecorderData _data;

        ////=============================================================================
        //// Constructor
        ////  
        ////=============================================================================

        public PausingState(TimeRecorderData data) {
            _data = data;
        }


        ////=============================================================================
        //// Public Method
        ////  
        ////=============================================================================
        public DateTime? GetNextStartMoment() {
            return null;
        }

        public TimeSpan? GetNextElapsedTime() {
            return _data.ElapsedTime;
        }

        public IRecorderState GetNextState() {
            return this;
        }
    }
}
