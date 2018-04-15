using System;
using System.Collections;
using System.Collections.Generic;
using TsumioSystem;

namespace TsumioSystem {
    /// <summary>
    /// TimeRecorderでStartメソッドが呼ばれたときの状態を表す
    /// </summary>
    public class StartingState : IRecorderState {

        ////=============================================================================
        //// Local Field
        ////  
        ////=============================================================================

        private IRecorderState _nextState;

        ////=============================================================================
        //// Constructor
        ////  
        ////=============================================================================

        public StartingState(IRecorderState nextState) {
            _nextState = nextState;
        }

        ////=============================================================================
        //// Public Method
        ////  
        ////=============================================================================

        /// <summary>
        /// 現在時刻を返す
        /// </summary>
        /// <returns></returns>
        public DateTime? GetNextStartMoment() {
            return DateTime.Now;
        }

        /// <summary>
        /// ゼロを返して初期化する
        /// </summary>
        /// <returns></returns>
        public TimeSpan? GetNextElapsedTime() {
            return TimeSpan.Zero;
        }

        /// <summary>
        /// 次の状態を返す（RecordingState)
        /// </summary>
        /// <returns></returns>
        public IRecorderState GetNextState() {
            return _nextState;
        }
    }
}