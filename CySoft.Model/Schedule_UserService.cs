using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model
{
    public class Schedule_UserService
    {
        /// <summary>
        /// 服务项目id
        /// </summary>
        public long id_service;
        /// <summary>
        /// 超赢用户ID
        /// </summary>
        public long id_master;
        /// <summary>
        /// 服务开始时间
        /// </summary>
        public DateTime? rq_begin;
        /// <summary>
        /// 服务结束时间
        /// </summary>
        public DateTime? rq_end;
        /// <summary>
        /// 数量
        /// </summary>
        public int sl;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? rq_create;
        /// <summary>
        /// 服务名称
        /// </summary>
        public string mc;

        /// <summary>
        /// 服务编码
        /// </summary>
        public string bm;
        /// <summary>
        /// 服务状态
        /// </summary>
        public int flag_stop;
    }
}
