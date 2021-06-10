using DAL.Entities.Common.DataDictionary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public partial class ApplicationDbContext
    {

        /// <summary>
        /// 数据字典 分组数据
        /// </summary>
        /// <param name="builder"></param>
        private void Configuration_Common_Group_Party(ModelBuilder builder)
        {
            var group = builder.Entity<CommonDataGroup>();
            group.HasData(new List<CommonDataGroup>() {
                new CommonDataGroup()
                {
                    Name=partyTypeInParty,
                    Description="政治面貌"
                },
                new CommonDataGroup()
                {
                    Name=PartyConferRecordType,
                    Description="会议记录内容的类型"
                }
            });
        }
        public const string partyTypeInParty = "partyTypeInParty";

        /// <summary>
        /// 数据字典 政治面貌
        /// </summary>
        /// <param name="builder"></param>
        private void Configuration_PartyTypeInParty(ModelBuilder builder)
        {
            #region actions

            var data = builder.Entity<CommonDataDictionary>();

            var actions = new List<CommonDataDictionary>(){
                new CommonDataDictionary()
                {
                    Alias="无",
                    Description="未指定面貌",
                    Color="#cccccc",
                    Key="none",
                    Value=0
                },
                 new CommonDataDictionary()
                {
                    Alias="群众",
                    Description="群众",
                    Color="#cccccc",
                    Key="social",
                    Value=1
                },
                 new CommonDataDictionary()
                {
                    Alias="少先队员",
                    Description="少先队员",
                    Color="#6bbade",
                    Key="child",
                    Value=2
                },
                  new CommonDataDictionary()
                {
                    Alias="共青团员",
                    Description="共青团员",
                    Color="#32e06f",
                    Key="teenager",
                    Value=4
                },
                   new CommonDataDictionary()
                {
                    Alias="入党积极分子",
                    Description="入党积极分子",
                    Color="#e28e8e",
                    Key="intend",
                    Value=8
                },
                    new CommonDataDictionary()
                {
                    Alias="预备党员",
                    Description="预备党员",
                    Color="#f53535",
                    Key="ready",
                    Value=16
                },
                     new CommonDataDictionary()
                {
                    Alias="党员",
                    Description="党员",
                    Color="#e60000",
                    Key="finnally",
                    Value=32
                },
                };
            foreach (var d in actions)
            {
                d.Id = dataId++;
                d.GroupName = partyTypeInParty;
            }
            data.HasData(actions);

            #endregion actions
        }
        public const string PartyConferRecordType = "PartyConferRecordType";
        private void Configuration_PartyConferRecordType(ModelBuilder builder)
        {

            var data = builder.Entity<CommonDataDictionary>();
            var index = 0;
            var actions = new List<CommonDataDictionary>(){
                new CommonDataDictionary(){Alias="一般发言", Description="发言或讲话", },
                 new CommonDataDictionary(){Alias="谈话人",},
                 new CommonDataDictionary(){Alias="被谈话人",},
                 new CommonDataDictionary(){Alias="推荐人",},
                 new CommonDataDictionary(){Alias="介绍人",},
                 new CommonDataDictionary(){Alias="学习笔记",},
                };
            foreach (var d in actions)
            {
                d.Id = dataId++;
                d.Value = index++;
                d.Key = index.ToString();
                d.Color = "#333333";
                d.GroupName = PartyConferRecordType;
            }
            data.HasData(actions);
        }
    }
}
