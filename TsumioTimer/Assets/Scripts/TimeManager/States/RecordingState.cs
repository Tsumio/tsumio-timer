using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TsumioSystem {

    /// <summary>
    /// 記録中の状態を表す。
    /// Updateメソッドで時間を更新されるのはこの状態
    /// </summary>
    public class RecordingState : IRecorderState {

        ////=============================================================================
        //// Local Field
        ////  
        ////=============================================================================

        /// <summary>
        /// 親から渡されてきた記録中の情報。
        /// プロパティはPublicで書き込み可能になっているが、このクラス内では書き込まないで欲しい。
        /// </summary>
        private TimeRecorderData _data;

        ////=============================================================================
        //// Constructor
        ////  
        ////=============================================================================

        public RecordingState(TimeRecorderData data) {
            _data = data;
        }

        ////=============================================================================
        //// Public Method
        ////  
        ////=============================================================================
        public DateTime? GetNextStartMoment() {
            return null;
        }

        /// <summary>
        /// 毎フレーム時刻を取得して、経過時間を計算している。
        /// Nullも対応。
        /// </summary>
        /// <returns></returns>
        public TimeSpan? GetNextElapsedTime() {
            //StartMomentがnullならば、計算せずにそのままnullを返す。
            if(!_data.StartMoment.HasValue) {
                return null;
            }
            return DateTime.Now - _data.StartMoment.Value;
        }

        public IRecorderState GetNextState() {
            return this;
        }
    }
}
