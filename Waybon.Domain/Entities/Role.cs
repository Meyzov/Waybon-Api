namespace Waybon.Domain.Entities
{
    public class Role
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

        private int? _maxGroupsAllowed;
        public int? MaxGroupsAllowed
        {
            get
            {
                return _maxGroupsAllowed;
            }
            set
            {
                _maxGroupsAllowed = value;
            }
        }
    }
}