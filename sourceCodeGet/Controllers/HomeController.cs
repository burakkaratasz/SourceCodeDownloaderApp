using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace sourceCodeGet.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DownloadSourceCode(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                ModelState.AddModelError("url", "URL adresi boş olamaz.");
                return View("Index");
            }

            string domainName = new Uri(url).Host.Replace("www.", ""); // Alan adını al
            string fileName = $"{domainName}_{DateTime.Now:yyyyMMdd_HHmmss}.txt"; // Dosya adını oluştur
            string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName); // Kayıt yolu oluştur

            using (WebClient client = new WebClient())
            {
                try
                {
                    string htmlCode = await client.DownloadStringTaskAsync(url);
                    await System.IO.File.WriteAllTextAsync(savePath, htmlCode);

                    ViewData["Message"] = $"Web sayfasının kaynak kodu başarıyla indirildi ve masaüstüne kaydedildi. Dosya Adı: {fileName}";
                }
                catch (WebException ex)
                {
                    ViewData["Error"] = $"Hata: {ex.Message}";
                }
            }

            return View("Index");
        }

        public IActionResult Privacy()
        {
            throw new NotImplementedException();
        }
    }
}