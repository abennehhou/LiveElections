using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Principal;

namespace LiveElections.Extensions
{
    /// <summary>
    /// Too lazy to create roles, etc, for this test project, but it is really baaad, even if it is just a test.
    /// </summary>
    public static class IPrincipalExtensions
    {
        private const string AppSettingsKeyAdministratorsEmails = "AdministratorsEmails";
        private static readonly string[] Separators = { "|" };
        private static IList<string> AdministratorsEmails = ConfigurationManager.AppSettings[AppSettingsKeyAdministratorsEmails].Split(Separators, StringSplitOptions.RemoveEmptyEntries);

        public static bool IsAdmin(this IPrincipal user)
        {
            return user.Identity != null && AdministratorsEmails.Contains(user.Identity.Name);
        }
    }
}
