using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CySoft.Model.Tb
{
    [Serializable]
    [DebuggerDisplay("name = {name},name = {name}, itemsCount = {items.Count}")]
    public class Tb_Sp_SpecGroup
    {
        public Tb_Sp_SpecGroup()
        {
        }

        public Tb_Sp_SpecGroup(long id, string name, List<string> items)
        {
            this.id = id;
            this.name = name;
            this.items = items;
        }

        public long id { get; set; }
        public string name { get; set; }
        public List<string> items { get; set; }
    }

    [Serializable]
    [DebuggerDisplay("name = {name},name = {name}, skuListCount = {skuList.Count}, specGroupList = {specGroupList.Count}")]
    public class Tb_Sp_Get:Tb_Sp
    {
        public Tb_Sp_Get()
        {
            this.specGroupList = new List<Tb_Sp_SpecGroup>();
            this.specGroup = new Tb_Sp_Expand_Template();
        }       
        public Tb_Sp_Expand_Template specGroup { get; set; }
        public IList<Tb_Sp_Sku_Query> skuList { get; set; }
        public IList<Tb_Sp_SpecGroup> specGroupList { get; set; }
        public IList<Tb_Sp_Dj_Query> levelPriceList { get; set; }
        public IList<Tb_Sp_Pic> picList { get; set; }
        public IList<Tb_Sp_Cgs_Query> CustomerPriceList { get; set; }
        public IList<Tb_Gys_Tag> gysTagList { get; set; }
    }
}
