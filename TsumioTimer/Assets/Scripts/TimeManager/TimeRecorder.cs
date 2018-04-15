using System.Collections;
using System.Collections.Generic;
using System;
using TsumioSystem;

namespace TsumioSystem {

    /// <summary>
    /// 時間を記録するためのクラス
    /// 
    /// 特定の一瞬を表す：Moment（時刻）
    /// 一定の期間を表す：Time（時間）
    /// </summary>
    public class TimeRecorder : ITimeRecorder {
        ////=============================================================================
        //// Local Field
        ////  
        ////=============================================================================

        /// <summary>
        /// 開始状態
        /// </summary>
        private StartingState _starting;

        /// <summary>
        /// 記録中状態
        /// </summary>
        private RecordingState _recording;

        /// <summary>
        /// 一時停止状態
        /// </summary>
        private PausingState _pausing;

        /// <summary>
        /// 破棄状態
        /// </summary>
        private DestroyingState _destroying;

        /// <summary>
        /// 再開状態
        /// </summary>
        private ResumingState _resuming;

        /// <summary>
        /// 完全な停止状態
        /// </summary>
        private NoWorkingState _noWorking;

        /// <summary>
        /// 開始時刻と経過時間を保持するデータ
        /// </summary>
        private TimeRecorderData _recorderData;

        /// <summary>
        /// イベント発火までの時間
        /// </summary>
        private TimeSpan _firingTime = TimeSpan.MaxValue;

        /// <summary>
        /// 登録されたイベント
        /// </summary>
        private Action _registerdAction;


        //ハイパーブサイクコード。一時停止のための一時変数。クソ。
        private TimeSpan _tempPausingTime = TimeSpan.Zero;
        private DateTime _tempPausingMoment = DateTime.Now;

        ////=============================================================================
        //// Properties
        ////  
        ////=============================================================================

        /// <summary>
        /// 現在の状態
        /// </summary>
        public IRecorderState CurrentState { get; private set; }

        /// <summary>
        /// Startメソッドが実行された時刻
        /// </summary>
        public DateTime? StartMoment {
            get {
                return _recorderData.StartMoment;
            }

            private set {
                //HACK:MinValueが代入されたとき、nullと同等に扱っている。
                //これはUpdate()内の処理を正常に回すためだが、もっとうまく書けるかも。
                if(value == DateTime.MinValue) {
                    _recorderData.StartMoment = null;
                }else {
                    _recorderData.StartMoment = value;
                }
            }
        }

        /// <summary>
        /// Startメソッドが実行されてからの経過時間
        /// </summary>
        public TimeSpan ElapsedTime {
            get {
                if(_recorderData.PausingTime >= TimeSpan.Zero) {
                    return _recorderData.ElapsedTime - _recorderData.PausingTime;
                }
                return _recorderData.ElapsedTime;
            }

            private set {
                _recorderData.ElapsedTime = value;
            }
        }

        /// <summary>
        /// 一時停止していた時間
        /// </summary>
        public TimeSpan PausingTime {
            get {
                return _recorderData.PausingTime;
            }

            private set {
                _recorderData.PausingTime = value;
            }
        }

        ////=============================================================================
        //// Constructor
        ////  
        ////=============================================================================

        /// <summary>
        /// 単なる記録の場合は引数を指定しない
        /// </summary>
        public TimeRecorder() {
            //記録インスタンスの生成
            _recorderData = new TimeRecorderData();

            //状態のインスタンスを生成
            CreateStateInstances();

            //現在の状態を完全停止に
            CurrentState = _noWorking;
        }

        /// <summary>
        /// 特定の時間でイベントを発火したい場合に使用
        /// </summary>
        /// <param name="firingTime">イベントが発火するまでにかかる経過時間</param>
        /// <param name="action">発火するイベント</param>
        public TimeRecorder(TimeSpan firingTime, Action action) : this() {
            _firingTime      = firingTime;
            _registerdAction = action;
        }

        ////=============================================================================
        //// Public Method
        ////  
        ////=============================================================================

        /// <summary>
        /// 時間の記録を開始する
        /// </summary>
        public void Start() {
            CurrentState = _starting;

            //HACK:これ散らばってるの最高にイヤ。
            _recorderData.PausingTime = TimeSpan.Zero;
        }

        /// <summary>
        /// 時間の記録を一時停止する
        /// </summary>
        public void Pause() {
            if(!CanPause) {
                return;
            }

            CurrentState = _pausing;

            //HACK:完全な汚物
            //一時停止のための初期化。別のクラスでこういった処理はすべき。
            _tempPausingMoment = DateTime.Now;
            _tempPausingTime = TimeSpan.Zero;
        }

        /// <summary>
        /// 時間の記録を再開する
        /// </summary>
        public void Resume() {
            if(!CanResume) {
                return;
            }

            CurrentState = _resuming;

            //HACK:汚すぎる。どうにかしておくれ
            //一時停止したあと、保存しておいた停止時間を記憶している
            _recorderData.PausingTime += _tempPausingTime;
            _tempPausingTime = TimeSpan.Zero;
        }

        /// <summary>
        /// 時間の記録を破棄する
        /// </summary>
        public void Destroy() {
            CurrentState = _destroying;

            //HACK:これ散らばってるの最高にイヤ。
            _recorderData.PausingTime = TimeSpan.Zero;
        }

        /// <summary>
        /// 時間の更新をする
        /// </summary>
        public void Update() {
            //HACK:ゴミコード。一時停止用にちゃんと調整すべき。クソコードでロジックが見えにくくなっている。
            //一時停止状態の場合、その停止時間を記憶し続ける
            if(CurrentState.Equals(_pausing)) {
                _tempPausingTime = DateTime.Now - _tempPausingMoment;
            }

            //本来の処理はここから！
            StartMoment  = CurrentState.GetNextStartMoment() ?? StartMoment;
            ElapsedTime  = CurrentState.GetNextElapsedTime() ?? ElapsedTime;
            CurrentState = CurrentState.GetNextState();//NOTE:状態の取得は最後にしないと、次の状態の情報でUpdateが更新されてしまう

            UpdateFiringAction();
        }

        ////=============================================================================
        //// Private Method
        ////  
        ////=============================================================================

        /// <summary>
        /// 初期化のために状態のインスタンスを生成
        /// </summary>
        private void CreateStateInstances() {
            //NOTE:初期化の順番に注意する。_recordingは他の状態に渡している。
            _recording  = new RecordingState(_recorderData);
            _starting   = new StartingState(_recording);
            _resuming   = new ResumingState(_recording, _recorderData);
            _pausing    = new PausingState(_recorderData);
            _noWorking = new NoWorkingState();
            _destroying = new DestroyingState(_noWorking);
        }

        /// <summary>
        /// 登録されたイベントが発火可能なら発火する
        /// </summary>
        private void UpdateFiringAction() {
            if(CanFireAction) {
                //イベント発火
                _registerdAction?.Invoke();

                //イベント発火後、初期化する
                _firingTime      = TimeSpan.MaxValue;
                _registerdAction = null;
                Destroy();
            }
        }

        /// <summary>
        /// イベント発火可能かどうか
        /// 経過時間とコンストラクタで設定された_firingTimeを確かめている。
        /// </summary>
        private bool CanFireAction => ElapsedTime >= _firingTime;

        /// <summary>
        /// 一時停止が可能かどうか
        /// </summary>
        private bool CanPause => (ElapsedTime > TimeSpan.Zero) && (!CurrentState.Equals(_noWorking));

        /// <summary>
        /// 再開が可能かどうか
        /// </summary>
        private bool CanResume => (ElapsedTime > TimeSpan.Zero) && CurrentState.Equals(_pausing);
    }

}
