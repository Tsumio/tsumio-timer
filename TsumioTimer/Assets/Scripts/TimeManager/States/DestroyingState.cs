using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TsumioSystem {

    /// <summary>
    /// 記録の破棄のための状態
    /// </summary>
    public class DestroyingState : IRecorderState {

        ////=============================================================================
        //// Local Field
        ////  
        ////=============================================================================

        private IRecorderState _nextState;

        ////=============================================================================
        //// Constructor
        ////  
        ////=============================================================================

        public DestroyingState(IRecorderState nextState) {
            _nextState = nextState;
        }

        ////=============================================================================
        //// Public Method
        ////  
        ////=============================================================================

        /// <summary>
        /// 最小値を返して、初期化させる
        /// </summary>
        /// <returns></returns>
        public DateTime? GetNextStartMoment() {
            return DateTime.MinValue;
        }

        /// <summary>
        /// Zeroを返して、初期化させる
        /// </summary>
        /// <returns></returns>
        public TimeSpan? GetNextElapsedTime() {
            return TimeSpan.Zero;
        }

        public IRecorderState GetNextState() {
            return _nextState;
        }
    }
}
