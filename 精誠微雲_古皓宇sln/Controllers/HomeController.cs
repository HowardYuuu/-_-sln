using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
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
			List<string> stringList = new List<string>();
			stringList.Add("ABC");
			stringList.Add("123");
			stringList.Add("456");

			@ViewBag.Ans_1 = stringList.Where(x => x == "123").FirstOrDefault();


			List<Person> people = new List<Person>();

			Person person1 = new Person();
			person1.Name = "張三";
			person1.Gender = "男";
			person1.BirthDate = new DateTime(1998, 07, 14);
			people.Add(person1);

			Person person2 = new Person();
			person2.Name = "李四";
			person2.Gender = "男";
			person2.BirthDate = new DateTime(1996, 06, 13);
			people.Add(person2);

			Person person3 = new Person();
			person3.Name = "倩倩";
			person3.Gender = "女";
			person3.BirthDate = new DateTime(1996, 06, 13);
			people.Add(person3);

			List<Person> personList = new List<Person>();
			foreach (var person in people)
			{
				personList.Add(person);
			}
			@ViewBag.Ans_2 = personList
				.Where(x => x.Gender == "女" && x.BirthDate.Value == new DateTime(1996, 06, 13).Date)
				.Select(x => x.Name).FirstOrDefault();

			ViewBag.Ans_3 = GenerateRandomNumber(50);


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
			if (result == null)
			{
				return NotFound();
			}

			return View(result);
		}

		static string GenerateRandomNumber(int length)
		{
			if (length <= 0)
			{
				string message = "長度需大於0";
				return message;
			}
			using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
			{
				byte[] randomNumber = new byte[length];
				rng.GetBytes(randomNumber);
			
				StringBuilder result = new StringBuilder(length);
				foreach (byte b in randomNumber)
				{
					int number = b % 10;
					result.Append(number);
				}

				return result.ToString();
			}
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