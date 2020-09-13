using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParperaEngineeringTest.Models;
using ParperaEngineeringTest.Models.ViewModels;

namespace ParperaEngineeringTest.Controllers
{
    //comment out [Authorize] here to disable authorization
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionContext _context;

        public TransactionsController(TransactionContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        // transactions sorted by datetime
        [HttpGet]
        public async Task<ActionResult<Transaction[]>> GetTransactions()
        {
            var sortedTransactions = _context.Transaction.OrderByDescending(s => s.Datetime);
            return await sortedTransactions.ToArrayAsync();
        }

        // PUT: api/Transactions/ModifyStatus/id
        // modify the status of a transaction
        [HttpPut("ModifyStatus/{id}")]
        public async Task<IActionResult> ModifyStatus(long id, StatusUpdate statusUpdate)
        {
            var transaction = await _context.Transaction.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            try
            {
                transaction.Status = statusUpdate.Status;
            } catch (ArgumentException)
            {
                return BadRequest();
            }

            _context.SetModified(transaction);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
