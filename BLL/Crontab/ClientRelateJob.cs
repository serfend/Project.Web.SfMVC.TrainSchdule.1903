﻿using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.ClientDevice;
using DAL.Entities.UserInfo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrainSchdule.Crontab;

namespace BLL.Crontab
{
    public class ClientRelateJob : ICrontabJob
    {
        private readonly ApplicationDbContext context;
        private readonly IUserActionServices userActionServices;

        public ClientRelateJob(ApplicationDbContext context, IUserActionServices userActionServices)
        {
            this.context = context;
            this.userActionServices = userActionServices;
        }
        public void RelateClientCompany()
        {
            var clients = context.ClientsDb
                .Where(c => c.CompanyCode == null)
                .Where(c => c.Ip != null)
                .Where(c => c.Ip != string.Empty)
                .ToList();
            RelateProperty(clients, (prev, refererTo) =>
            {
                prev.CompanyCode = refererTo.CompanyCode;
                userActionServices.Log(UserOperation.UserInfoUpdate, prev.Ip, $"依据[{refererTo.Ip}]绑定到:[{prev.CompanyCode}]", true);
            });
        }
        public void RelateClientMajor()
        {
            var clients = context.ClientsDb
                .Where(c => c.OwnerId == null)
                .Where(c => c.Ip != null)
                .Where(c => c.Ip != string.Empty)
                .ToList();
            var companies = clients
                .Where(c => c.CompanyCode != null)
                .Select(c => c.CompanyCode)
                .Distinct()
                .Select(c =>
                {
                    var managers = context.AppUsersDb
                        .Where(u => u.CompanyInfo.CompanyCode == c)
                        .Where(u => u.CompanyInfo.Duties.IsMajorManager);
                    return new KeyValuePair<string, User>(c, managers.FirstOrDefault());
                }).ToDictionary(c => c.Key);
            foreach (var c in clients)
            {
                var nacUserCheck = CheckNacUser(c.FutherInfo, out var userNac);
                if (nacUserCheck != null)
                    c.Owner = nacUserCheck;
                else if (c.CompanyCode != null)
                    c.Owner = companies[c.CompanyCode].Value;
                if (c.Owner != null)
                    userActionServices.Log(UserOperation.UserInfoUpdate, c.Ip, $"{userNac}[{c.CompanyCode}]绑定到:{c.Owner.BaseInfo.RealName}", true);
            }
        }
        private User CheckNacUser(string nacInfo, out string nacUser)
        {
            nacUser = null;
            if (nacInfo == null || nacInfo.Length < 10) return null;
            var item = JsonConvert.DeserializeObject(nacInfo) as JObject;
            if (item == null) return null;
            var userid = item.GetValue("nac_user").ToString();
            nacUser = userid;
            userid = Regex.Replace(userid, @"\d", "");
            var userGroups = userid.Split('_');
            var db = context.AppUserBaseInfos;
            var max_length = userGroups.Max(i => i.Length);
            var dict = new Dictionary<int, List<string>>();
            foreach (var g in userGroups)
            {
                var len = g.Length;
                if (!dict.ContainsKey(len))
                    dict.Add(len, new List<string>());
                dict[len].Add(g);
            }
            for (var i = max_length; i >= 4; i--)
            {
                if (!dict.ContainsKey(i)) continue;
                if (!dict.ContainsKey(i - 1))
                    dict.Add(i - 1, new List<string>());
                foreach (var g in dict[i])
                {
                    var user = db.FirstOrDefault(i => i.PinYin.Contains(g));
                    if (user != null) return context.AppUsersDb.FirstOrDefault(u => u.BaseInfoId == user.Id);
                    dict[i - 1].Add(g.Substring(1));
                };
            }
            return null;
        }
        private void RelateProperty(IEnumerable<Client> clients, Action<Client, Client> callback)
        {
            foreach (var c in clients)
            {
                var ips = c.Ip.Split('.');
                if (ips.Length < 4) continue;
                var ipRegion = string.Join('.', ips.Take(3));
                var ipD = int.Parse(ips[3]);
                var sameRegion = context.ClientsDb
                    .Where(client => client.CompanyCode != null)
                    .Where(client => client.Ip != null)
                    .Where(client => client.Ip.StartsWith(ipRegion) )
                    .ToList();
                var nearest = sameRegion.OrderBy(x => Math.Abs(ipD - int.Parse(x.Ip.Split('.')[3]))).FirstOrDefault();
                if (nearest == null) continue;
                callback.Invoke(c, nearest);
            }
        }
        public void Run()
        {
            RelateClientCompany();
            RelateClientMajor();
            context.SaveChanges();
        }
    }
}
