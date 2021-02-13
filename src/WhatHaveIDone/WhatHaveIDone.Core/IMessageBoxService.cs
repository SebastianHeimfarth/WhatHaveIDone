namespace WhatHaveIDone.Core
{
    public interface IMessageBoxService
    {
        bool AskYesNoQuestion(string question, string caption);
    }
}