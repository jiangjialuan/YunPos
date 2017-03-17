using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Utility.Pay
{
  public  class PaymentParms
  {
      public string name { get; set;}

      public string orderId { get; set; }

      public string sellerEmail { get; set; }

      public string partner { get; set; }

      public string key { get; set; }

      public decimal amount { get; set; }

      public string subject { get; set; }

      public string body { get; set; }

      public string buyerEmail { get; set; }

      public DateTime date { get; set; }

      public string showUrl { get; set; }

      public string returnUrl { get; set; }

      public string notifyUrl { get; set; }

      public string attach { get; set; }
  }
}
