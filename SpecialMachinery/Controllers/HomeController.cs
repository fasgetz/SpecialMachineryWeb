using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpecialMachinery.Models;

namespace SpecialMachinery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult gallery()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageViewModel vm)
        {
            await Task.Run(() =>
            {
                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("BotMachinery@gmail.com", "Сервис-почта");
                // кому отправляем
                MailAddress to = new MailAddress("markys.bym5@gmail.com");
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = $"Новый заказ от {vm.Email}";
                // текст письма
                m.Body = $"<h2>{vm.Name} ({vm.Email}) просит связаться по телефону {vm.Phone}</h2>";
                m.Body += $"<p>{vm.Description}</p>";
                // письмо представляет код html
                m.IsBodyHtml = true;
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                // логин и пароль
                smtp.Credentials = new NetworkCredential("BotMachinery@gmail.com", "bot12345");
                smtp.EnableSsl = true;
                smtp.Send(m);

                // Второму уотправителю
                // кому отправляем
                to = new MailAddress("Kononovich_max@mail.ru");
                // создаем объект сообщения
                m = new MailMessage(from, to);
                smtp.Send(m);
            });


            return View("SuccessMessage", vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
