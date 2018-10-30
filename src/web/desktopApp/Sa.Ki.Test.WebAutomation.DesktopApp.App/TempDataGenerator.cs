namespace Sa.Ki.Test.WebAutomation.DesktopApp.App
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class TempDataGenerator
    {
        public static WebContextInfo GenerateContext(int orderNumber, int elsCount)
        {
            var i = orderNumber;

            var wc = new WebContextInfo
            {
                Name = $"Context {i}",
                Description = $"Description for context {i}",
                Tags = new List<string> { $"tag{i}", "contextTag" },
                Elements = new List<WebElementInfo>(),
                Locator = new WebLocatorInfo { LocatorValue = "." }
            };

            for (int j = 1; j < elsCount; j++)
            {
                if (j != 2)
                {
                    var c = new CombinedWebElementInfo
                    {
                        Name = $"Control {i}_{j}",
                        Description = $"Description for control {i}_{j}",
                        Tags = new List<string> { $"tag{j}", "controlTag" },
                        Elements = new List<WebElementInfo>(),
                        Parent = wc,
                        Locator = new WebLocatorInfo { LocatorValue = "./div[@id='control']", IsRelative = true }
                    };
                    wc.Elements.Add(c);


                    var dd = new DropDownElementInfo
                    {
                        Name = $"DropDown {i}_{j}",
                        Description = $"Description for DropDown {i}_{j}",
                        Tags = new List<string> { $"tag{j}", "controlTag" },
                        Parent = wc,
                        Locator = new WebLocatorInfo { LocatorValue = "./div[@id='control']", IsRelative = true },
                        Elements = new List<WebElementInfo>()
                    };
                    wc.Elements.Add(dd);
                    dd.Elements.Add(new WebElementInfo
                    {
                        Name = $"DropDown {i}_{j} Input",
                        Description = $"Description for DropDown {i}_{j} Input",
                        Tags = new List<string> { $"tag{j}", "controlTag" },
                        Parent = wc,
                        Locator = new WebLocatorInfo { LocatorValue = "./div[@id='control']", IsRelative = true },
                        InnerKey = DropDownElementInfo.Keys.Input
                    });
                    dd.Elements.Add(new WebElementInfo
                    {
                        Name = $"DropDown {i}_{j} Option",
                        Description = $"Description for DropDown {i}_{j} Option",
                        Tags = new List<string> { $"tag{j}", "controlTag" },
                        Parent = wc,
                        Locator = new WebLocatorInfo { LocatorValue = "./div[@id='control']", IsRelative = true },
                        InnerKey = DropDownElementInfo.Keys.Option
                    });

                    var rg = new DropDownElementInfo
                    {
                        Name = $"RadioGroup {i}_{j}",
                        Description = $"Description for RadioGroup {i}_{j}",
                        Tags = new List<string> { $"tag{j}", "controlTag" },
                        Parent = wc,
                        Locator = new WebLocatorInfo { LocatorValue = "./div[@id='control']", IsRelative = true },
                        Elements = new List<WebElementInfo>()
                    };
                    wc.Elements.Add(rg);
                    rg.Elements.Add(new WebElementInfo
                    {
                        Name = $"DropDown {i}_{j} Option",
                        Description = $"Description for RadioGroup {i}_{j} Option",
                        Tags = new List<string> { $"tag{j}", "controlTag" },
                        Parent = wc,
                        Locator = new WebLocatorInfo { LocatorValue = "./div[@id='control']", IsRelative = true },
                        InnerKey = RadioGroupElementInfo.Keys.Option
                    });

                    for (int k = 1; k < 5; k++)
                    {
                        var e = new WebElementInfo
                        {
                            Name = $"Element {i}_{j}_{k}",
                            Description = $"Description for element {i}_{j}_{k}",
                            Tags = new List<string> { $"tag{j}", "elementTag" },
                            Parent = c,
                            Locator = new WebLocatorInfo { LocatorValue = "./input[@id='input']", IsRelative = true }
                        };
                        c.Elements.Add(e);
                    }
                }
                else
                {
                    var e = new WebElementInfo
                    {
                        Name = $"Element {i}_{j}",
                        Description = $"Description for element {i}_{j}",
                        Tags = new List<string> { $"tag{j}", "elementTag" },
                        Parent = wc,
                        IsKey = true,
                        Locator = new WebLocatorInfo { LocatorValue = "./input[@id='input']", IsRelative = true }
                    };
                    wc.Elements.Add(e);
                }
            }

            return wc;
        }
    }
}
