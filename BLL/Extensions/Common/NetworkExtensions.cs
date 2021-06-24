using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions.Common
{
    public static class NetworkExtensions
    {
        private static List<ushort> handle = new List<ushort>() { 0, 8, 16, 24 };
        public static uint Ip2Int(this string ip)
        {
            var list = ip.Split('.');
            if (list.Length < 4) return 0;
            uint result = 0;
            for (int i = 0; i < 4; i++)
                result = (result << 8)+uint.Parse(list[i]);
            return result;
        }
        public static string Int2Ip(this int ip)=> string.Join(".", handle.Select(i => (((ip << i) >> 24) & 0xff).ToString()));
        public static Tuple<uint, ushort> ExtractIp(this string ipWithMask)
        {
            var r = ipWithMask.Split('/');
            return new Tuple<uint, ushort>(r[0].Ip2Int(), ushort.Parse(r[1]));
        }
        public static bool EqualIp(this string ip,string compareToIp, ushort mask = 32) =>ip.Ip2Int().EqualIp(compareToIp.Ip2Int(), mask);
        public static bool EqualIp(this uint ip, uint compareToIp, ushort mask = 32) => ip >> mask == compareToIp >> mask;
       
    }
    public static class NetworkFilterExtensions
    {
        /// <summary>
        /// 按ip筛选
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="ipProp">选取ip字段</param>
        /// <param name="ip">匹配到ip</param>
        /// <param name="mask">掩码</param>
        /// <returns></returns>
        public static IQueryable<TSource> FilterByIp<TSource> (this IQueryable<TSource> list, Func<TSource, uint> ipProp, uint ip, ushort mask = 32) => list.Where(i => ipProp(i).EqualIp(ip, mask));
        /// <summary>
        /// 按ip筛选
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="ipProp">选取ip字段</param>
        /// <param name="ip">匹配到ip</param>
        /// <param name="mask">掩码</param>
        /// <returns></returns>
        public static IQueryable<TSource> FilterByIp<TSource> (this IQueryable<TSource> list, Func<TSource, uint> ipProp, string ipWithMask) {
            var t = ipWithMask.ExtractIp();
            return list.FilterByIp(ipProp,t.Item1,t.Item2);
        }
    }
}
