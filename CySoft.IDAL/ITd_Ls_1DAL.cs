using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.IDAL.Base;
using CySoft.Model.Other;

namespace CySoft.IDAL
{
    public interface ITd_Ls_1DAL : IBaseDAL
    {
        object GetHomePageData(Hashtable param);

        List<HomePagePay> QueryHomePagePays(Hashtable param);

        decimal QueryHomePageShopKcj(Hashtable param);

        List<HomePageCxspModel> QueryHomePageCxsp(Hashtable param);

        List<HomePageZxspModel> QueryHomePageZxsp(Hashtable param);

    }
}
