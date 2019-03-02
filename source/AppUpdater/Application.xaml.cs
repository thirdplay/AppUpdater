using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AppUpdater
{
    /// <summary>
    /// Application.xaml の相互作用ロジック
    /// </summary>
    public partial class Application
    {
        /// <summary>
        /// 現在の <see cref="AppDomain"/> の <see cref="Application"/> オブジェクトを取得します。
        /// </summary>
        public static Application Instance => Current as Application;

        /// <summary>
        /// 静的コンストラクタ。
        /// </summary>
        static Application()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => ReportException(sender, args.ExceptionObject as Exception);
        }

        /// <summary>
        /// 起動イベント。
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            this.DispatcherUnhandledException += (sender, args) =>
            {
                ReportException(sender, args.Exception);
                args.Handled = true;
            };

            // 親メソッド呼び出し
            base.OnStartup(e);
        }

        /// <summary>
        /// 例外報告
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="exception">例外</param>
        /// <param name="isShutdown">シャットダウンするかどうか</param>
        public static void ReportException(object sender, Exception exception, bool isShutdown = true)
        {
            #region const

            const string messageFormat = @"
===========================================================
ERROR, date = {0}, sender = {1},
{2}
";
            const string path = "error.log";

            #endregion const

            try
            {
                var message = string.Format(messageFormat, DateTimeOffset.Now, sender, exception);

                Debug.WriteLine(message);
                File.AppendAllText(path, message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            // 終了
            if (isShutdown)
            {
                Current.Shutdown();
            }
        }
    }
}
