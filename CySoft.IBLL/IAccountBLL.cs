using System;
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using System.Collections.Generic;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace CySoft.IBLL
{
    public interface IAccountBLL : IBaseBLL
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model">登录信息</param>
        /// <returns>登录结果</returns>
        BaseResult LogOn(UserLogin model);

       /// <summary>
        /// 移动端登录
        /// </summary>
        /// <param name="model">登录信息</param>
        /// <returns>登录结果</returns>
        BaseResult MobileLogin(UserLogin model); 
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model">注册信息</param>
        /// <returns>登录结果</returns>
        BaseResult Register(UserRegister model);

        BaseResult CheckHadRegister(UserRegister model);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model">修改密码相关信息</param>
        /// <returns></returns>
        BaseResult ChangePassword(UserChangePWD model);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model">修改密码相关信息</param>
        /// <returns></returns>
        BaseResult ResetPassword(UserChangePWD model);
        /// <summary>
        /// 更新用户信息(服务接口)
        /// </summary>
        /// <param name="model">用户信息</param>
        /// <returns></returns>
        BaseResult UpdataPart(Tb_User model);
        /// <summary>
        /// 获得用户完整信息
        /// </summary>
        /// <param name="param">用户身份信息</param>
        /// <returns></returns>
        BaseResult GetInfo(Hashtable param);
        /// <summary>
        /// 用户权限的模块
        /// </summary>
        /// <param name="id_roleList">用户权限列表</param>
        /// <returns></returns>
        BaseResult GetRoleFunctions(long[] id_roleList);
        /// <summary>
        /// 取消账号绑定
        /// </summary>
        /// <param name="param">用户身份信息</param>
        /// <returns></returns>
        BaseResult Unbing(Hashtable param);
        /// <summary>
        /// 绑定手机
        /// </summary>
        /// <param name="param">用户身份信息</param>
        /// <returns></returns>
        BaseResult BingPhone(Hashtable param);
        /// <summary>
        /// 绑定邮件
        /// </summary>
        /// <param name="param">用户身份信息</param>
        /// <returns></returns>
        BaseResult BingEmail(Hashtable param);
        /// <summary>
        /// 发送邮件验证
        /// </summary>
        /// <param name="param">用户身份信息</param>
        /// <returns></returns>
        BaseResult VaildEmail(Hashtable param);
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="param">用户身份信息</param>
        /// <returns></returns>
        BaseResult CreateAccount(Hashtable param);
        /// <summary>
        /// 获取业务员登录信息_分页
        /// </summary>
        /// <param name="param">业务员资料、分页范围</param>
        /// <returns></returns>
        PageNavigate GetCheckInfo(Hashtable param);
        /// <summary>
        /// 获取平台下的用户详细资料列表_分页
        /// </summary>
        /// <param name="param">查询条件</param>
        /// <returns></returns>
        PageNavigate QueryUserPage(Hashtable param);

        BaseResult ShopRegister(Hashtable param);

        BaseResult GetAllUser(Hashtable param = null);

        BaseResult ChangeUserPwd(Hashtable param);

        BaseResult ChangeUserVersion(Hashtable param);

        BaseResult CheckServiceForLogin(Hashtable param);

        BaseResult GetUserInfo(Hashtable param);

        BaseResult ChangeCompanyno(Hashtable param);

    }
}
