using Microsoft.AspNetCore.Mvc.RazorPages;
using PuppeteerSharp;

namespace AutoLegalTracker_API.WebServices
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
                Headless = false,
                EnqueueAsyncMessages = true,
                IgnoreHTTPSErrors = true,
                // Args = new string[] { "--disable-web-security", "--disable-features=IsolateOrigins,site-per-process", "--disable-extensions", "--disable-gpu", "--no-sandbox", "--disable-dev-shm-usage", "--disable-setuid-sandbox", "--no-zygote" },
            };

            Console.WriteLine("Downloading chromium");

            using var browserFetcher = new BrowserFetcher();
            // await browserFetcher.DownloadAsync();

            _browser = await Puppeteer.LaunchAsync(options);
            
            _page = await _browser.NewPageAsync();
            await _page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99 Safari/537.36");

            _page.RequestFailed += (sender, e) =>
            {
                // Handle request failures
                Console.WriteLine($"Request failed: {e.Request.Method} {e.Request.Url}");
            };

            _page.PageError += (sender, e) =>
            {
                // Handle page errors
                Console.WriteLine($"Page error: {e.Message}");
            };
            

            await _page.SetViewportAsync(new ViewPortOptions
            {
                Width = 1024,
                Height = 650
            });
            await InterceptarPedidosDeFirmaDigitalAsync();
        }

        public async Task<bool> LogIn(string url, string selectorUsuario, string selectorContra, string usuario, string contra, string selectorEntrar)
        {
            Console.WriteLine("Loggin in");
            await _page.GoToAsync(url);
            await _page.WaitForSelectorAsync(selectorUsuario);
            await _page.TypeAsync(selectorUsuario, usuario);
            await _page.TypeAsync(selectorContra, contra);
            await _page.WaitForTimeoutAsync(3000); // delete this later

            await _page.ClickAsync("#modalMostrarMensaje > div > div > div.modal-footer > button:nth-child(3)");
            await _page.WaitForTimeoutAsync(3000); // delete this later

            await _page.ClickAsync(selectorEntrar);
            await _page.WaitForTimeoutAsync(3000); // delete this later
            
            if(_page != null)
            {
                Console.WriteLine("Log exitoso");
                return true;
            }
            return false;
        }

        public async Task<bool> ClickSelector(string selector, string selectorParaConfirmacion)
        {
            await _page.ClickAsync(selector);

            if (await _page.WaitForSelectorAsync(selectorParaConfirmacion) != null)
            {
                return true;
            }
            return false;
        }

        public async Task<List<String>> GetStringArray(string functionJS)
        {
            var result = await _page.EvaluateFunctionAsync(functionJS);
            List<string> list = new();
            foreach (var s in result)
            {
                list.Add(s.ToString());
            }
            return list;
        }

        public async Task<string> GetString(string functionJS)
        {
            var result = await _page.EvaluateFunctionAsync(functionJS);
            return result.ToString();
        }

        public async Task TypeAsync(string selector, string text)
        {
            await _page.TypeAsync(selector, text);
        }

        public async Task ClearInputAsync(string selector)
        {
            await _page.ClickAsync(selector);
            await _page.Keyboard.PressAsync("Backspace");
            await _page.Keyboard.PressAsync("Backspace");
            await _page.Keyboard.PressAsync("Backspace");
            await _page.Keyboard.PressAsync("Backspace");
            await _page.Keyboard.PressAsync("Backspace");
            await _page.Keyboard.PressAsync("Backspace");
            await _page.Keyboard.PressAsync("Backspace");
            await _page.Keyboard.PressAsync("Backspace");
            await _page.Keyboard.PressAsync("Backspace");
            await _page.Keyboard.PressAsync("Backspace");
            await _page.Keyboard.PressAsync("Backspace");
            await _page.Keyboard.PressAsync("Backspace");
        }
        public async Task ExecuteJs(string functionJS)
        {
            await _page.ExposeFunctionAsync(functionJS, () => { });
        }

                public async Task ExecuteJs(string functionJS, string param)
        {
            await _page.EvaluateFunctionAsync(functionJS, new object[] { param });
        }

        public async Task<bool> GoToUrl(string url)
        {
            // try
            // {
                await _page.DisposeAsync();
                _page = await _browser.NewPageAsync();
                await _page.GoToAsync(url, timeout: 5000);
            // }
            // catch
            // {
            //     await _page.WaitForTimeoutAsync(30000);
            //     await _page.ReloadAsync();
            //     await _page.WaitForTimeoutAsync(1000);
            // }
            if (_page != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> GoToUrl(string url, int timeout)
        {
            // try
            // {
            await _page.DisposeAsync();
            _page = await _browser.NewPageAsync();
            await _page.GoToAsync(url, timeout: timeout);
            // }
            // catch
            // {
            //     await _page.WaitForTimeoutAsync(30000);
            //     await _page.ReloadAsync();
            //     await _page.WaitForTimeoutAsync(1000);
            // }
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

        public async Task Wait(int time)
        {
            await _page.WaitForTimeoutAsync(time);
        }

        public async Task InterceptarPedidosDeFirmaDigitalAsync()
        {
            await _page.SetRequestInterceptionAsync(true);
            _page.Request += async (sender, e) =>
            {
                if (e.Request.Url.Contains("version"))
                {
                    //log request
                    var headersConcatenated = e.Request.Headers.Aggregate("", (current, header) => current + (header.Key + ": " + header.Value + "\n"));
                    Console.WriteLine(
                        "NEW REQUEST AT" + DateTime.Now.ToString("h:mm:ss.fff tt")
                        + "\nRequest Body: " + e.Request.Url
                        + "\nRequest Headers: " + headersConcatenated
                    );

                    Console.WriteLine("request with version");
                    ResponseData response = new ResponseData
                    {
                        Status = System.Net.HttpStatusCode.OK,
                        ContentType = "application/json;charset=UTF-8",
                        Body = "{{\"identifier\":\"f8e5f470-bcff-4c50-8fd6-ccfa2fea12d6\",\"version\":\"2.2.9.276\"}}",
                        Headers = new Dictionary<string, object>
                        {
                            {"Access-Control-Allow-Origin", "*" },
                            {"Access-Control-Allow-Methods", "POST, GET, OPTIONS"},
                            {"Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept"},
                            {"Access-Control-Allow-Private-Network", true},
                            {"Content-Length", 75}
                        }

                    };
                    await e.Request.RespondAsync(response);
                    Console.WriteLine(
                        "NEW RESPONSE AT" + DateTime.Now.ToString("h:mm:ss.fff tt")
                        + "\nResponse Code: " + response.Status.ToString()
                        + "\nResponse Body: " + response.Body
                        + "\nResponse Headers: " + response.Headers.ToString()
                        + "\nContent Type: " + response.ContentType
                        );
                }
                else if (e.Request.Url.Contains("updateLicense"))
                {
                    Console.WriteLine("request with updateLicence");
                    ResponseData response = new ResponseData
                    {
                        Status = System.Net.HttpStatusCode.OK,
                        ContentType = "application/json;charset=UTF-8",
                        Body = "{{\"error\" : 1 , \"errorMessage\" : \"OK\" }}",
                        Headers = new Dictionary<string, object>
                        {
                            {"Access-Control-Allow-Origin", "*" },
                            {"Access-Control-Allow-Methods", "POST, GET, OPTIONS"},
                            {"Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept"},
                            {"Access-Control-Allow-Private-Network", true},
                            {"Content-Length", 75}
                        }

                    };
                    await e.Request.RespondAsync(response);
                    Console.WriteLine(
                        "NEW RESPONSE AT" + DateTime.Now.ToString("h:mm:ss.fff tt")
                        + "\nResponse Code: " + response.Status.ToString()
                        + "\nResponse Body: " + response.Body
                        + "\nResponse Headers: " + response.Headers.ToString()
                        + "\nContent Type: " + response.ContentType
                        );
                }
                else
                {
                    await e.Request.ContinueAsync();
                }
            };
        }

        public async Task Select(string selectSelector, string optionSelector)
        {
            await _page.SelectAsync(selectSelector, optionSelector);
        }

        internal async Task refreshNoTimeout()
        {
            await _page.ReloadAsync(timeout: 0);
        }

        #endregion Public Methods
    }
}
