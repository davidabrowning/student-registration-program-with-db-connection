﻿namespace StudentRegistrationProgramWithDBConnection
{
    internal interface IOutput
    {
        void PrintTitle(string text);
        void PrintMessage(string text);
        void PrintError(string text);
        void PrintWarning(string text);
        void PrintSuccess(string text);
        void PrintInactive(string text);
        void PrintPrompt(string text);
        void PrintList<T>(IEnumerable<T> list);
        void ConfirmToContinue();
        void PrintLine();
        void PrintSectionDivider();
        void Clear();
    }
}
