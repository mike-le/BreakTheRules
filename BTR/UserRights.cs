using System.Collections.Generic;


namespace BTR
{
    public class UserRights
    {
        public List<string> ExecAdGroups { get; set; }
        public List<string> ExecSAMAccounts { get; set; }
        public List<string> AdminAdGroups { get; set; }
        public List<string> AdminSAMAccounts { get; set; }
        public List<string> ManagerAdGroups { get; set; }
        public List<string> ManagerSAMAccounts { get; set; }
        public List<string> WhitelistedAdGroups { get; set; }
        public List<string> WhitelistedSAMAccounts { get; set; }
        public List<string> BlacklistedAdGroups { get; set; }
        public List<string> BlacklistedSAMAccounts { get; set; }
        public List<string> BlacklistedOUNames { get; set; }
    }
}
