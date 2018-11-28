namespace Sa.Ki.Test.WebAutomation
{
    public static class WebElementTypes
    {
        //Base items

        /// <summary>
        /// Single web element
        /// </summary>
        public const string Element = nameof(Element);

        /// <summary>
        /// Context web element. Contains other element
        /// </summary>
        public const string Context = nameof(Context);

        /// <summary>
        /// Context web element. Contains other element
        /// </summary>
        public const string Page = nameof(Page);

        /// <summary>
        /// Element that includes set of child elements
        /// </summary>
        public const string Control = nameof(Control);

        //Custom items

        public const string DropDown = nameof(DropDown);
        public const string RadioGroup = nameof(RadioGroup);

        //Extra
        public const string Frame = nameof(Frame);
        public const string Reference = nameof(Reference);
        public const string Directory = nameof(Directory);
    }
}
