using System.Collections;
using System.Collections.Generic;
using System;
using TsumioSystem;

namespace TsumioSystem {

    /// <summary>
    /// TimeRecorederクラスで使うデータ群。
    /// 開始時刻と経過時間を保持している
    /// </summary>
    public class TimeRecorderData {

        ////=============================================================================
        //// Properties
        ////  
        ////=============================================================================

        /// <summary>
        /// Startメソッドが実行された時刻
        /// </summary>
        public DateTime? StartMoment { get; set; }

        /// <summary>
        /// Startメソッドが実行されてからの経過時間。ポーズ時間を引いて計算したい
        /// </summary>
        public TimeSpan ElapsedTime { get; set; }

        /// <summary>
        /// ポーズ中の時間
        /// </summary>
        public TimeSpan PausingTime { get; set; } = TimeSpan.Zero;
    }
}