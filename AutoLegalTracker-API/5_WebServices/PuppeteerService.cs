using Microsoft.AspNetCore.Mvc.RazorPages;
using PuppeteerSharp;

namespace AutoLegalTracker_API._5_WebServices
{
    public class PuppeteerService
    {
        private IPage _page;
        private IBrowser _browser;
        

        #region Public Methods

        public async Task InicializeService()
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
        }

        public async Task<bool> LogIn(string url, string selectorUsuario, string selectorContra, string usuario, string contra, string selectorEntrar)
        {
            Console.WriteLine("Loggin in");
            await _page.GoToAsync(url);
            await _page.WaitForSelectorAsync(selectorUsuario);
            await _page.TypeAsync(selectorUsuario, usuario);
            await _page.TypeAsync(selectorContra, contra);
            await _page.ClickAsync(selectorEntrar);
            await _page.WaitForTimeoutAsync(3000); // delete this later
            Console.WriteLine("Log exitoso");
            if(_page != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ClickSelector(string selector, string selectorParaConfirmacion)
        {
            await _page.ClickAsync(selector);
            await _page.WaitForTimeoutAsync(3000); // delete this later

            if (await _page.WaitForSelectorAsync(selectorParaConfirmacion) != null)
            {
                return true;
            }
            return false;
        }

        public async Task<string[]> GetStringArray(string functionJS)
        {
            var result = await _page.EvaluateFunctionAsync(functionJS);
            List<string> list = new();
            foreach (var s in result)
            {
                list.Add(s.ToString());
            }
            return list.ToArray();
        }

        public async Task<bool> GoToUrl(string url)
        {
            await _page.GoToAsync(url);
            if (_page != null)
            {
                return true;
            }
            return false;
        }

        public async Task<uint> GetNumberWithSelector(string selector)
        {
            var res = await _page.QuerySelectorAsync(selector);
            if(res != null)
            {
                var jsHandle = await res.GetPropertyAsync("innerHTML");
                var inner = await jsHandle.JsonValueAsync<uint>();
                return inner;
            }
            return 0;
        }
        public async Task<string> GetPropertyWithSelector(string selector, string property)
        {
            var res = await _page.QuerySelectorAsync(selector);
            if (res != null)
            {
                var jsHandle = await res.GetPropertyAsync(property);
                var inner = await jsHandle.JsonValueAsync<string>();
                return inner;
            }
            return " ";
        }

        public async Task<string> GetPropertyWithSelector(string selector, string property, int pos)
        {
            var res = await _page.QuerySelectorAllAsync(selector);
            if (res != null)
            {
                var jsHandle = await res[pos].GetPropertyAsync(property);
                var inner = await jsHandle.JsonValueAsync<string>();
                return inner;
            }
            return " ";
        }

        #endregion Public Methods
    }
}
