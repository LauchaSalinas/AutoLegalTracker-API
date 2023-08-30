using Microsoft.AspNetCore.Mvc.RazorPages;
using PuppeteerSharp;

namespace AutoLegalTracker_API._5_WebServices
{
    public class PuppeteerService
    {
        private IPage _page;
        private IBrowser _browser;
        

        #region Public Methods

        public async Task<IPage?> InicializeService()
        {
            var options = new LaunchOptions
            {
                Headless = false
            };

            Console.WriteLine("Downloading chromium");

            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            

            _browser = await Puppeteer.LaunchAsync(options);
            
            _page = await _browser.NewPageAsync();
            await _page.SetViewportAsync(new ViewPortOptions
            {
                Width = 1920,
                Height = 1080
            });
            

            if(_page != null)
            {
                return _page;
            }
            return null;
        }

        public async Task<IPage?> LogIn(IPage page, string url, string selectorUsuario, string selectorContra, string usuario, string contra, string selectorEntrar)
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

        public async Task<IPage?> ClickSelector(IPage page, string selector, string selectorParaConfirmacion)
        {
            await page.ClickAsync(selector);
            await page.WaitForTimeoutAsync(3000); // delete this later

            if (await page.WaitForSelectorAsync(selectorParaConfirmacion) != null)
            {
                return page;
            }
            return null;
        }

        public async Task<string[]> GetStringArray(IPage page, string functionJS)
        {
            var result = await page.EvaluateFunctionAsync(functionJS);
            List<string> list = new();
            foreach (var s in result)
            {
                list.Add(s.ToString());
            }
            return list.ToArray();
        }

        public async Task<IPage> GoToUrl(IPage page, string url)
        {
            await page.GoToAsync(url);
            if (page != null)
            {
                return page;
            }
            return null;
        }

        public async Task<uint> GetNumberWithSelector(IPage page, string selector)
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
        public async Task<string> GetTextWithSelector(IPage page, string selector)
        {
            var res = await page.QuerySelectorAsync(selector);
            if (res != null)
            {
                var jsHandle = await res.GetPropertyAsync("innerText");
                var inner = await jsHandle.JsonValueAsync<string>();
                return inner;
            }
            return " ";
        }

        public async Task<string> GetTextWithSelector(IPage page, string selector, int pos)
        {
            var res = await page.QuerySelectorAllAsync(selector);
            if (res != null)
            {
                var jsHandle = await res[pos].GetPropertyAsync("innerText");
                var inner = await jsHandle.JsonValueAsync<string>();
                return inner;
            }
            return " ";
        }

        #endregion Public Methods
    }
}
