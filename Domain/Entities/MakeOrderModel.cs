using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class MakeOrderModel
    {
        public int CardNum { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Fio { get; set; }

        public string DepartmentName { get; set; }

        public int DepartmentNum { get; set; }

        public string PhoneNumber { get; set; }

        public string OrderType { get; set; }

        public bool isPaidByCash { get; set; }

    }
}