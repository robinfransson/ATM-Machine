using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATM_Machine
{
    public class ATM
    {
        public List<Bill> Bills { get; set; }
        public List<string> CurrentInvetory => Bills.GroupBy(bill => bill.Value)
                                                    .OrderByDescending(group => group.Key)
                                                    .Select(grp => $"${grp.Key} x {grp.Count()}")
                                                    .ToList();

        public int CurrentInventoryValue => Bills.Sum(bill => bill.Value);

        public ATM()
        {
            Bills = new()
            {
                new Bill(1000),
                new Bill(1000),
                new Bill(500),
                new Bill(500),
                new Bill(500),
                new Bill(100),
                new Bill(100),
                new Bill(100),
                new Bill(100),
                new Bill(100)
            };
        }



        public (bool success, string[] billsWithdrawn) Withdraw(int amount)
        {


            int remainingToWithdraw = amount;

            //initiate a new list to return for info about what bills was withdrawn from the atm machine
            List<Bill> billsTakenFromATM = new();


            while(billsTakenFromATM.Sum(bill => bill.Value) != amount)
            {

                //how many bills of each kind in the atm machine
                bool hasOneThousandBills = Bills.Any(bill => bill.Value == 1000);
                bool hasFiveHundredBills = Bills.Any(bill => bill.Value == 500);
                bool hasOneHundredBills = Bills.Any(bill => bill.Value == 100);


                //if there are any bills in the atm machine
                bool hasMoreBills = hasOneHundredBills && hasFiveHundredBills && hasOneThousandBills;

                if (remainingToWithdraw >= 1000 && hasOneThousandBills)
                {
                    var bill = Bills.FirstOrDefault(bill => bill.Value == 1000);
                    billsTakenFromATM.Add(bill);
                    Bills.Remove(bill);
                }



                else if (remainingToWithdraw >= 500 && hasFiveHundredBills)
                {
                    var bill = Bills.FirstOrDefault(bill => bill.Value == 500);

                    billsTakenFromATM.Add(bill);
                    Bills.Remove(bill);
                }




                else if (remainingToWithdraw >= 100 && hasOneHundredBills)
                {
                    var bill = Bills.FirstOrDefault(bill => bill.Value == 100);

                    billsTakenFromATM.Add(bill);
                    Bills.Remove(bill);
                }

                //if the ATM machine can't fulfill the withdrawal 
                else if(remainingToWithdraw < 100 || !hasMoreBills)
                {
                    Bills.AddRange(billsTakenFromATM);
                    return (false, null); // success = false, the list of bills withdrawn also null
                }

                remainingToWithdraw = amount - billsTakenFromATM.Sum(bill => bill.Value);
            }
            //the withdrawal was successful and the list of bills withdrawn also gets returned to show what
            //bills the user got
            string[] billsWithdrawn = billsTakenFromATM.GroupBy(bill => bill.Value)
                                                       .OrderByDescending(group => group.Key)
                                                       .Select(group => $"${group.Key} x {group.Count()}")
                                                       .ToArray();
            return (true, billsWithdrawn);
        }

    }



}
