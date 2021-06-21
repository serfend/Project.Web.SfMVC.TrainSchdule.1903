using Abp.Extensions;
using Abp.Linq.Expressions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Permisstions
{

    public static class ApplicationPermissions
    {
        [Description("用户")]
        public static class User
        {
            public static Permission Item { get; set; }
            [Description("系统信息")]
            public static class Application
            {

                public static Permission Item { get; set; }
                [Description("密码")]
                public static class Password
                {
                    public static Permission Item { get; set; }
                }


                [Description("授权码")]
                public static class AuthKey
                {
                    public static Permission Item { get; set; }
                }

            }

            [Description("基本信息")]
            public static class BaseInfo
            {
                public static Permission Item { get; set; }
                [Description("姓名、身份证号等基础信息")]
                public static class Base
                {
                    public static Permission Item { get; set; }
                }
            }
            [Description("家庭情况信息")]
            public static class SocialInfo
            {
                public static Permission Item { get; set; }
                [Description("联系方式")]
                public static class Contact
                {
                    public static Permission Item { get; set; }
                }

                [Description("居住地")]
                public static class FamilyLocation
                {
                    public static Permission Item { get; set; }
                }
            }
            [Description("单位信息")]
            public static class CompanyInfo
            {

                public static Permission Item { get; set; }
                [Description("职务及级别")]
                public static class Duty
                {
                    public static Permission Item { get; set; }
                }

                [Description("所属单位")]
                public static class Company
                {
                    public static Permission Item { get; set; }
                }
            }
            [Description("自定义信息")]
            public static class CustomeInfo
            {

                public static Permission Item { get; set; }
            }
            [Description("党团信息")]
            public static class PartyInfo
            {
                public static Permission Item { get; set; }
            }
        }

        [Description("单位")]
        public static class Company
        {
            public static Permission Item { get; set; }
            [Description("层级")]
            public static class Tree
            {
                public static Permission Item { get; set; }
            }
            [Description("属性")]
            public static class Detail
            {
                public static Permission Item { get; set; }
            }

        }
        [Description("党团")]
        public static class Party
        {
            public static Permission Item { get; set; }
            [Description("党团单位")]
            public static class Group
            {
                public static Permission Item { get; set; }
            }
            [Description("办会")]
            public static class Confer
            {
                [Description("会议")]
                public static class NormalConfer
                {
                    public static Permission Item { get; set; }
                }
                [Description("操作记录")]
                public static class ConferRecord
                {
                    public static Permission Item { get; set; }
                }
                public static Permission Item { get; set; }
            }
        }
        [Description("申请")]
        public static class Apply
        {
            public static Permission Item { get; set; }
            [Description("请假")]
            public static class ApplyInday
            {
                public static Permission Item { get; set; }
                [Description("召回")]
                public static class Recall
                {
                    public static Permission Item { get; set; }
                }

                [Description("确认时间")]
                public static class ExecuteStatus
                {
                    public static Permission Item { get; set; }
                }
                [Description("属性")]
                public static class Detail
                {
                    public static Permission Item { get; set; }
                }


                [Description("附加信息")]
                public static class AttachInfo
                {
                    public static Permission Item { get; set; }
                }

                [Description("审批流")]
                public static class AuditStream
                {
                    public static Permission Item { get; set; }
                }
            }
            [Description("休假")]
            public static class Vacation
            {
                public static Permission Item { get; set; }

                [Description("召回")]
                public static class Recall
                {
                    public static Permission Item { get; set; }
                }

                [Description("确认时间")]
                public static class ExecuteStatus
                {
                    public static Permission Item { get; set; }
                }
                [Description("属性")]
                public static class Detail
                {
                    public static Permission Item { get; set; }
                }

                [Description("附加信息")]
                public static class AttachInfo
                {
                    public static Permission Item { get; set; }
                }

                [Description("审批流")]
                public static class AuditStream
                {
                    public static Permission Item { get; set; }
                }
            }

        }
        [Description("终端")]
        public static class Client
        {
            [Description("管理")]
            public static class Manage
            {
                [Description("基本信息")]
                public static class Info { public static Permission Item { get; set; } }
                [Description("远程管理")]
                public static class Remote { public static Permission Item { get; set; } }
                public static Permission Item { get; set; }
            }
            [Description("病毒情况")]
            public static class Virus
            {
                [Description("病毒信息")]
                public static class Info
                {
                    public static Permission Item { get; set; }
                }
                [Description("病毒处置")]
                public static class Handle
                {
                    public static Permission Item { get; set; }
                }
                public static Permission Item { get; set; }
            }
            public static Permission Item { get; set; }

        }
        [Description("成绩")]
        public static class Grade
        {
            public static Permission Item { get; set; }
            [Description("科目考核")]
            public static class Subject
            {
                public static Permission Item { get; set; }
                [Description("标准")]
                public static class Standard
                {
                    public static Permission Item { get; set; }
                }

                [Description("考核")]
                public static class Exam
                {
                    public static Permission Item { get; set; }
                }

                [Description("记录")]
                public static class Record
                {
                    public static Permission Item { get; set; }
                }
            }
            [Description("评比")]
            public static class MemberRate
            {
                public static Permission Item { get; set; }
                [Description("属性")]
                public static class Detail
                {
                    public static Permission Item { get; set; }
                }
            }
        }

        [Description("资源")]
        public static class Resources
        {
            public static Permission Item { get; set; }
            [Description("菜单")]
            public static class Menu
            {
                public static Permission Item { get; set; }
            }
            [Description("短网址")]
            public static class ShortUrl
            {
                public static Permission Item { get; set; }
            }
            [Description("文件")]
            public static class File
            {
                public static Permission Item { get; set; }
                [Description("属性")]
                public static class Detail
                {
                    public static Permission Item { get; set; }
                }

                [Description("文件夹")]
                public static class Folder
                {
                    public static Permission Item { get; set; }
                }
            }
        }
        [Description("动态")]
        public static class Activity
        {
            public static Permission Item { get; set; }
            [Description("帖子")]
            public static class Post
            {
                public static Permission Item { get; set; }
            }
            [Description("即时消息")]
            public static class AppMessage
            {
                public static Permission Item { get; set; }
            }
        }

        [Description("权限")]
        public static class Permissions
        {
            public static Permission Item { get; set; }
            [Description("角色")]
            public static class Role
            {
                public static Permission Item { get; set; }
            }
            [Description("权限")]
            public static class PermisionItem
            {
                public static Permission Item { get; set; }

            }
        }
    }


}
