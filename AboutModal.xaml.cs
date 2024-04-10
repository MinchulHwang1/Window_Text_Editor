using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WP_A02
{
    /*
    * PARTIAL CLASS     : AboutModal
    * DESCRIPTION	    :
    *	This class is a class that creates a modal window..
    *	This class is called when About is clicked in Help and provides information about this notepad to the user.
    */
    public partial class AboutModal : Window
    {

        /*
        *	CONSTRUCTOR     : AboutModal()
        *	DESCRIPTION		:
        *	    This constructor is executed when the user clicks About in a Windows window and instantiates the window of the modal associated with it.
        *	PARAMETERS		:
        *		void        :    Void is used as there are no paramters for this constructor
        *	RETURNS			:
        *		void	    :	 There are no return values for this constructor
        */
        public AboutModal()
        {
            InitializeComponent();
        }

        /*
        *	METHOD  		: About_Click(object sender, RoutedEventArgs e)
        *	DESCRIPTION		:
        *		This method is executed when the user presses the close button in the modal window.
        *       When executed, the modal window is closed.
        *	PARAMETERS		:
        *		object sender		    :	 This is a reference to the button click event that triggered the method
        *		RoutedEventArgs e       :    Contains the passed data related to the event of clicking "Close"
        *	RETURNS			:
        *		void		:	 There is no return value for this method
        */
        private void About_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /*
        *	METHOD  		: RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        *	DESCRIPTION		:
        *		This method is responsible for outputting the content entered into RichBoxText.
        *       This RichBoxText box is read-only and its contents cannot be modified.
        *	PARAMETERS		:
        *		object sender		         :	   This is a reference to the button click event that triggered the method
        *		TextChangedEventArgs e       :     Contains the data written down in RichTextBox
        *	RETURNS			:
        *		void		:	 There is no return value for this method
        */
        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
