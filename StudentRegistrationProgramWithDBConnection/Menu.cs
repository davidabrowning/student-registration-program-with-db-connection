﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistrationProgramWithDBConnection
{
    internal class Menu
    {
        private IOutput output;
        private IInput input;
        private DatabaseTransfer databaseTransfer;

        private const string MainMenuTitle = "Huvudmeny";
        private const string MainMenuOptionRegister = "Registrera ny student";
        private const string MainMenuOptionEditOne = "Ändra student";
        private const string MainMenuOptionListAll = "Lista alla studenter";
        private const string MainMenuOptionQuit = "Avsluta programmet";
        private const string MainMenuPrompt = "Ditt val:";
        private const string RegisterMenuTitle = "Registrera ny student";
        private const string EditMenuTitle = "Ändra student";
        private const string EditMenuPromptStudentId = "Student att ändra (ange student id-nummer):";
        private const string ListAllMenuTitle = "Lista alla studenter";
        private const string QuitMenuTitle = "Avsluta programmet";
        private const string InvalidMenuInputTitle = "Oväntad inmatning";
        private const string PromptFirstName = "Förnamn:";
        private const string PromptLastName = "Efternamn:";
        private const string PromptCity = "Stad:";
        private const string SuccessStudentRegistered = "Ny student registerad.";
        private const string SuccessStudentEdited = "Student uppdaterad.";
        private const string SuccessListComplete = "Listan klar.";
        private const string SuccessGoodbye = "Tack och hej då!";
        private const string WarningStudentIdNotFound = "Lyckades inte hitta student med detta id-nummer.";
        private const string WarningUnexpectedInput = "Oväntad inmatning. Försök igen.";
        private const string WarningStudentIsNull = "Student är null.";

        public Menu(IOutput output, IInput input, DatabaseTransfer databaseTransfer)
        {
            this.output = output;
            this.input = input;
            this.databaseTransfer = databaseTransfer;
        }
        public void Go()
        {
            ShowMainMenu();
        }
        public void ShowMainMenu()
        {
            output.PrintTitle(MainMenuTitle);
            output.PrintMessage($"[1] {MainMenuOptionRegister}");
            output.PrintMessage($"[2] {MainMenuOptionEditOne}");
            output.PrintMessage($"[3] {MainMenuOptionListAll}");
            output.PrintMessage($"[Q] {MainMenuOptionQuit}");
            output.PrintLine();
            output.PrintPrompt(MainMenuPrompt);
            HandleMainMenuSelection();
        }
        public void HandleMainMenuSelection()
        {
            switch(input.GetStringInput().ToUpper())
            {
                case "1":
                    ShowRegistrationMenu();
                    break;
                case "2":
                    ShowEditMenu();
                    break;
                case "3":
                    ShowStudentList();
                    break;
                case "Q":
                    ShowQuitProgram();
                    break;
                default:
                    ShowInvalidMenuInput();
                    break;
            }
        }
        public void ShowRegistrationMenu()
        {
            output.PrintTitle(RegisterMenuTitle);
            Student student = GetNewStudentFromUser();
            databaseTransfer.Add(student);
            output.PrintSuccess(SuccessStudentRegistered);
            output.ConfirmToContinue();
            ShowMainMenu();
        }
        private Student GetNewStudentFromUser()
        {
            return new Student()
            {
                FirstName = input.GetStringInput(PromptFirstName),
                LastName = input.GetStringInput(PromptLastName),
                City = input.GetStringInput(PromptCity)
            };
        }
        public void ShowEditMenu()
        {
            output.PrintTitle(EditMenuTitle);
            output.PrintList<Student>(databaseTransfer.AllStudents());
            output.PrintLine();
            int idToEdit = input.GetIntInput(EditMenuPromptStudentId);
            if (databaseTransfer.IsValidStudentId(idToEdit))
                EditStudent(idToEdit);
            else
                output.PrintWarning(WarningStudentIdNotFound);
            output.ConfirmToContinue();
            ShowMainMenu();
        }
        private void EditStudent(int studentId)
        {
            output.PrintTitle(EditMenuTitle);
            Student? originalStudent = databaseTransfer.AllStudents().Where(s => s.StudentId == studentId).FirstOrDefault();
            output.PrintMessage(originalStudent.ToString());
            output.PrintLine();
            Student updatedStudentInfo = GetNewStudentFromUser();
            databaseTransfer.Update(originalStudent, updatedStudentInfo);
            output.PrintLine();
            output.PrintMessage(originalStudent.ToString());
            output.PrintSuccess(SuccessStudentEdited);
        }
        public void ShowStudentList()
        {
            output.PrintTitle(ListAllMenuTitle);
            foreach (Student student in databaseTransfer.AllStudents())
               output.PrintMessage(student.ToString() ?? WarningStudentIsNull);
            output.ConfirmToContinue();
            ShowMainMenu();
        }
        public void ShowQuitProgram()
        {
            output.PrintTitle(QuitMenuTitle);
            output.PrintMessage(SuccessGoodbye);
            output.ConfirmToContinue();
            output.Clear();
        }

        public void ShowInvalidMenuInput()
        {
            output.PrintTitle(InvalidMenuInputTitle);
            output.PrintWarning(WarningUnexpectedInput);
            output.ConfirmToContinue();
            ShowMainMenu();
        }
    }
}
