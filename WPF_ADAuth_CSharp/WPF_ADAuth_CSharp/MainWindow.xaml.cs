using System.Windows;
using System.DirectoryServices.Protocols;

namespace WPF_ADAuth_CSharp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ADAuthentication adauth = new ADAuthentication();
        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            bool successfulLogin = AuthenticateUser();
            if(!successfulLogin)
            {
                btn_Submit.Content = "Try Again";
            }
        }

        private bool AuthenticateUser()
        {
            string username = tb_Username.Text;
            string password = pb_Password.Password;

            if(username == ""  | password == "")
            {
                MessageBox.Show("You must enter both a username and a password");
                return false;
            }
            else
            { 
                string ldapServer = ADAuthentication.getLdapServer();

                if (ADAuthentication.authorizedUsers.Contains(username))
                {
                    try
                    {
                        ADAuthentication.bindToLDAPServer(ldapServer, username, password);
                        MessageBox.Show("Login Successful");
                        return true;
                    }
                    catch (LdapException lexc)
                    {
                        string errorMessage = ADAuthentication.returnErrorMessage(lexc);
                        MessageBox.Show(errorMessage);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("User is not authorized");
                    return false;
                }
            }
        }
    }
}
