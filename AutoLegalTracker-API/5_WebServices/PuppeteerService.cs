using Microsoft.AspNetCore.Mvc.RazorPages;
using PuppeteerSharp;

namespace AutoLegalTracker_API._5_WebServices
{
    public class PuppeteerService
    {


        #region Public Methods

        public async Task<IPage?> inicializeService()
        {
            var options = new LaunchOptions
            {
                Headless = false
            };

            Console.WriteLine("Downloading chromium");

            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            IPage page; 

            using (var browser = await Puppeteer.LaunchAsync(options))
            {
                page = await browser.NewPageAsync();
                await page.SetViewportAsync(new ViewPortOptions
                {
                    Width = 1920,
                    Height = 1080
                });
            }

            if(page != null)
            {
                return page;
            }
            return null;
        }

        public async Task<IPage?> logeoService(IPage page, string url, string selectorUsuario, string selectorContra, string usuario, string contra, string selectorEntrar)
        {
            Console.WriteLine("Loggin in");
            await page.GoToAsync(url);
            await page.WaitForSelectorAsync(selectorUsuario);
            await page.TypeAsync(selectorUsuario, usuario);
            await page.TypeAsync(selectorContra, contra);
            await page.ClickAsync(selectorEntrar);
            await page.WaitForTimeoutAsync(3000); // delete this later
            Console.WriteLine("Log exitoso");
            if(page != null)
            {
                return page;
            }
            return null;
        }

        public async Task<IPage?> clickeoService(IPage page, string selector, string selectorParaConfirmacion)
        {
            await page.ClickAsync(selector);
            await page.WaitForTimeoutAsync(3000); // delete this later

            if (await page.WaitForSelectorAsync(selectorParaConfirmacion) != null)
            {
                return page;
            }
            return null;
        }

        public async Task<string[]> stringsService(IPage page, string functionJS)
        {
            var result = await page.EvaluateFunctionAsync(functionJS);
            List<string> list = new();
            foreach (var s in result)
            {
                list.Add(s.ToString());
            }
            return list.ToArray();
        }

        public async Task<IPage> irService(IPage page, string url)
        {
            await page.ClickAsync(url);
            if (page != null)
            {
                return page;
            }
            return null;
        }

        public async Task<uint> innerUintService(IPage page, string selector)
        {
            var res = await page.QuerySelectorAsync(selector);
            if(res != null)
            {
                var jsHandle = await res.GetPropertyAsync("innerHTML");
                var inner = await jsHandle.JsonValueAsync<uint>();
                return inner;
            }
            return 0;
        }
        public async Task<string> innerStringService(IPage page, string selector)
        {
            var res = await page.QuerySelectorAsync(selector);
            if (res != null)
            {
                var jsHandle = await res.GetPropertyAsync("innerHTML");
                var inner = await jsHandle.JsonValueAsync<string>();
                return inner;
            }
            return " ";
        }

        public async Task<string> innerStringService(IPage page, string selector, int pos)
        {
            var res = await page.QuerySelectorAllAsync(selector);
            if (res != null)
            {
                var jsHandle = await res[pos].GetPropertyAsync("innerHTML");
                var inner = await jsHandle.JsonValueAsync<string>();
                return inner;
            }
            return " ";
        }

        #endregion Public Methods
    }
}
