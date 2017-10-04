using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BoSi_Reminder
{
    public static class ValidatorExtensions
    {
        //-----------ВИПРАВЛЕНО ПОМИЛКУ-----------//
        //валідація пошти користувача з використанням регулярного виразу
        public static bool IsValidEmailAddress(this string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }
    }
}
