using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace GCCorePSAV.Helpers
{
    [HtmlTargetElement("auto-complete-client")]
    public class AutoCompleteTagHelper : TagHelper
    {
        //Attributes declaration
        private const string IdAttributeName = "id";
        private const string SourceAttributeName = "items-source";
        private const string MinLengthAttributeName = "min-length";

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(SourceAttributeName)]
        public List<Models.PSAVCrud.ClientModel.ClientAutoComplete> ItemsSource { get; set; } = null;

        [HtmlAttributeName(MinLengthAttributeName)]
        public int MinLength { get; set; } = 0;

        //Add behaviour for the TagHelper
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            var sb = new StringBuilder();
            //HTML
            sb.AppendLine("<input id=\"" + Id + "\" name=\"" + Id + "\" autocomplete='on' class='form-control'/>");
            // Script for converting html input into JQuery-UI autocomplete
            sb.AppendLine("<script type='text/javascript'>// <![CDATA[");
            sb.AppendLine("$(\"#" + Id + "\").autocomplete({");
            if (ItemsSource != null && ItemsSource.Count > 0)
            {
                sb.AppendLine("source: ");
                sb.Append("[");
                for (int i = 0; i < ItemsSource.Count; i++)
                {
                    sb.Append(i == 0 ? "\"" + ItemsSource[i].FullName.ToString() + "\"" : ", \"" + ItemsSource[i].FullName.ToString() + "\"");
                }
                sb.Append("]");
            }
            sb.Append(string.Format(", minLength: {0}", MinLength));
            sb.AppendLine("});");
            sb.AppendLine("// ]]></script>");
//Add HTML and script code to output
output.Content.AppendHtml(sb.ToString());
        }
    }
}
