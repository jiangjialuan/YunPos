using System;
using System.Diagnostics;
using CySoft.Model.Base;

namespace CySoft.Model.Tb
{
    [Serializable]
    [DebuggerDisplay("id = {id}, fatherId = {fatherId}, bm = {bm}, text = {text}, children count = {children.Count}")]
    public class Tb_Khfl_Tree : BaseTreeGuid<Tb_Khfl_Tree>
    {
        public string bm { get; set; }
    }

}