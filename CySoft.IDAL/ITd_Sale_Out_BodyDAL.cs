using System;
using System.Collections;
using System.Collections.Generic;
using CySoft.IDAL.Base;
using CySoft.Model.Tb;
using CySoft.Model.Td;

namespace CySoft.IDAL
{
    public interface ITd_Sale_Out_BodyDAL : IBaseDAL
    {
        int UpdateBody(Type type, IDictionary param, string database = null);
        int Updatefh(Type type, IDictionary param, string database = null);
        int Updatezf(Type type, IDictionary param, string database = null);
        int UpdatefhCrossingck(Type type, IDictionary param, string database = null);
        int UpdateBatchConfirm(Type type, IDictionary param, string database = null);
    }
}
