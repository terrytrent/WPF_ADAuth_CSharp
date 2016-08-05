using System.Collections.Generic;
using System.Net;
using System.DirectoryServices.Protocols;
using System.DirectoryServices.ActiveDirectory;
using DnDns.Enums;
using DnDns.Query;

namespace WPF_ADAuth_CSharp
{
    public class ADAuthentication
    {
        public static List<string> authorizedUsers = new List<string>();
        public static Dictionary<string, string> ldapErrors = new Dictionary<string, string>();

        public ADAuthentication()
        {
            setAuthorizedUsers();
            setLDAPErrors();
        }

        private void setLDAPErrors()
        {
            ldapErrors.Add("52e", "Username or Password incorrect");
            ldapErrors.Add("530", "User Account not permitted to logon during this time");
            ldapErrors.Add("531", "User Account not permitted to logon at this computer");
            ldapErrors.Add("532", "User Account password expired");
            ldapErrors.Add("533", "User Account disabled");
            ldapErrors.Add("701", "User Account expired");
            ldapErrors.Add("773", "User Account password must be reset");
            ldapErrors.Add("775", "User Account Locked");
        }

        private void setAuthorizedUsers()
        {
            authorizedUsers.Add("zzzttrent");
            authorizedUsers.Add("ttrent"); 
        }

        public static void bindToLDAPServer(string ldapServer, string username, string password)
        {
            LdapConnection connection = new LdapConnection(ldapServer);
            NetworkCredential credential = new NetworkCredential(username, password);
            connection.Credential = credential;
            connection.Bind();
        }

        public static string getLdapServer()
        {
            Domain localDomain = Domain.GetCurrentDomain();
            string localDomainAsString = localDomain.ToString();

            DnsQueryRequest request = new DnsQueryRequest();
            DnsQueryResponse Response = request.Resolve($"_ldap.{localDomainAsString}", NsType.A, NsClass.ANY, System.Net.Sockets.ProtocolType.Udp);

            string answer = Response.AuthoritiveNameServers[0].Answer;

            int indexOfDomain = answer.IndexOf($".{localDomainAsString}");
            int lengthToSkip = 20;
            int lengthOfLdapServer = indexOfDomain - lengthToSkip;
            string nameOfLdapServer = $"{answer.Substring(lengthToSkip, lengthOfLdapServer)}.{localDomainAsString}";
            return nameOfLdapServer;
        }

        public static string returnErrorMessage(LdapException lexc)
        {
            string error = lexc.ServerErrorMessage;
            string errorcode = error.Substring(77, 3);
            string errorMessage = ADAuthentication.ldapErrors[errorcode];

            return errorMessage;
        }
    }
}
