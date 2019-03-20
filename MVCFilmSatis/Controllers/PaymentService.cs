using MVCFilmSatis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCFilmSatis.Controllers
{
    public abstract class PaymentService
    {
        public abstract bool MakePayment(IPaymentModel pm);
    }

    public class BankTransferService : PaymentService
    {
        public override bool MakePayment(IPaymentModel pm)
        {
            var info = (BankTransferPayment)pm;
            //Bankaya baglanıp odeme var mı kontrol et
            //odeme varsa true dondur yoksa false dondur.
            return true;
        }
    }
    public class CreditCardService : PaymentService
    {
        public override bool MakePayment(IPaymentModel pm)
        {
            var info = (CreditCardPayment)pm;
            //Kart Bilgileri geçerli mi ve tutar çekiliyor mu ? 
            //odeme alındıysa true dondur
            //odeme basarısız olduysa false dondur
            return true;

        }
    }
}