using WebMonk.RazorSharp.Html2RazorSharp;

namespace Html2RazorSharpTester
{
    public static class Program
    {
        static void Main()
        {
            var t = TranslatorBase.CreateTextual("<x>" + "ilya<b>basin</b>" + "</x>", false, true);
            var html = t.ToRazorSharp();

            var x = 6;
            
            //var translatorWm = new Translator(File.ReadAllText(InputWm), true);
            //File.WriteAllText(OutputWm, translatorWm.ToRazorSharp());

            //var translatorMvc = new Translator(File.ReadAllText(InputMvc), true);
            //File.WriteAllText(OutputMvc, translatorMvc.ToRazorSharp());

            //var translator = new Translator(File.ReadAllText(Input), true);
            //File.WriteAllText(Output, translator.ToRazorSharp());
        }

        #region Properties
        public const string InputWm = @"C:\Users\ilyabasin\Desktop\ToTranslateWM.txt";
        public const string OutputWm = @"C:\Users\ilyabasin\Desktop\TranslationResultWM.cs";
        public const string InputMvc = @"C:\Users\ilyabasin\Desktop\ToTranslateMvc.txt";
        public const string OutputMvc = @"C:\Users\ilyabasin\Desktop\TranslationResultMvc.cs";
        public const string OutputHtml = @"C:\Users\ilyabasin\source\repos\TDM\tdm.core\Testers\Html2RazorSharpTester\Output.html";
        public const string Input = @"C:\Users\ilyabasin\source\repos\TDM\tdm.core\Testers\Html2RazorSharpTester\Input.txt";
        public const string Output = @"C:\Users\ilyabasin\source\repos\TDM\tdm.core\Testers\Html2RazorSharpTester\Output.cs";
        #endregion
    }
}
