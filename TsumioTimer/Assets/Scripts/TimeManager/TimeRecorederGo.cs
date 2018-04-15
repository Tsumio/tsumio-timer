using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace TsumioSystem {
    /// <summary>
    /// TimeRecorderの簡単な使用方法を示すためのサンプルクラス。
    /// 各メソッドは最小サンプルとなっているため、実際にTimeRecorderクラスを使用する際に参考にしてほしい。
    /// </summary>
    public class TimeRecorederGo : MonoBehaviour {

        ////=============================================================================
        //// Local Field
        ////  
        ////=============================================================================

        /// <summary>
        /// レコーダー
        /// </summary>
        ITimeRecorder _recorder = new TimeRecorder();

        /// <summary>
        /// 開始時刻を表すテキスト
        /// </summary>
        [SerializeField]
        private Text _startText;

        /// <summary>
        /// 経過時間を表すテキスト
        /// </summary>
        [SerializeField]
        private Text _elapsedText;

        ////=============================================================================
        //// MonoBehaviour
        ////  
        ////=============================================================================

        public virtual void Update() {
            //タイマーを利用するには、以下のようにUpdateメソッド内でTimeRecorderのインスタンスのUpdateメソッドを呼ぶ必要がある。
            _recorder.Update();
            UpdateTexts();
        }

        ////=============================================================================
        //// Public Method
        ////  
        ////=============================================================================

        /// <summary>
        /// 記録の開始
        /// </summary>
        public void StartRecording() {
            /*Note:ここでNewしているが、StartRecording2と辻褄を合わせるためにインスタンスを生成しているだけ。
              本来はStartメソッドか何かで一度インスタンスを生成しておけば、何度もインスタンスを生成する必要はない。*/
            _recorder = new TimeRecorder();
            _recorder.Start();
        }

        /// <summary>
        /// 記録の開始と同時にイベントの登録をする
        /// 3秒後にデバッグログにメッセージを表示。
        /// </summary>
        public void StartRecoding2() {
            /*Note:ここでNewしているが、StartRecordingと辻褄を合わせるためにインスタンスを生成している。
                   本来はStartメソッドか何かで一度インスタンスを生成しておけば、何度もインスタンスを生成する必要はない。*/
            _recorder = new TimeRecorder(new TimeSpan(0, 0, 3), () => Debug.Log("イベント発火したよー"));
            _recorder.Start();
        }

        /// <summary>
        /// 記録の一時停止
        /// </summary>
        public void PauseRecording() {
            _recorder.Pause();
        }

        /// <summary>
        /// 記録の再開
        /// </summary>
        public void ResumeRecording() {
            _recorder.Resume();
        }

        /// <summary>
        /// 記録の破棄
        /// </summary>
        public void DestroyRecording() {
            _recorder.Destroy();
        }

        ////=============================================================================
        //// Private Method
        ////  
        ////=============================================================================

        /// <summary>
        /// 画面上の文字を更新する
        /// </summary>
        private void UpdateTexts() {
            //必要なら書式を指定する
            _startText.text   = $"記録開始時刻：{_recorder.StartMoment.GetValueOrDefault().ToString("HH:mm:ss")}";
            _elapsedText.text = $"経過時間：{_recorder.ElapsedTime.ToString()}";
        }
    }
}
