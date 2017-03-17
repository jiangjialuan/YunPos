using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Td;
using CySoft.Model.Tz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Utility;
using CySoft.IBLL;

//进货付款
namespace CySoft.BLL.BusinessBLL
{

    public class BusinessBLL : BaseBLL, IBusinessBLL
    {

        public BaseResult ProcedureQuery(Hashtable param)
        {
            BaseResult br = new BaseResult();

            if (param == null ||
              param["json_param"] == null || string.IsNullOrEmpty(param["json_param"].ToString()) ||
               param["sign"] == null || string.IsNullOrEmpty(param["sign"].ToString())
               )
            {
                br.Success = false;
                br.Message.Add("必要参数不可以为空.");
                return br;
            }

            var reqModel = new ProcedureQueryModel();
            try
            {
                reqModel = Utility.JSON.Deserialize<ProcedureQueryModel>(param["json_param"].ToString());
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Add("必要参数 json_param 格式不正确.");
                return br;
            }

            if (reqModel == null || string.IsNullOrEmpty(reqModel.proname))
            {
                br.Success = false;
                br.Message.Add("必要参数不符合要求.");
                return br;
            }

            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("proname", reqModel.proname);
                if (!string.IsNullOrEmpty(reqModel.proparam))
                    ht.Add("proparam", reqModel.proparam);
                var rList = DAL.ProcedureQuery(ht);

                br.Success = true;
                br.Message.Clear();
                br.Message.Add(string.Format("操作成功!"));
                br.Data = rList;
                return br;
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Clear();
                br.Message.Add(string.Format("操作失败 原因:" + ex.Message));
                return br;
            }

        }


        public BaseResult ProcedureOutQuery(Hashtable param)
        {
            BaseResult br = new BaseResult();

            if (param == null ||
              param["json_param"] == null || string.IsNullOrEmpty(param["json_param"].ToString()) ||
               param["sign"] == null || string.IsNullOrEmpty(param["sign"].ToString())
               )
            {
                br.Success = false;
                br.Message.Add("必要参数不可以为空.");
                return br;
            }

            var reqModel = Utility.JSON.Deserialize<ProcedureOutQueryModel>(param["json_param"].ToString());

            if (reqModel == null || string.IsNullOrEmpty(reqModel.proname))
            {
                br.Success = false;
                br.Message.Add("必要参数不符合要求.");
                return br;
            }

            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("proname", reqModel.proname);
                ht.Add("str", reqModel.str ?? "");
                ht.Add("outstr", reqModel.outstr ?? "");

                var rList = DAL.ProcedureOutQuery(ht);
                var outstr = ht["outstr"].ToString();

                ProcedureOutQueryResult result = new ProcedureOutQueryResult() { rList = rList, outstr = outstr };

                br.Success = true;
                br.Message.Clear();
                br.Message.Add(string.Format("操作成功!"));
                br.Data = result;
                return br;
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Clear();
                br.Message.Add(string.Format("操作失败 原因:" + ex.Message));
                return br;
            }

        }


        public BaseResult ShydhQuery(Hashtable param)
        {
            List<Td_Jh_Dd_1_ApiModel> head = new List<Td_Jh_Dd_1_ApiModel>();
            List<Td_Jh_Dd_2_ApiModel> body = new List<Td_Jh_Dd_2_ApiModel>();

            BaseResult br = new BaseResult();
            var dbHead = DAL.QueryList<Td_Jh_Dd_1_QueryModel>(typeof(Td_Jh_Dd_1), param);
            if (dbHead != null && dbHead.Count() > 0)
            {
                Hashtable ht = new Hashtable();
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("id_bill_list", (from r in dbHead select r.id ).ToArray());
                var dbBody = DAL.QueryList<Td_Jh_Dd_2_QueryModel>(typeof(Td_Jh_Dd_2), ht);

                foreach (var item in dbHead)
                {
                    var headItem = item.CloneApiModel();
                    head.Add(headItem);
                }

                foreach (var item in dbBody)
                {
                    var headItem = item.CloneApiModel();
                    body.Add(headItem);
                }
            }

            br.Data = new { head = head, body = body };
            br.Success = true;
            return br;

        }

    }
}
