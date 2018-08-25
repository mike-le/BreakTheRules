using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;

namespace BTR
{
    

    public class UNOSUserPrincipalService
    {
        private UserRights _userRights;
        private string DistinguishedName;
        private bool UserInGroup(string GroupName)
        {
            try
            {
                using (PrincipalContext cntxt = new PrincipalContext(ContextType.Domain))
                {
                    using (GroupPrincipal group = GroupPrincipal.FindByIdentity(cntxt, GroupName))
                    {
                        if (group != null)
                        {
                            if (UserInGroup(group))
                                return true;
                        }
                        return false;
                    }
                }
            }
            catch
            {
            }
            return false;

        }
        private bool UserInGroup(GroupPrincipal group)
        {
            foreach (var memberPrincipal in group.Members)
            {
                if (memberPrincipal.GetType() == typeof(GroupPrincipal))
                {
                    if (UserInGroup(memberPrincipal as GroupPrincipal))
                        return true;
                }
                else if (memberPrincipal.GetType() == typeof(UserPrincipal))
                {
                    if (memberPrincipal.SamAccountName == SAMName)
                        return true;
                }
            }
            return false;
        }

        public string DisplayName { get; }
        public string SAMName { get; }
        public string Manager { get;  }
        public List<string> ManagerList { get; private set; }
        public bool Enabled { get;  }
        public bool IsExecutive
        {
            get
            {
                if (_userRights.ExecAdGroups == null && _userRights.ExecSAMAccounts == null)
                    return false;
                if (_userRights.ExecAdGroups != null)
                {
                    foreach (string GroupName in _userRights.ExecAdGroups)
                    {
                        if (UserInGroup(GroupName))
                            return true;
                    }
                }
                if (_userRights.ExecSAMAccounts != null)
                    return _userRights.ExecSAMAccounts.Contains(SAMName);
                return false;
            }
        }
        public bool IsAppAdmin {
            get
            {
                if (_userRights.AdminAdGroups == null && _userRights.AdminSAMAccounts == null)
                    return false;
                if (_userRights.AdminAdGroups != null)
                {
                    foreach (string GroupName in _userRights.AdminAdGroups)
                    {
                        if (UserInGroup(GroupName))
                            return true;
                    }
                }
                if (_userRights.AdminSAMAccounts != null)
                    return _userRights.AdminSAMAccounts.Contains(SAMName);
                return false;
            }
        }
        public bool IsManager {
            get
            {
                if (_userRights.ManagerAdGroups == null && _userRights.ManagerSAMAccounts == null)
                    return false;
                if (_userRights.ManagerSAMAccounts != null)
                    return _userRights.ManagerSAMAccounts.Contains(SAMName);
                if (_userRights.ManagerAdGroups != null)
                {
                    foreach (string managerAdGroup in _userRights.ManagerAdGroups)
                    {
                        if (UserInGroup(managerAdGroup))
                        {
                            ManagerList.Add(managerAdGroup);
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        public bool IsBlackListed {
            get
            {
                // disallow admins or explicitly whitelisted users to be blacklisted
                if (IsAppAdmin || IsExecutive)
                    return false;
                if (_userRights.WhitelistedAdGroups != null)
                {
                    foreach (string wlAdGroup in _userRights.WhitelistedAdGroups)
                    {
                        if (UserInGroup(wlAdGroup))
                            return false;
                    }
                }
                if (_userRights.WhitelistedSAMAccounts != null && _userRights.WhitelistedSAMAccounts.Contains(SAMName))
                    return false;
                if (_userRights.BlacklistedAdGroups == null && _userRights.BlacklistedSAMAccounts == null && 
                    _userRights.BlacklistedOUNames == null)
                    return false;
                if (_userRights.BlacklistedAdGroups != null)
                {
                    foreach (string blAdGroup in _userRights.BlacklistedAdGroups)
                    {
                        if (UserInGroup(blAdGroup))
                            return true;
                    }
                }
                if (_userRights.BlacklistedSAMAccounts != null)
                    return _userRights.BlacklistedSAMAccounts.Contains(SAMName);
                if (_userRights.BlacklistedOUNames != null)
                {
                    foreach (string blacklistItem in _userRights.BlacklistedOUNames)
                    {
                        if (DistinguishedName.Contains(blacklistItem))
                            return true;
                    }
                    return false;
                }
                return false;
            }
        }

       

        public UNOSUserPrincipalService(IHttpContextAccessor accessor, UserRights userRights)
        {
            _userRights = userRights;
            ManagerList = new List<string>();
            var context = accessor.HttpContext;

            using (PrincipalContext cntxt = new PrincipalContext(ContextType.Domain))
            {
                string tempStr = context.User.Identity.Name;
                using (UNOSUserPrincipal user = UNOSUserPrincipal.FindByIdentity(cntxt, tempStr))
                {
                    if (user != null)
                    {
                        DisplayName = user.DisplayName;
                        SAMName = user.SamAccountName;
                        Enabled = user.Enabled ?? false;
                        DistinguishedName = user.DistinguishedName;
                    }

                    else
                    {
                        throw new System.Exception("User not found in domain!");
                    }
                }
            }
        }
        [DirectoryRdnPrefix("CN")]
        [DirectoryObjectClass("Person")]
        private class UNOSUserPrincipal : UserPrincipal
        {
            public UNOSUserPrincipal(PrincipalContext context) : base(context)
            { }

            [DirectoryProperty("manager")]
            public string Manager
            {
                get
                {
                    if (ExtensionGet("manager").Length != 1)
                        return string.Empty;
                    return (string)ExtensionGet("manager")[0];
                }
            }
            [DirectoryProperty("directReports")]
            public List<string> DirectReports
            {
                get
                {
                    List<string> directReportList = new List<string>();
                    if (ExtensionGet("directReports").Length == 0)
                        return directReportList;
                    foreach (var directReport in ExtensionGet("directReports"))
                    {
                        directReportList.Add((string)directReport);
                    }
                    return directReportList;
                }
            }
            // Implement the overloaded search method FindByIdentity.
            public static new UNOSUserPrincipal FindByIdentity(PrincipalContext context, string identityValue)
            {
                return (UNOSUserPrincipal)FindByIdentityWithType(context, typeof(UNOSUserPrincipal), identityValue);
            }

            // Implement the overloaded search method FindByIdentity. 
            public static new UNOSUserPrincipal FindByIdentity(PrincipalContext context, IdentityType identityType, string identityValue)
            {
                return (UNOSUserPrincipal)FindByIdentityWithType(context, typeof(UNOSUserPrincipal), identityType, identityValue);
            }
        }
    }
}