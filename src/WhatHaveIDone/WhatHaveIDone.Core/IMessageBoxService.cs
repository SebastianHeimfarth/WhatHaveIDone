using System;
using System.Collections.Generic;
using System.Text;

namespace WhatHaveIDone.Core
{
    public interface IMessageBoxService
    {
        bool AskYesNoQuestion(string question, string caption);
    }
}
