using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CalligraphyAssistantMain.Code
{
    public class MessageBoxEx
    {
        public static void ShowInfo(string text, Window window = null)
        {
            ShowInfo(text, "提示", window);
        }

        public static void ShowInfo(string text, string title, Window window = null)
        {
            if (window != null)
            {
                MessageBox.Show(window, text, title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public static void ShowError(string text, Window window = null)
        {
            ShowError(text, "错误", window);
        }

        public static void ShowError(string text, string title, Window window = null)
        {
            if (window != null)
            {
                MessageBox.Show(window, text, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static MessageBoxResult ShowQuestion(string text, string title)
        {
            return MessageBox.Show(text, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public static MessageBoxResult ShowQuestion(Window owner, string text, string title)
        {
            return MessageBox.Show(owner, text, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
