using System;

namespace API.Dtos
{
    public class MenuConfigDto
    {
        public int Id { get; set; }
        public int SubMenuId { get; set; }
        public string SubMenuName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string Url { get; set; }
        public int MenuHeadId { get; set; }
        public string MenuHeadName { get; set; }
    }
    }
