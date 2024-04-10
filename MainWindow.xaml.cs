/*
* FILE				: MainWindow.xaml.cs
* PROJECT			: PROG 2121 - Windows Programming Assignment 02
* PROGRAMMERS		:
*   Cody Glanville ID: 8864645
*   Minchul Hwang  ID: 8818858
* FIRST VERSION		: September 23, 2023
* DESCRIPTION		:
*	This file is used in order to create many functionalities that will be utilized in the user interface of our assignment in
*	which we are recreating a Notepad application. As such, methods for saving text, opening files, exiting the application safely, 
*	and many more are written in order to create a fully working notepad replica program. 
*/

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;                    // Added to help in closing the program through the "X" Window button (CloseEventArgs)
using System.Diagnostics.Eventing.Reader;


namespace WP_A02
{
    /*
    * PARTIAL CLASS     : MainWindow
    * DESCRIPTION	    :
    *	This class houses all of the methods used in order to recreate a fully functioning Notepad replica program.
    *	As such, this class will hold all of the methods for our Main Window's behaviour in our program.
    */
    public partial class MainWindow : Window
    {

        /*
        *	CONSTRUCTOR     : MainWindow()
        *	DESCRIPTION		:
        *		This constructor instantiates an object of class type MainWindow which will be used as a template in creating our
        *		main window for our Notepad replica program. As such, it will help in initiating our keybinds for us as well in order 
        *		to allow shortcuts to be used for our "File" menu items.
        *	PARAMETERS		:
        *		void        :    Void is used as there are no paramters for this constructor
        *	RETURNS			:
        *		void	    :	 There are no return values for this constructor
        */
        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, New_Executed));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Executed));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.SaveAs, SaveAs_Executed));
        }

        string userAlert = "Do you want to save the changes to ";

        string alertMessageHeader = "Notebook Replica";

        bool workSpaceDirty = false;                // Determines if there are unsaved changes in the work area that need to possibly be saved

        bool saveValid = false;                     // Determines the status of the file (saved / unsaved)

        private string savedFilePath = "";          // A variable that contains the saved files path

        string allText = "";                        // Contains the all the text from the current file as a string

        string newTitle = "Untitled";               // Holds the title of the file to be used for the Main Windows title

        string newWindowTitle = "Untitled";         // Used for setting a new windows' title to the default of "Untitled" (With "New" dropdown)

        string fileSavePrompt = "";                 


        /*
        *	METHOD  		: New_Executed(object sender, ExecutedRoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method is executed when the shortcut key for New is pressed. When you press the shortcut Ctrl + o, New_Click
        *		is called and the appropriate action is performed.
        *	PARAMETERS		:
        *		object sender			        :	 This is a reference to the button click event that triggered the method
        *		ExecutedRoutedEventArgs e       :    Execute the data related to the event of clicking the "New" menu button in shortcut
        *	RETURNS			:
        *		void			                :	 There is no return value for this method
        */
        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            New_Click(sender, e);
        }

        /*
        *	METHOD  		: Open_Executed(object sender, ExecutedRoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method is executed when the shortcut key for Open is pressed. When you press the shortcut Ctrl + O,
        *		Open_Click is called and the appropriate action is performed.
        *	PARAMETERS		:
        *		object sender			        :	 This is a reference to the button click event that triggered the method
        *		ExecutedRoutedEventArgs e       :    Execute the data related to the event of clicking the "Open" menu button in shortcut
        *	RETURNS			:
        *		void			                :	 There is no return value for this method
        */
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Open_Click(sender, e);
        }

        /*
        *	METHOD  		: Save_Executed(object sender, ExecutedRoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method is executed when the shortcut key for Save is pressed. When you press the shortcut Ctrl + S, Save_Click is called and
        *		the appropriate action is performed.
        *	PARAMETERS		:
        *		object sender			        :	 This is a reference to the button click event that triggered the method
        *		ExecutedRoutedEventArgs e       :    Execute the data related to the event of clicking the "Save" menu button in shortcut
        *	RETURNS			:
        *		void			                :	 There is no return value for this method
        */
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save_Click(sender, e);
        }

        /*
        *	METHOD  		: SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method is executed when the shortcut key for Save As is pressed. When you press the shortcut Ctrl + Shift + S,
        *		SaveAs_Click is called and the appropriate action is performed.
        *	PARAMETERS		:
        *		object sender			        :	 This is a reference to the button click event that triggered the method
        *		ExecutedRoutedEventArgs e       :    Execute the data related to the event of clicking the "SaveAS" menu button in shortcut
        *	RETURNS			:
        *		void			                :	 There is no return value for this method
        */
        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAs_Click(sender, e);
        }

        /*
        *	METHOD  		: Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method is executed when the shortcut key for Exit is pressed. When you press the shortcut Ctrl + X,
        *		Exit_Click is called and the appropriate action is performed.
        *	PARAMETERS		:
        *		object sender			        :	 This is a reference to the button click event that triggered the method
        *		ExecutedRoutedEventArgs e       :    Execute the data related to the event of clicking the "Exit" menu button in shortcut
        *	RETURNS			:
        *		void			                :	 There is no return value for this method
        */
        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exit_Click(sender, e);
        }

        /*
        *	METHOD  		: New_Click(object sender, RoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method will be utilized when a user clicks on the "New" option in the dropdown menu created for "File". As such, it will open up a fresh
        *		untitled document for the user to be able to work on. If there are unsaved changes to the document the user is working on, they will be prompted to
        *		save their changes if they wish and after this event, the new document will open for the user. 
        *	PARAMETERS		:
        *		object sender			:	 This is a reference to the button click event that triggered the method
        *		RoutedEventArgs e       :    Contains the data related to the event of clicking the "New" menu button
        *	RETURNS			:
        *		void			        :	 There is no return value for this method
        */
        private void New_Click(object sender, RoutedEventArgs e)
        {

            if (saveValid==true && workSpaceDirty == false)
            {
                ResetVars();
            }
            else if (saveValid == true && workSpaceDirty == true)
            {
                NewSavingInstances(sender, e);
            }
            else if (saveValid == false && workSpaceDirty == false)
            {
                ResetVars();
            }
            else
            {
                NewSavingInstances(sender, e);
            }
        }

        /*
        *	METHOD  		: ResetVars()
        *	DESCRIPTION		:
        *		This method is used in order to reset the variables used in the program for when a new window is opened
        *		for the user. As such, all the variables are defaulted and technically refreshed in order to be prepared
        *		for any changes to be made in the new window. This helps create a fresh work area for the user.
        *	PARAMETERS		:
        *		void               :    There are no parameters for this method
        *	RETURNS			:
        *		void			   :	There is no return value for this method
        */
        private void ResetVars()
        {
            MainWritingArea.Text = "";
            allText = "";
            saveValid = false;
            newTitle = newWindowTitle;
            workSpaceDirty = false;
            TheWindow.Title = newWindowTitle;
            fileSavePrompt = "";
        }

        /*
        *	METHOD  		: SetFileSavePrompt()
        *	DESCRIPTION		:
        *		This method is used in order help set the prompt for message boxes that appear when changes need to be saved for
        *		a new file to open, the window needs to be closed, or a new window needs to be opened. This will help properly display
        *		the full file path the user to notify them of the file that may lose changes if they do not take the proper actions.
        *	PARAMETERS		:
        *		void               :     There are no parameters for this method
        *	RETURNS			:
        *		void			   :	 There is no return value for this method
        */
        private void SetFileSavePrompt()
        {
            if (TheWindow.Title == "*Untitled")
            {
                fileSavePrompt = userAlert + newWindowTitle + "?";
            }
            else
            {
                fileSavePrompt = userAlert + savedFilePath + "?";
            }
        }

        /*
        *	METHOD  		: NewSavingInstances()
        *	DESCRIPTION		:
        *		This method is used to help in using the "New" menu item. The method will prompt the user to show them their file that 
        *		may need to be saved. After the user enters their option, they will either be prompted to enter a file name for their work or
        *		their work will automatically be saved (if it was already saved to a previous file). Otherwise if they select "No", the work 
        *		will not try to be saved. In each instance of both "Yes" and "No" being chosen, the ResetVars() method will be called to 
        *		refresh the variables used in the program.
        *	PARAMETERS		:
        *		object sender          :     This is a reference to the button click event that triggered the method
        *		RoutedEventArgs e      :     Contains the passed data related to the event of clicking "New"
        *	RETURNS			:
        *		void			       :	 There is no return value for this method
        */
        private void NewSavingInstances(object sender, RoutedEventArgs e)
        {
            SetFileSavePrompt();

            MessageBoxResult newResult = MessageBox.Show(fileSavePrompt, alertMessageHeader, MessageBoxButton.YesNoCancel);

            if (newResult == MessageBoxResult.Yes)
            {
                Save_Click(sender, e);

                if (saveValid == true) // Checks if cancel was hit in the save menu showing files
                {
                    ResetVars();
                }
            }
            else if (newResult == MessageBoxResult.No)
            {
                ResetVars();
            }
        }

        /*
        *	METHOD  		: Open_Click(object sender, RoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method will be utilized when a user clicks on the "Open" option in the dropdown menu created for "File". Ultimately, this method allows
        *		users to open up specific files to be implemented into the working text area of the notebook application. Users will be prompted to save their work
        *		if there are any unsaved changes made to their current document.
        *	PARAMETERS		:
        *		object sender			:	 This is a reference to the button click event that triggered the method
        *		RoutedEventArgs e       :    Contains the data related to the event of clicking the "Open" menu button
        *	RETURNS			:
        *		void			        :	 There is no return value for this method
        */
        private void Open_Click(object sender, RoutedEventArgs e)             // User clicks on the "Open" button from the file dropdown menu
        {
            SetFileSavePrompt();

            if (workSpaceDirty == true && TheWindow.Title == "*Untitled")         // The file has not been saved before (file name not unique so SaveAs needed)
            {
                // Prompt user to save any changes

                MessageBoxResult result = MessageBox.Show(fileSavePrompt, alertMessageHeader, MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.No)                               // User does not wish to save content, so skip and immediately go to open
                {
                    FileOpen(sender, e);
                }

                else if (result == MessageBoxResult.Yes)     // User wants to save so first save, then open new file
                {
                    SaveAs_Click(sender, e);              // Call saveAs method

                    if (saveValid == true)                   // Open will be called only if a save is valid and the user did not hit "cancel" in the save area
                    {
                        FileOpen(sender, e);
                    }
                }
            }
            else if (workSpaceDirty == true && TheWindow.Title != "Untitled")         // File already has name, so only need to "save" any changes
            {
                MessageBoxResult userOutcome = MessageBox.Show(fileSavePrompt, alertMessageHeader, MessageBoxButton.YesNoCancel);

                if (userOutcome == MessageBoxResult.No)         // User does not wish to save
                {
                    FileOpen(sender, e);
                }

                else if (userOutcome == MessageBoxResult.Yes)   // User wishes to save any changes made first
                {
                    Save_Click(sender, e);                   // Call save method

                    FileOpen(sender, e);                        // Call the FileOpen method to open a file 
                }
            }
            else        // No file has been created and the workspace is not dirty, so just open a file
            {
                FileOpen(sender, e);
            }
        }

        /*
        *	METHOD  		: FileOpen(object sender, RoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method will be used in order to recieve the text from a specific file and save it to a string variable to also write
        *		it to the main textbox that serves as the working area for this notepad application. This specific method is used multiple times
        *		in the Open_Click() method in order to successfully open files for the user. Other variables are also set in order to change the 
        *		title of the window and reset the status of any changes made to the working areas text.
        *	PARAMETERS		:
        *		object sender			:	 This is a reference to the button click event that triggered the method
        *		RoutedEventArgs e       :    Contains the data related to the event of clicking the "Open" menu button
        *	RETURNS			:
        *		void			        :	 There is no return value for this method
        */
        private void FileOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)                                     // Open file if user chooses to
            {
                string filepath = openFileDialog.FileName;                               // Gets the file path of the opened file
                savedFilePath = filepath;                                                // Save to the private string the file path to be utilized for saving

                allText = File.ReadAllText(filepath);

                string text = File.ReadAllText(openFileDialog.FileName);                 // Reads the text from the file 
                string fileTitle = System.IO.Path.GetFileNameWithoutExtension(filepath); // Gets the name of the final directory
                newTitle = fileTitle;

                TheWindow.Title = fileTitle;                             // Set the title of the window to be the name of the opened file
                MainWritingArea.Text = text;                             // Set the text in the work area to be the same as the text in the opened file
                workSpaceDirty = false;                                  // Reset the workspace to not be "dirty"

                saveValid = true;                                        // Set save valid to true 
            }
        }

        /*
        *	METHOD  		: SaveFile()
        *	DESCRIPTION		:
        *		This method will be utilized in order to save the contents of the main working text area to a file. This method in particular compared
        *		to the other saving methods will be used in order to allow a user to pick a specific directory to save the file under and also a 
        *		customized name of their choosing. This method will be reused in the other saving methods utilized by the "File" dropdown menu.
        *	PARAMETERS		:
        *		void            :    No parameters are used for this method
        *	RETURNS			:
        *		void			:	 There is no return value for this methods
        */
        private void SaveFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt;";
            string fileContents = MainWritingArea.Text;

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;

                try
                {
                    File.WriteAllText(fileName, fileContents);                   // Change saveFile into fileContents to save current content in work area

                    workSpaceDirty = false;                                                        // Workspace is saved therefore its NOT DIRTY

                    savedFilePath = System.IO.Path.GetFullPath(fileName);                          // Take the path from where you save it and save it.

                    allText = File.ReadAllText(fileName);

                    newTitle = System.IO.Path.GetFileNameWithoutExtension(fileName);        // Get the name of the file you are saving and save it.

                    TheWindow.Title = newTitle;                                                    // Give the title to window

                    saveValid = true;
                }
                catch (Exception ex)        // Flag should be set here if the cancel has been taken
                {
                    MessageBox.Show("File has not been saved...");
                }
            }
        }


        /*
        *	METHOD  		: MenuItem_Click(object sender, RoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method allows the main window to utilize the menu buttons created for File, Edit, and Help upon the action of clicking the items.
        *		Without this, the program will not be able to utilize its main methods, let alone run.
        *	PARAMETERS		:
        *		object sender			:	 This is a reference to the button click event that triggered the method
        *		RoutedEventArgs e       :    Contains the data related to the event of clicking a menu item
        *	RETURNS			:
        * 		void			        :	 There is no return value for this method
        */
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
        }


        /*
        *	METHOD  		: Save_Click(object sender, RoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method will be utilized when a user clicks on the "Save" option of the "File" dropdown menu. For this specific method, the user 
        *		will have already had a previously saved file. As such, upon the methods activation, the contents inside of the main working text area
        *		will be saved and written to the working files contents. 
        *	PARAMETERS		:
        *		object sender			:	 This is a reference to the button click event that triggered the method
        *		RoutedEventArgs e       :    Contains the data related to the event of clicking the "Save" menu button
        *	RETURNS			:
        *		void			        :	 There is no return value for this method
        */
        private void Save_Click(object sender, RoutedEventArgs e)        // User clicks on the "Save" button from the file menu
        {
            if (saveValid == false)
            {
                SaveFile();
            }
            else
            {
                try
                {
                    string fileContents = MainWritingArea.Text;                     // RECIEVES TEXT FROM MAIN WORKING AREA AS A STRING

                    File.WriteAllText(savedFilePath, fileContents);         // Change saveFile into fileContents to save current content in work area

                    workSpaceDirty = false;

                    saveValid = true;

                    if (TheWindow.Title.Contains('*') == true && allText != MainWritingArea.Text)
                    {
                        TheWindow.Title = newTitle;
                        allText = MainWritingArea.Text;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot be saved");
                }
            }
        }


        /*
        *	METHOD  		: SaveAs_Click(object sender, RoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method will be utilized when a user clicks on the "Save As" menu item from the "File" dropdown menu. This method will then
        *		call upon the SaveFile() method in order to allow the user to save the contents of the file to a specific directory and with a custom 
        *		name if they wish to do so. Otherwise, the user will be able to use "no" or "cancel" in order to not activate the functionality of these 
        *		methods.
        *	PARAMETERS		:
        *		object sender			:	 This is a reference to the button click event that triggered the method
        *		RoutedEventArgs e       :    Contains the data related to the event of clicking the "Save As" menu button
        *	RETURNS			:
        *		void			        :	 There is no return value for this method
        */
        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }


        /*
        *	METHOD  		: TextBox_TextChanged(object sender, TextChangedEventArgs e)
        *	DESCRIPTION		:
        *		This method will be utilized to determine if the text area has been modified. As such, the method will act to help count
        *		the characters in the work area and also determine if the title needs to be changed by adding a "*" to imply that there 
        *		are unsaved changes to the workarea the user is currently utilizing. 
        *	PARAMETERS		:
        *		object sender			     :	  This is a reference to the textbox object
        *		TextChangedEventArgs e       :    Contains the data related to the event of the textbox's content changing
        *	RETURNS			:
        *		void			             :	  There is no return value for this method
        */
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) // Determines if the textbox workspace is changed
        {
            CharacterCount();                                                   // Calls the CharacterCount method in order to continuously check the characters used in the textbox

            this.workSpaceDirty = true;  // If workspace has unresolved changes, the workspace is classified as being dirty

            if (TheWindow.Title.Contains('*') == false && allText != MainWritingArea.Text)
            {
                TheWindow.Title = "*" + TheWindow.Title;
            }
            else
            {
                if (allText == MainWritingArea.Text)
                {
                    TheWindow.Title = newTitle;
                    saveValid = true;
                    workSpaceDirty = false;
                }
            }
        }

        /*
        *	METHOD  		: CharacterCount()
        *	DESCRIPTION		:
        *		This method will be utilized in order to count the amount of characters in the textbox that serves as the main working area for the user.
        *		As such, the amount of characters will be determined by taking the entire length of the text in the working area and convert it into a string
        *		to be displayed through the status bar's content area. 
        *	PARAMETERS		:
        *		void            :   Void is used as there are no parameters for this method
        *	RETURNS			:
        *		void			:	There is no return value for this method
        */
        private void CharacterCount()                                           // Used in order to count the characters in the users' workspace
        {
            int i = MainWritingArea.Text.Length;                                // Get the length of the characters
            string chars = i.ToString();                                        // Convert the total length to a string
            CharacterHolder.Content = "Characters: " + chars;                   // Place it in the CharacterHolder Labels content
        }

        /*
        *	METHOD  		: CloseProgram(Object sender, CancelEventArgs e)
        *	DESCRIPTION		:
        *		This method will be used in order to safely close the program. Upon the user trying to force shut down the program using the "X" button
        *		in the window, the method will determine if the workspace has any changes that need to be saved. Depending on if the user wants to save
        *		the changes or not, the program will either save the changes and close, or just close. 
        *	PARAMETERS		:
        * 		Object sender            :   This is a reference to the object that created the instance of the event
        *		CancelEventArgs e        :   Contains the data related to the clsoing event that may occur
        *	RETURNS			:
        *		void			         :	 There is no return value for this method
        */
        private void CloseProgram(Object sender, CancelEventArgs e)                     // Stops the user from closing the program is their workspace has changed text (forced exit through the window)
        {
            SetFileSavePrompt();

            if (this.workSpaceDirty)
            {
                // Program must ask user to save the text before exit it.

                MessageBoxResult outcome = MessageBox.Show(fileSavePrompt, alertMessageHeader, MessageBoxButton.YesNoCancel);

                if (outcome == MessageBoxResult.Yes)                                // Activated when the user chooses yes to save
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();

                    string fileContents = MainWritingArea.Text;                     // RECIEVES TEXT FROM MAIN WORKING AREA AS A STRING

                    // Textbox Implementation Here
                    saveFileDialog.Filter = "Text Files (*.txt)|*.txt;";

                    RoutedEventArgs secondParam = new RoutedEventArgs();            

                    if (saveValid == false)                                         // The text have not been saved ever.
                    {
                        SaveFile(); 
                    }
                    else
                    {
                        Save_Click(sender, secondParam);
                    }

                    if (saveValid == false)
                    {
                        e.Cancel = true;
                    }
                    else if (saveValid == true)
                    {
                        e.Cancel = false;
                    }
                }
                else if (outcome == MessageBoxResult.No)     // User did not care about the unfinished work, therefore the program can be closed
                {
                    e.Cancel = false;
                }
                else if (outcome == MessageBoxResult.Cancel)
                {       // When user want to cancel it
                    e.Cancel = true;
                }
            }
            // Microsofts website https://learn.microsoft.com/en-us/dotnet/api/system.windows.window.closing?view=windowsdesktop-7.0
        }

        /*
        *	METHOD  		: Exit_Click(object sender, RoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method calls upon the Close() method which will ultimately call the CloseProgram() method. This will act to 
        *		possible save any changes before closing the program and exit safely much like the "X" button in the window. However,
        *		this method is accessed through the "Exit" button in the "File" dropdown menu.
        *	PARAMETERS		:
        *		object sender            :   This is a reference to the button click event that triggers the method
        *		RoutedEventRgs e         :   Contains the data relating to the event that triggered the button click for Exit_Click()
        *	RETURNS			:
        *		void			         :	 There is no return value for this method
        */
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /*
        *	METHOD  		: About_Click(object sender, RoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method will be utilized in order create a pop-up used to explain the application and its functionlities similar to how
        *		a pop-up works for the Notepad application this assignment is based on.
        *	PARAMETERS		:
        *		object sender            :   This is a reference to the button click event that triggers the method
        *		RoutedEventRgs e         :   Contains the data relating to the event that triggered the button click for About_Click()
        *	RETURNS			:
        *		void			         :	 There is no return value for this method
        */
        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutModal aboutModal = new AboutModal();
            aboutModal.ShowDialog();
        }
    }
}