using ATM_Machine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM_MachineASP.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ATM _atm;




        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string[] TransactionDetails { get; set; }


        [BindProperty]
        public int WithdrawAmount { get; set; }


        public int CurrentTotal { get; set; }
        public List<string> Inventory { get; set; }



        public IndexModel(ILogger<IndexModel> logger, ATM atm)
        {
            _logger = logger;
            _atm = atm;

        }







        public void OnGet()
        {
            //selects a new string with bill info when the get request is recieved
            Inventory = _atm.CurrentInvetory;
            CurrentTotal = _atm.CurrentInventoryValue;
        }


        public IActionResult OnPost()
        {
            if(WithdrawAmount < 1)
            {
                ErrorMessage = "Can't withdraw $0";
                return RedirectToPage();
            }
            else if(_atm.CurrentInventoryValue < WithdrawAmount)
            {
                ErrorMessage = "The ATM does not have that much money at the moment.";
                return RedirectToPage();
            }
            var (success, billsWithdrawn) = _atm.Withdraw(WithdrawAmount);
            if(!success)
            {
                ErrorMessage = $"Could not withdraw ${WithdrawAmount} from the ATM, not enough bills to cover the amount";
            }
            else
            {
                SuccessMessage = $"Withdrew {WithdrawAmount} from the amount, got back:";
                TransactionDetails = billsWithdrawn;

            }
            return RedirectToPage();
        }
    }
}
