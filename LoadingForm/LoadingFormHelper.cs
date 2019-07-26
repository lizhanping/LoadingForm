/*
*---------------------------------
*|		All rights reserved.
*|		author: lizhanping
*|		version:1.0
*|		File: LoadingFormHelper.cs
*|		Summary: 
*|		Date: 2019/7/26 16:15:30
*---------------------------------
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace LoadingForm
{
    /// <summary>
    /// 等待窗体帮助类
    /// </summary>
    public  class LoadingFormHelper
    {
        private static  WaitingForm instance;
        public static event EventHandler WaitCompleted;
        
        private static WaitingForm Defalut
        {
            get
            {
                if (instance == null||instance.IsDisposed)
                {
                    instance = new WaitingForm();
                    instance.WaitingCompleted += Instance_WaitingCompleted;
                    Console.WriteLine("chushihua 1 ci");
                }

                return instance;
            }
        }

        private static void Instance_WaitingCompleted(object sender, EventArgs e)
        {
            WaitCompleted?.Invoke(sender, e);
        }

        public static void Show()
        {
            Show("请稍候...");
        }

        public static void Show(string tip)
        {
            Show(tip, 100);
        }

        public static void Show(string tip,int maxValue)
        {
            Show(tip, maxValue, ProgressRunMode.Forever);
        }

        public static void Show(string tip,int maxValue,ProgressRunMode mode)
        {
            Defalut.Tips = tip;
            Defalut.MaxValue = maxValue;
            Defalut.Mode = mode;
            Defalut.Show();
        }

        public static void Close()
        {
            instance?.Close();
        }

        public static void UpdateProgress(int value)
        {
            if (instance == null || instance.IsDisposed)
                return;
            instance.Progress = value;
        }

        public static void ClearAllEvent()
        {
            if (WaitCompleted == null)
                return;
            Delegate[] dels = WaitCompleted.GetInvocationList();
            foreach (var del in dels)
            {
                object delObj = del.GetType().GetProperty("Method").GetValue(del, null);
                string funcName = (string)delObj.GetType().GetProperty("Name").GetValue(delObj, null);////方法名
                Console.WriteLine(funcName);
                WaitCompleted -= del as EventHandler;
            }
        }
    }
}
