using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParperaEngineeringTest.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public DateTime Datetime { get; set; }
        public string Descripton { get; set; }
        public double Amount { get; set; }
        public string Status
        {
            get { return StatusEnum.ToString(); }
            set {
                try
                {
                    StatusEnum = value.ParseEnum<TransactionStatus>();
                } catch(ArgumentException e)
                {
                    throw new ArgumentException("The status is invalid", e);
                }
            }
        }
        [NotMapped]
        private TransactionStatus StatusEnum { get; set; }
    }
}
