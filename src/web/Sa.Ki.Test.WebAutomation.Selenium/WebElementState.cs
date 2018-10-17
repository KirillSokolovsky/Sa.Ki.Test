namespace Sa.Ki.Test.WebAutomation.Selenium
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Flags]
    public enum WebElementState
    {
        None = 0,

        Present = 1,
        Visible = Present | 2,
        Enabled = Visible | 4,

        NotPresent = 8,
        NotVisible = Present | 16,
        Disabled = Visible | 32,

        ReadyForAction = Enabled
    }
}
