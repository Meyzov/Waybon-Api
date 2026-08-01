namespace Waybon.Domain.Entities
{
    public class RolePermission
    {
        private int _roleId;
        public int RoleId
        {
            get
            {
                return _roleId;
            }

            set
            {
                _roleId = value;
            }
        }

        private int _permissionId;
        public int PermissionId
        {
            get
            {
                return _permissionId;
            }

            set
            {
                _permissionId = value;
            }
        }
    }
}