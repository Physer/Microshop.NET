using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Web.Utilities;

[HtmlTargetElement(Attributes = "active-link-support")]
public class ActiveClassTagHelper(IHtmlGenerator generator) : AnchorTagHelper(generator)
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (ViewContext?.RouteData?.Values["page"]?.Equals(Page) == false)
            return;

        var existingClasses = output.Attributes["class"].Value.ToString();
        output.Attributes.RemoveAll("class");
        output.Attributes.Add("class", $"{existingClasses} active");
    }
}
