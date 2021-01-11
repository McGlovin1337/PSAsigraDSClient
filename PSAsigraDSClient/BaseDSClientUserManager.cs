using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientUserManager: DSClientCmdlet
    {

        protected abstract void ProcessUserManager(UserManager userManager);

        protected override void DSClientProcessRecord()
        {
            UserManager userManager = DSClientSession.getUserManager();

            ProcessUserManager(userManager);

            userManager.Dispose();
        }

        protected class DSClientUser
        {
            public int UserId { get; private set; }
            public string Name { get; private set; }
            public string From { get; private set; }
            public string FullName { get; private set; }
            public int EffectiveQuota { get; private set; }
            public int MaxOnlineQuota { get; private set; }
            public int UsedQuota { get; private set; }

            public DSClientUser(dsclient_user_info user)
            {
                UserId = user.id;
                Name = user.name;
                From = user.from;
                FullName = user.full_name;
                EffectiveQuota = user.quota_effective;
                MaxOnlineQuota = user.quota_max_online;
                UsedQuota = user.quota_used;
            }
        }

        protected class DSClientUserGroup
        {
            public int GroupId { get; private set; }
            public string Name { get; private set; }
            public string From { get; private set; }
            public string Description { get; private set; }
            public int EffectiveQuota { get; private set; }
            public int MaxOnlineQuota { get; private set; }
            public int UsedQuota { get; private set; }

            public DSClientUserGroup(user_group_info group)
            {
                GroupId = group.id;
                Name = group.group;
                From = group.from;
                Description = group.description;
                EffectiveQuota = group.quota_effective;
                MaxOnlineQuota = group.quota_max_online;
                UsedQuota = group.quota_used;
            }
        }

        protected class DSClientUserGroupRole
        {
            public int RoleId { get; private set; }
            public string Name { get; private set; }
            public string From { get; private set; }
            public string Role { get; private set; }
            public string Type { get; private set; }

            public DSClientUserGroupRole(user_group_role role)
            {
                RoleId = role.role_id;
                Name = role.user_name;
                From = role.user_from;
                Role = EnumToString(role.user_role);
                Type = EnumToString(role.role_type);
            }
        }

        protected class DSClientCurrentLoginRole
        {
            public string Role { get; private set; }

            public DSClientCurrentLoginRole(EUserGroupRole role)
            {
                Role = EnumToString(role);
            }
        }
    }
}