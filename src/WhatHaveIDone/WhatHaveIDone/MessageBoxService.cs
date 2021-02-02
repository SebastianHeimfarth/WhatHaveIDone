using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using WhatHaveIDone.Core;

namespace WhatHaveIDone
{
    public class MessageBoxService : IMessageBoxService
    {
        public bool AskYesNoQuestion(string question, string caption)
        {
            return MessageBox.Show(question, caption, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
    }
}
