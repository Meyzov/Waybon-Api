namespace Waybon.Domain.Entities
{
    public class Permission
    {
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

        private string _name = string.Empty;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
    }
}