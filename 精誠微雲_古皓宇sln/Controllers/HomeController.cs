using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using 精誠微雲_古皓宇sln.Models;
using 精誠微雲_古皓宇sln.ViewModels;

namespace 精誠微雲_古皓宇sln.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly dbBankContext _dbBankContext;

		public HomeController(ILogger<HomeController> logger, dbBankContext dbBankContext)
		{
			_logger = logger;
			_dbBankContext = dbBankContext;
		}

		public IActionResult Index()
		{
			var result = from n in _dbBankContext.NetBankUsers
						 join a in _dbBankContext.Accounts
						 on n.UserGuid equals a.OwnerGuid
						 where n.NationId == "K188888886"
						 select new UserViewModel
						 {
							 NationId = n.NationId,
							 BranchId = a.BranchId,
							 AcctSerialId = a.AcctSerialId
						 };
			if(result == null)
			{
				return NotFound();
			}

			return View(result);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}