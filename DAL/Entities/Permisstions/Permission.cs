using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Abp.Linq.Expressions;
using Castle.Core.Internal;
using Newtonsoft.Json;

namespace DAL.Entities.Permisstions
{
    public class Permission
    {

        public string Key { get; set; }
        public string Description { get; set; }

    }
}