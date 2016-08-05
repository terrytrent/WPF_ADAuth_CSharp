#AD Authentication Test with WPF using C*#*

Similar to [AD Authentication Test using C*#*](https://github.com/terrytrent/ADAuthTest_CSharp), but using a graphical UI to interface with user.

* Pulls DNS servers from first Ethernet interface
* Uses the DnDNS (https://dndns.codeplex.com/) Library to pull the local (on network) LDAP server from DNS, looking up SRV records
* Confirms user attempting login is authorized, using List for simulated authorization
* If there is an error returned from LDAP it is parsed and the error message returned to the user
