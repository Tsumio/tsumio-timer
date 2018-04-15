using System.Collections;
using System.Collections.Generic;
using System;

namespace TsumioSystem {
    /// <summary>
    /// 現在どの記録状態なのかを表すインターフェイス
    /// </summary>
    public interface IRecorderState {
        /// <summary>
        /// 開始時刻を返す
        /// </summary>
        /// <returns></returns>
        DateTime? GetNextStartMoment();

        /// <summary>
        /// 経過時間を返す
        /// </summary>
        /// <returns></returns>
        TimeSpan? GetNextElapsedTime();

        /// <summary>
        /// 次の状態を返す
        /// </summary>
        /// <returns></returns>
        IRecorderState GetNextState();
    }
}
