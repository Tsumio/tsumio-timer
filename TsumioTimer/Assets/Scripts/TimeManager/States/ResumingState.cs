using System.Collections;
using System.Collections.Generic;
using System;

namespace TsumioSystem {

    /// <summary>
    /// 再開状態を表す
    /// </summary>
    public class ResumingState : IRecorderState {

        ////=============================================================================
        //// Local Field
        ////  
        ////=============================================================================

        private IRecorderState _nextState;

        /// <summary>
        /// 親から渡されてきた記録中の情報。
        /// プロパティはPublicで書き込み可能になっているが、このクラス内では書き込まないで欲しい。
        /// </summary>
        private TimeRecorderData _data;

        ////=============================================================================
        //// Constructor
        ////  
        ////=============================================================================

        public ResumingState(IRecorderState nextState, TimeRecorderData data) {
            _nextState = nextState;
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
            //StartMomentがnullならば、計算せずにそのままnullを返す。
            if(!_data.StartMoment.HasValue) {
                return null;
            }
            return DateTime.Now - _data.StartMoment.Value;
        }

        /// <summary>
        /// RecordingStateを返す
        /// </summary>
        /// <returns></returns>
        public IRecorderState GetNextState() {
            return _nextState;
        }
    }
}